using UnityEngine;
using System.Collections;

public class MoveHinge : MonoBehaviour {

	States state;
	private GameObject m_player = GameObject.FindWithTag ("Player");

	public float dampingRatio = 10;
	public float linearDrag = 1.0f;
	public float angularDrag = 5.0f;

	private float oldDrag;
	private float oldAngularDrag;

	GameObject mainCamera;
	GameObject grabbedDoor;
	public float maxGrabDistance;
	//bool grabbing;
	Vector3 hitPoint;
	float handDistance;

	//HingeJoint joint;
	SpringJoint spring;

//	public Vector3 anchor;
//	public Vector3 axis;

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
		if (Input.GetMouseButtonDown (0)) {
			int x = Screen.width / 2;
			int y = Screen.height / 2;

			Camera camera = mainCamera.GetComponent<Camera> ();
			Ray ray = camera.ScreenPointToRay (new Vector3 (x, y));
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				MoveHinge p = hit.collider.GetComponent<MoveHinge> ();
				if (p != null) {
					hitPoint = hit.point;
					float localDistance = Vector3.Distance (p.gameObject.transform.position, mainCamera.transform.position);
					if (localDistance <= maxGrabDistance) {
						//grabbing = true;
						grabbedDoor = p.gameObject;

						handDistance = localDistance;

//						anchor = joint.anchor;
//						axis = joint.axis;


						state.draggingDoor = true;

						spring = grabbedDoor.AddComponent<SpringJoint> ();
						spring.transform.position = hitPoint;
						//spring.anchor = hitPoint;
						spring.maxDistance = handDistance;
						spring.damper = dampingRatio;
						spring.connectedBody = grabbedDoor.GetComponent<Rigidbody>();
						spring.connectedAnchor = hit.transform.InverseTransformPoint (hitPoint);

						oldDrag = spring.connectedBody.drag;
						oldAngularDrag = spring.connectedBody.angularDrag;

						spring.connectedBody.drag = linearDrag;
						spring.connectedBody.angularDrag = angularDrag;

						//objectRot = carriedObject.transform.localRotation;
						//currentDistance = Vector3.Distance (p.gameObject.transform.position, mainCamera.transform.position);
						//rotationOffset = carriedObject.transform.rotation * Quaternion.Inverse (mainCamera.transform.rotation);
					}
				}
			}
		} else if (Input.GetMouseButton (0)) {
			
			if (grabbedDoor != null) {
			 	//Destroy (joint);

				Ray ray = getCameraRay ();
				//Vector3 hand = ray.GetPoint (handDistance);
				//Vector3 hand = ray.GetPoint (handDistance);

				//Vector3 point = mainCamera.transform.position + mainCamera.transform.forward * handDistance;

//				spring.transform.position = hand;
//
//				spring.damper = dampingRatio;
//				//spring.frequency = frequency;
//				spring.maxDistance = distance;
//
//				spring.connectedBody = grabbedDoor.GetComponent<Rigidbody>();

				//spring.connectedAnchor = hand;

				//Vector3 heading = joint.anchor - hand;

				//float angle = Vector3.Angle (axis, heading);

				//spring.transform.position = hand;



				//Debug.Log ("p" + point);
				Debug.Log ("connectedAnchor" + ray.GetPoint (handDistance));
//				Debug.Log ("heading" + heading);
//				Debug.Log ("axis" + axis);

				//angle should be calculated from direction of mouse and speed

				//grabbedDoor.transform.RotateAround (grabbedDoor.transform.TransformPoint(anchor), grabbedDoor.transform.TransformDirection(axis), angle);
				//axis = heading;



				//Camera mainCamera = FindCamera ();

				//while (Input.GetMouseButton (0)) {
					//Ray newRay = getCameraRay();
				spring.transform.position = ray.GetPoint (handDistance);
					//yield return null;
				//}





			}
		} else if (Input.GetMouseButtonUp (0)) {
//			joint = grabbedDoor.AddComponent<HingeJoint> ();
//			joint.anchor = anchor;
//			joint.axis = axis;
			//must also set limits

			if (spring.connectedBody) {    
				spring.connectedBody.drag = oldDrag;
				spring.connectedBody.angularDrag = oldAngularDrag;
				spring.connectedBody = null;
			}
			handDistance = -1;
			grabbedDoor = null;
			state.draggingDoor = false;
		}
	}


//	Vector3 getCameraVectorParallelToDoorPlane () {
//		Vector3 cameraVector = getCameraVector ();
//

	//}

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
