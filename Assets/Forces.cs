using UnityEngine;
using System.Collections;

public class Forces
{
	States state;
	GameObject mainCamera;

	public Vector3 cameraForward
	{
		get { return mainCamera.transform.forward; }
	}

	public Vector3 cameraRight
	{
		get { return mainCamera.transform.right; }
	}

	//public 

	public Forces () {
		state = States.Instance;
		mainCamera = GameObject.FindWithTag ("FPSCamera");
	}

	/// <summary>
	/// Adds a force to a rigidbody where the Vector of the force is relative to
	/// the player's field of view.
	/// </summary>
	/// <param name="magnitude">Magnitude.</param>
	/// <param name="rigidbody">Rigidbody.</param>
//	public void addShootForceToRigidbody (float magnitude, Rigidbody rigidbody, Vector3 point = null?) {
//		Vector3 forward = cameraForward;
//		float xRot = States.getMouseXAxis ();
//		float yRot = States.getMouseYAxis ();
//
//
//	}

	public Ray getCameraRay() {
		int x = Screen.width / 2;
		int y = Screen.height / 2;

		return getCamera ().ScreenPointToRay (new Vector3 (x, y));
	}

	public Vector3 getCameraVector() {
		return getCameraRay().direction;
	}

	public Camera getCamera () {
		return mainCamera.GetComponent<Camera> ();
	}

	public GameObject getCameraGameObject () {
		if (mainCamera == null) {
			mainCamera = GameObject.FindWithTag ("FPSCamera");
		}

		return mainCamera;
	}
}

