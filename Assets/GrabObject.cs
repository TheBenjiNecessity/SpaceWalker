using UnityEngine;
using System.Collections;

public class GrabObject : MonoBehaviour {

	enum MouseButtons {LEFT = 0, RIGHT = 1, MIDDLE = 2};

	GameObject mainCamera;
	bool grabbing;
	GameObject grabbedObject;
	public float maxGrabDistance;
	public float smooth;
	public float jumpForce;
	public float jumpableRadius = 2.0f;

	// Use this for initialization
	void Start () {
		mainCamera = GameObject.FindWithTag ("FPSCamera");
	}
	
	// Update is called once per frame
	void Update () {
		if (grabbing) {
			clutch (grabbedObject);
			letgo ();
		} else {
			grab ();
		}

		canJump ();
	}

	//
	void clutch (GameObject go) {
		if (go != null) {
			this.transform.position = Vector3.Lerp (this.transform.position, 
				go.transform.position + this.transform.forward * 1, Time.deltaTime * smooth);

			Rigidbody rigidBody = this.GetComponent<Rigidbody> ();
			rigidBody.velocity = Vector3.zero;
			rigidBody.angularVelocity = Vector3.zero;
		}
	}

	//initiates 'cluching'
	void grab () {
		if (Input.GetMouseButtonDown (0)) {
			int x = Screen.width / 2;
			int y = Screen.height / 2;

			Camera camera = mainCamera.GetComponent<Camera> ();
			Ray ray = camera.ScreenPointToRay (new Vector3 (x, y));
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				Grabable g = hit.collider.GetComponent<Grabable> ();
				if (g != null && Vector3.Distance(g.gameObject.transform.position, mainCamera.transform.position) <= maxGrabDistance) {
					grabbing = true;
					grabbedObject = g.gameObject;

					Rigidbody thisr = this.GetComponent<Rigidbody> ();
					thisr.isKinematic = true;
					thisr.velocity = Vector3.zero;
					thisr.angularVelocity = Vector3.zero;
				}
			}
		}
	}

	void letgo () {
		if (Input.GetMouseButton((int)MouseButtons.LEFT) || Input.GetKey (KeyCode.LeftShift)) {
			jump ();
		}

		if (!Input.GetMouseButton ((int)MouseButtons.LEFT) && !Input.GetKey (KeyCode.LeftShift)) {
			stopGrabbing ();
		}
	}

	void canJump () {
		Collider[] objects = Physics.OverlapSphere(transform.position, jumpableRadius);

		if (objects.Length > 1) {
			//if I'm close to an object that is 'Grabbable' then
			//   I should be able to move in the direction I'm pointing
			//if I'm close to a jumpable object then
			//   
			Collider bestCollider;
			Grabable grabable;
			jumpableObject jumpable;
			foreach (Collider collider in objects) {
				grabable = collider.GetComponent<Grabable> ();
				if (grabable != null) {
					bestCollider = collider;
					break;
				}

				jumpable = collider.GetComponent<jumpableObject> ();
			}

			if (grabable != null) {
				jump ();
			} else if (jumpable != null){

			}
		}
	}

	void jump () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			Rigidbody rigidBody = this.GetComponent<Rigidbody> ();
			rigidBody.isKinematic = false;
			rigidBody.AddForce (mainCamera.transform.forward * jumpForce);
			rigidBody.angularVelocity = Vector3.zero;
			stopGrabbing ();
		}
	}

	void stopGrabbing () {
		grabbing = false;
		grabbedObject = null;
	}
}
