using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GrabObject : MonoBehaviour {

	enum MouseButtons {LEFT = 0, RIGHT = 1, MIDDLE = 2};

	GameObject mainCamera;
	bool grabbing;
	GameObject grabbedObject;
	public float maxGrabDistance;
	public float smooth;
	public float jumpForce;
	public float jumpableRadius = 2.0f;

	/*
		This is how it works:
		movement:
		mouse
			look around like normal. 
			left and right causes whole body yaw rotation
			up and down causes head camera pitch rotation (with clamping)

		w pitch body forward (only when grabbing)
		s pitch body backward (only when grabbing)
		a roll body left (only when grabbing)
		d roll body right (only when grabbing)

		space move 'feet'-ward
		shift move 'head'-ward
		q move (strafe) left
		e move (strafe) right
	*/

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
		Vector3 jumpDirection;
		GameObject player = GameObject.FindWithTag ("Player");
		if (Input.GetKeyDown (KeyCode.Space)) {
			
		} else if (Input.GetKeyDown (KeyCode.Q)) {

		} else if (Input.GetKeyDown (KeyCode.E)) {

		} else if (Input.GetKeyDown (KeyCode.LeftControl)) {

		}

		if (jumpDirection != null) {

		}

		if (Input.GetKeyDown (KeyCode.Space)) {
			Collider[] objects = Physics.OverlapSphere (transform.position, jumpableRadius);

			if (objects.Length > 1) { 
				Collider bestCollider;
				Grabable grabable = null;
				List<jumpableObject> jumpableObjects = new List<jumpableObject> ();
				foreach (Collider collider in objects) {
					//grabable = collider.GetComponent<Grabable> ();
					if (grabable != null) {
						bestCollider = collider;
						break;
					}

					jumpableObject jumpable = collider.GetComponent<jumpableObject> ();
					if (jumpable != null) {
						jumpableObjects.Add (jumpable);
					}
				}

				if (grabable != null) {
					jump ();
				} else if (jumpableObjects.Count > 0) {
					List<Vector3> normals = new List<Vector3> ();
					GameObject playerObject = GameObject.FindWithTag ("Player");
					Vector3 playerPosition = playerObject.transform.position;
					foreach (jumpableObject jo in jumpableObjects) {
						normals.AddRange (jo.getNormals(playerPosition));
					}

					var normal = normals [0];

				}
			}
		}
	}

	List<jumpableObject> getJumpableObjectAtPoint (Vector3 point) {
		List<jumpableObject> result = new List<jumpableObject> ();
		Collider[] objects = Physics.OverlapSphere (transform.position, jumpableRadius);
		foreach (Collider collider in objects) {
			jumpableObject jumpable = collider.GetComponent<jumpableObject> ();
			if (jumpable != null) {
				result.Add (jumpable);
			}
		}
		return result;
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
