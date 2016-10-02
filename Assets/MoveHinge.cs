using UnityEngine;
using System.Collections;

public class MoveHinge : MonoBehaviour {

	States state;
	//private GameObject m_player = GameObject.FindWithTag ("Player");

	public float XSensitivity = 20f;
	public float YSensitivity = 20f;

	public float dampingRatio = 10;
	public float linearDrag = 1.0f;
	public float angularDrag = 5.0f;

	private float oldDrag;
	private float oldAngularDrag;

	GameObject mainCamera;
	GameObject grabbedDoor;
	public float maxGrabDistance;
	//bool grabbing;
	Vector3 localHitPoint;
	Vector3 worldHitPoint;
	float handDistance;

	//HingeJoint joint;
	//SpringJoint spring;

	public Vector3 anchor;
	public Vector3 axis;

	public Vector3 closedDoorDirection;
	public Vector3 fullyOpenDoorDirection;

	public Vector3 doorDirection;//the direction that the side of the door opposite the 

	// Use this for initialization
	void Start () {
		state = States.Instance;
		mainCamera = GameObject.FindWithTag ("FPSCamera");
		//joint = this.GetComponent<HingeJoint> ();
	}

	// Update is called once per frame
	void Update () {
		if (grabbedDoor != null) {
			moveDoor ();
			releaseDoor ();
		} else {
			selectDoor ();
		}
	}

	void selectDoor () {
		if (Input.GetMouseButtonDown (0)) {
			int x = Screen.width / 2;
			int y = Screen.height / 2;

			Camera camera = mainCamera.GetComponent<Camera> ();
			Ray ray = camera.ScreenPointToRay (new Vector3 (x, y));
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				MoveHinge p = hit.collider.GetComponent<MoveHinge> ();
				if (p != null) {
					float localDistance = Vector3.Distance (p.gameObject.transform.position, mainCamera.transform.position);
					if (localDistance <= maxGrabDistance) {
						grabbedDoor = p.gameObject;

						localHitPoint = grabbedDoor.transform.InverseTransformPoint(hit.point);
						worldHitPoint = hit.point;
						handDistance = localDistance;
						state.draggingDoor = true;

						Rigidbody grabbedDoorRigidbody = grabbedDoor.GetComponent<Rigidbody> ();
						if (grabbedDoorRigidbody) {
							grabbedDoorRigidbody.angularDrag = 0f;
							grabbedDoorRigidbody.velocity = Vector3.zero;
						}
					}
				}
			}
		}
	}

	void moveDoor () {
		if (Input.GetMouseButton (0)) {
			if (grabbedDoor != null) {
				float xRot = -States.getMouseXAxis () * XSensitivity;
				float yRot = States.getMouseYAxis () * YSensitivity;

				Debug.Log ("xRot" + xRot);
				Debug.Log ("yRot" + yRot);

				Vector3 cameraForward = mainCamera.transform.forward;
				Vector3 cameraRight = mainCamera.transform.right;

				Vector3 blue = new Vector3 (cameraForward.x * xRot, cameraForward.y * xRot, cameraForward.z * xRot);
				Vector3 red = new Vector3 (cameraForward.x * yRot, cameraForward.y * yRot, cameraForward.z * yRot);

				Vector3 forceVector = blue + red;

				Rigidbody grabbedDoorRigidbody = grabbedDoor.GetComponent<Rigidbody> ();
				if (grabbedDoorRigidbody) {
					grabbedDoorRigidbody.isKinematic = false;
					grabbedDoorRigidbody.AddForceAtPosition (forceVector, worldHitPoint);
				}
			}
		}
	}

	void releaseDoor () {
		if (Input.GetMouseButtonUp (0)) {
			handDistance = -1;
			grabbedDoor = null;
			state.draggingDoor = false;
		}
	}

	Vector3 getCameraVector() {
		return getCameraRay().direction;
	}

	Ray getCameraRay() {
		int x = Screen.width / 2;
		int y = Screen.height / 2;

		Camera camera = mainCamera.GetComponent<Camera> ();
		return camera.ScreenPointToRay (new Vector3 (x, y));
	}
}
