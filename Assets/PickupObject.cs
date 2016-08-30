﻿using UnityEngine;
using System.Collections;

public class PickupObject : MonoBehaviour {
	enum MouseButtons {LEFT = 0, RIGHT = 1, MIDDLE = 2};

	GameObject mainCamera;
	bool carrying;
	GameObject carriedObject;
	GameObject grabber;

	Quaternion rotationOffset;

	public float distance;
	public float smooth;
	public float shootForce;
	public float maxPickupDistance;

	Vector3 previousPosition;

	// Use this for initialization
	void Start () {
		previousPosition = Vector3.zero;
		mainCamera = GameObject.FindWithTag ("FPSCamera");
		grabber = GameObject.FindWithTag ("grabber");
	}
	
	// Update is called once per frame
	void Update () {
		if (carrying) {
			carry (carriedObject);
			drop ();
		} else {
			pickup ();
		}

		if (carriedObject != null) {
			previousPosition = mainCamera.transform.position + mainCamera.transform.forward * distance;
		}
	}

	void carry(GameObject go) {
		go.transform.position = Vector3.Lerp(go.transform.position, 
			grabber.transform.position, 
			Time.deltaTime * smooth);

		go.transform.rotation = Quaternion.Lerp (go.transform.rotation, grabber.transform.rotation * rotationOffset, Time.deltaTime * smooth);

		Rigidbody rigidBody = go.GetComponent<Rigidbody> ();

		rigidBody.velocity = Vector3.zero;
		rigidBody.angularVelocity = Vector3.zero;
	}

	void pickup() {
		if (Input.GetMouseButtonDown (0)) {
			int x = Screen.width / 2;
			int y = Screen.height / 2;

			Camera camera = mainCamera.GetComponent<Camera> ();
			Ray ray = camera.ScreenPointToRay (new Vector3 (x, y));
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				Pickupable p = hit.collider.GetComponent<Pickupable> ();
				if (p != null && Vector3.Distance(p.gameObject.transform.position, mainCamera.transform.position) <= maxPickupDistance) {
					carrying = true;
					carriedObject = p.gameObject;

					rotationOffset = carriedObject.transform.rotation * Quaternion.Inverse (grabber.transform.rotation);

				}
			}
		}
	}

	void drop() {
		Rigidbody rigidBody = carriedObject.GetComponent<Rigidbody> ();
		if (Input.GetMouseButtonUp ((int)MouseButtons.RIGHT)) {
			rigidBody.AddForce (mainCamera.transform.forward * shootForce);
		} else if (Input.GetMouseButtonUp ((int)MouseButtons.LEFT)) {
			//rigidBody.velocity = velocity;

			var positionOfCarriedObject = mainCamera.transform.position + mainCamera.transform.forward * distance;
			Vector3 heading = positionOfCarriedObject - previousPosition;
			float distanceOfDisplacement = heading.magnitude;
			Vector3 direction = heading / distanceOfDisplacement;
			float velocity = distanceOfDisplacement / Time.deltaTime;

			Vector3 scaledDirection = Vector3.Scale(direction, new Vector3(velocity, velocity, velocity));

			rigidBody.velocity = scaledDirection;
		}

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