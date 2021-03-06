﻿using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class PickupObject : MonoBehaviour {
	enum MouseButtons {LEFT = 0, RIGHT = 1, MIDDLE = 2};

	States state;
	GameObject mainCamera;
	bool carrying;
	GameObject carriedObject;

	Quaternion rotationOffset;

	public float distance;
	public float smooth;
	public float shootForce;
	public float rotationSmoothing = 5f;
	public float scrollSensitivity = 0.1f;
	public float XSensitivity = 2f;
	public float YSensitivity = 2f;

	public float maxPickupDistance;
	private float minCarryDistance = 0.85f;

	private float currentDistance = 0;

	//private Quaternion objectRot;

	Vector3 previousPosition;

	// Use this for initialization
	void Start () {
		state = States.Instance;
		previousPosition = Vector3.zero;
		mainCamera = GameObject.FindWithTag ("FPSCamera");
	}
	
	// Update is called once per frame
	void Update () {
		if (carrying) {
			carry ();
			drop ();
		} else {
			pickup ();
		}

		if (carriedObject != null) {
			previousPosition = mainCamera.transform.position + mainCamera.transform.forward * distance;
		}
	}

	/// <summary>
	/// A method called every frame that handles carrying in an on going way.
	/// </summary>
	void carry() {
		if (Input.GetKey (KeyCode.R) && carriedObject != null) {
			//rotate the object around based on mouse movement
			float xRot = States.getMouseXAxis () * XSensitivity;
			float yRot = States.getMouseYAxis () * YSensitivity;

			state.rotatingObject = true;
			carriedObject.transform.RotateAround (carriedObject.transform.position, mainCamera.transform.up, -xRot);
			carriedObject.transform.RotateAround (carriedObject.transform.position, mainCamera.transform.right, yRot);
			//TODO: for bonus points, let the player let go of the object while rotating and have it still rotate.
		} else {
			state.rotatingObject = false;
			//move the object around based on where the player is looking
			int x = Screen.width / 2;
			int y = Screen.height / 2;
			Camera camera = mainCamera.GetComponent<Camera> ();
			Ray ray = camera.ScreenPointToRay (new Vector3 (x, y));
			Vector3 direction = ray.direction;

			//handle how far the object is by scrolling
			float scroll = Input.GetAxis ("Mouse ScrollWheel");
			if (scroll > 0) {
				currentDistance -= scrollSensitivity;
			} else if (scroll < 0) {
				currentDistance += scrollSensitivity;
			}

			if (currentDistance >= maxPickupDistance || currentDistance <= minCarryDistance) {
				if (scroll > 0) {
					currentDistance += scrollSensitivity;
				} else if (scroll < 0) {
					currentDistance -= scrollSensitivity;
				}
			}

			carriedObject.transform.position = Vector3.Lerp(carriedObject.transform.position, mainCamera.transform.position + direction.normalized * currentDistance, Time.deltaTime * smooth);
			carriedObject.transform.rotation = Quaternion.Lerp (carriedObject.transform.rotation, mainCamera.transform.rotation * rotationOffset, Time.deltaTime * smooth);

			Rigidbody rigidBody = carriedObject.GetComponent<Rigidbody> ();

			rigidBody.velocity = Vector3.zero;
			rigidBody.angularVelocity = Vector3.zero;
		}
	}

	/// <summary>
	/// Pickup an object. Find the first object along a ray cast 
	/// from the camera and parent it to the player to simulate carrying.
	/// </summary>
	void pickup() {
		if (Input.GetMouseButtonDown (0)) {
			int x = Screen.width / 2;
			int y = Screen.height / 2;

			Camera camera = mainCamera.GetComponent<Camera> ();
			Ray ray = camera.ScreenPointToRay (new Vector3 (x, y));
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				Pickupable p = hit.collider.GetComponent<Pickupable> ();
				if (p != null) {
					float localDistance = Vector3.Distance (p.gameObject.transform.position, mainCamera.transform.position);
					if (localDistance <= maxPickupDistance && localDistance >= minCarryDistance) {
						carrying = true;
						carriedObject = p.gameObject;

						//objectRot = carriedObject.transform.localRotation;
						currentDistance = Vector3.Distance (p.gameObject.transform.position, mainCamera.transform.position);
						rotationOffset = carriedObject.transform.rotation * Quaternion.Inverse (mainCamera.transform.rotation);
					}
				}
			}
		}
	}

	/// <summary>
	/// Attempt to drop the object. If the right mouse button is pressed then throw the object.
	/// If the left mouse button is released, let go of the object.
	/// </summary>
	void drop() {
		if (Input.GetMouseButtonDown ((int)MouseButtons.RIGHT)) {
			throwObject ();
		} else if (Input.GetMouseButtonUp ((int)MouseButtons.LEFT)) {
			letGo ();
		}
	}

	/// <summary>
	/// Throws the object.
	/// </summary>
	void throwObject () {
		Rigidbody rigidBody = carriedObject.GetComponent<Rigidbody> ();
		rigidBody.isKinematic = false;
		carriedObject.transform.parent = null;
		rigidBody.AddForce (mainCamera.transform.forward * shootForce);

		release ();
	}

	/// <summary>
	/// Lets the object go.
	/// </summary>
	void letGo () {
		Rigidbody rigidBody = carriedObject.GetComponent<Rigidbody> ();
		var positionOfCarriedObject = mainCamera.transform.position + mainCamera.transform.forward * distance;
		Vector3 heading = positionOfCarriedObject - previousPosition;
		float distanceOfDisplacement = heading.magnitude;
		Vector3 direction = heading / distanceOfDisplacement;
		float velocity = distanceOfDisplacement / Time.deltaTime;

		Vector3 scaledDirection = Vector3.Scale(direction, new Vector3(velocity, velocity, velocity));

		rigidBody.velocity = scaledDirection;

		release ();
	}

	/// <summary>
	/// Unparent the object and set the instance to null.
	/// </summary>
	void release () {
		if (Input.GetMouseButtonUp ((int)MouseButtons.LEFT)
			|| Input.GetMouseButtonDown ((int)MouseButtons.RIGHT)) {
			carrying = false;
			carriedObject.transform.parent = null;
			carriedObject = null;
			previousPosition = Vector3.zero;
			rotationOffset = Quaternion.identity;
		}
	}
}