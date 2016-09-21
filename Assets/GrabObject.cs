/**
 * Notes:
 * 	types of methods (methods starting with these prefixes have these functions):
 * 		tryTo...: a method that will check if the user has a given input and then initiate
 * 			a given action
 * 		continueTo...: a method that performs an ongoing action
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class Movement {
	public enum limb {LEFTHAND, RIGHTHAND, LEFTFOOT, RIGHTFOOT, NONE};
	public enum direction {FORWARD, BACK, LEFT, RIGHT, UP, DOWN, NONE};

	public limb movementLimb;
	public direction movementDirection;

	public static Dictionary<KeyCode, Dictionary<KeyCode, Movement>> movements = new Dictionary<KeyCode, Dictionary<KeyCode, Movement>>(){
		{
			KeyCode.W, new Dictionary<KeyCode, Movement>(){
				{KeyCode.Q, new Movement(KeyCode.W, KeyCode.Q)},
				{KeyCode.E, new Movement(KeyCode.W, KeyCode.E)},
				{KeyCode.Z, new Movement(KeyCode.W, KeyCode.Z)},
				{KeyCode.C, new Movement(KeyCode.W, KeyCode.C)}
			}
		},
		{
			KeyCode.A, new Dictionary<KeyCode, Movement>(){
				{KeyCode.Q, null},
				{KeyCode.E, new Movement(KeyCode.A, KeyCode.E)},
				{KeyCode.Z, null},
				{KeyCode.C, new Movement(KeyCode.A, KeyCode.C)}
			}
		},
		{
			KeyCode.S, new Dictionary<KeyCode, Movement>(){
				{KeyCode.Q, new Movement(KeyCode.S, KeyCode.Q)},
				{KeyCode.E, new Movement(KeyCode.S, KeyCode.E)},
				{KeyCode.Z, new Movement(KeyCode.S, KeyCode.Z)},
				{KeyCode.C, new Movement(KeyCode.S, KeyCode.C)}
			}
		},
		{
			KeyCode.Space, new Dictionary<KeyCode, Movement>(){
				{KeyCode.Q, null},
				{KeyCode.E, null},
				{KeyCode.Z, new Movement(KeyCode.Space, KeyCode.Z)},
				{KeyCode.C, new Movement(KeyCode.Space, KeyCode.C)}
			}
		},
		{
			KeyCode.LeftShift, new Dictionary<KeyCode, Movement>(){
				{KeyCode.Q, new Movement(KeyCode.LeftShift, KeyCode.Q)},
				{KeyCode.E, new Movement(KeyCode.LeftShift, KeyCode.E)},
				{KeyCode.Z, null},
				{KeyCode.C, null}
			}
		},
		{
			KeyCode.D, new Dictionary<KeyCode, Movement>(){
				{KeyCode.Q, new Movement(KeyCode.D, KeyCode.Q)},
				{KeyCode.E, null},
				{KeyCode.Z, new Movement(KeyCode.D, KeyCode.Z)},
				{KeyCode.C, null}
			}
		}
	};

	public Movement (KeyCode movementDirection, KeyCode movementLimb) {
		this.movementLimb = translateLimbKeyToLimbEnum(movementLimb);
		this.movementDirection = translateDirectionKeyToDirectionEnum(movementDirection);
	}

	public void applyForceToPlayer (GameObject gameObject, List<jumpableObject> jObjectsNearPlayer) {
		//TODO: there should be a limit to the amount of force added
		Transform transform = gameObject.transform;
		Rigidbody body = gameObject.GetComponent<Rigidbody> ();
		body.isKinematic = false;
		Vector3 worldVector = translateVectorDirectionToTransformVector (transform);
		Vector3 worldLimbPosition = getForcePositionForLimb (movementLimb);
//		Vector3 localLimbPosition = transform.InverseTransformPoint(worldLimbPosition);
//		Vector3 localVector = transform.InverseTransformDirection(worldVector);

		//if (hasValidJObject (vector, jObjectsNearPlayer)) {
		body.AddForceAtPosition(worldVector * 1f, worldLimbPosition);
		//}
	}

	private bool hasValidJObject (Vector3 transformVector, List<jumpableObject> jObjectsNearPlayer) {
		foreach (jumpableObject jObject in jObjectsNearPlayer) {
			foreach (Vector3 normal in jObject.jumpNormals) {
				if (Vector3.Angle (transformVector, normal) < 10.0) {//should be vectorAngleTolerance
					return true;
				}
			}
		}
		return false;
	}

	private Vector3 translateVectorDirectionToTransformVector (Transform localTransform) {
		switch (movementDirection) {
			case direction.FORWARD:
			return localTransform.forward;
			case direction.BACK:
			return -localTransform.forward;
			case direction.LEFT:
			return -localTransform.right;
			case direction.RIGHT:
			return localTransform.right;
			case direction.UP:
			return localTransform.up;
			case direction.DOWN:
			return -localTransform.up;
			default:
			return Vector3.zero;
		}
	}

	private limb translateLimbKeyToLimbEnum(KeyCode key) {
		switch (key) {
			case KeyCode.Q:
				return limb.LEFTHAND;
			case KeyCode.E:
				return limb.RIGHTHAND;
			case KeyCode.Z:
				return limb.LEFTFOOT;
			case KeyCode.C:
				return limb.RIGHTFOOT;
			default:
				return limb.NONE;
		};
	}

	private direction translateDirectionKeyToDirectionEnum(KeyCode key) {
		switch (key) {
		case KeyCode.W:
			return direction.FORWARD;
		case KeyCode.A:
			return direction.LEFT;
		case KeyCode.S:
			return direction.BACK;
		case KeyCode.D:
			return direction.RIGHT;
		case KeyCode.LeftShift:
			return direction.DOWN;
		case KeyCode.Space:
			return direction.UP;
		default:
			return direction.NONE;
		};
	}

	private Vector3 getForcePositionForLimb(limb forceLimb) {
		Vector3 position = Vector3.zero;

		switch (forceLimb) {
			case limb.LEFTFOOT:
				position = GameObject.Find ("leftFoot").transform.position;
				break;
			case limb.RIGHTFOOT:
				position = GameObject.Find ("rightFoot").transform.position;
				break;
			case limb.LEFTHAND:
				position = GameObject.Find ("leftHand").transform.position;
				break;
			case limb.RIGHTHAND:
				position = GameObject.Find ("rightHand").transform.position;
				break;
		};

		return position;
	}
}

public class GrabObject : MonoBehaviour {

	enum MouseButtons {LEFT = 0, RIGHT = 1, MIDDLE = 2};
	private States state;

	GameObject mainCamera;
	GameObject grabbedObject;
	GameObject player;

	public float maxGrabDistance;
	public float smooth;
	public float jumpForce = 250f;
	public float jumpableRadius = 2.0f;
	public float vectorAngleTolerance = 10.0f;
	public float torque;
	public float force;

	public bool shouldSmooth;

	public float playerRotationSmooth = 10f;

	private Quaternion playerRot;

	void Start () {
		state = States.Instance;
		mainCamera = GameObject.FindWithTag ("FPSCamera");
		player = GameObject.FindWithTag ("Player");
	}
		
	void Update () {
		//check if the player is currently holding onto something
		if (isGrabbing ()) {
			continueToGrab ();
			tryToLetGo ();
		} else {
			tryToGrab ();
		}

		tryToMove ();
		//tryToJump ();
	}

	/* ========================== Convenience Functions ========================== */
	#region Convenience Functions
	bool isLeftMouseButtonBeingHeldDown () { return Input.GetMouseButton ((int)MouseButtons.LEFT); }
	bool isRightMouseButtonBeingHeldDown () { return Input.GetMouseButton ((int)MouseButtons.RIGHT); }

	bool wasLeftMouseButtonClicked () { return Input.GetMouseButtonDown ((int)MouseButtons.LEFT); }
	bool wasRightMouseButtonClicked () { return Input.GetMouseButtonDown ((int)MouseButtons.RIGHT); }

	private bool isGrabbing () { return grabbedObject != null; }

	private Transform getPlayerTransform () {
		GameObject player = GameObject.FindWithTag ("Player");//Will grabbing this every time be slow?
		return player.transform;
	}

	public GameObject getGrabbedObject () {
		return grabbedObject;
	}
	#endregion

	//called in update if player is in state of grabbing
	//continue grabbing the grabable object
	void continueToGrab () {
		if (grabbedObject != null) {
			this.transform.position = Vector3.Lerp (this.transform.position,
				grabbedObject.transform.position + this.transform.forward * 1, Time.deltaTime * smooth);

			Rigidbody rigidBody = this.GetComponent<Rigidbody> ();
			rigidBody.velocity = Vector3.zero;
			rigidBody.angularVelocity = Vector3.zero;
		}
	}

	//initiates 'grabbing'

	/// <summary>
	/// Called in the update if the splayer is about to grab something.
	/// Try to find a grabbable object near the player under the reticle
	/// and initiate grabbing if a grabbable object was found.
	/// </summary>
	void tryToGrab () {
		if (wasLeftMouseButtonClicked ()) {//TODO: will this method allow the player to grab something as well as hold items?
			int x = Screen.width / 2;
			int y = Screen.height / 2;

			Camera camera = mainCamera.GetComponent<Camera> ();
			Ray ray = camera.ScreenPointToRay (new Vector3 (x, y));
			RaycastHit hit;
			if (Physics.Raycast (ray, out hit)) {
				Grabable g = hit.collider.GetComponent<Grabable> ();
				if (g != null && Vector3.Distance(g.gameObject.transform.position, mainCamera.transform.position) <= maxGrabDistance) {
					grabbedObject = g.gameObject;
					state.grabbingObject = true;
					Rigidbody thisr = this.GetComponent<Rigidbody> ();
					thisr.isKinematic = true;
					thisr.velocity = Vector3.zero;
					thisr.angularVelocity = Vector3.zero;
				}
			}
		}
	}

	void tryToLetGo () {
		if (!isLeftMouseButtonBeingHeldDown ()) {// && !Input.GetKey (KeyCode.LeftShift)) {
			stopGrabbing ();
		}
	}

	void tryToMove () {
		if (grabbedObject == null) { //if the player is not grabbing something
			if (hasNotPressedMovementKeysExtended ()) {
				return;
			}

			if (!Input.GetKey (KeyCode.Q)
			    && !Input.GetKey (KeyCode.E)
			    && !Input.GetKey (KeyCode.Z)
			    && !Input.GetKey (KeyCode.C)) {
				return;
			}

			//TODO: this movement should be just like: w adds force to pitch forward.

			List<jumpableObject> jObjectsNearPlayer = getAllJumpableObjectsNearPlayer ();

			foreach (KeyCode key in new KeyCode[]{KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.LeftShift, KeyCode.Space}) {
				if (Input.GetKey (key)) {
					foreach (KeyCode keyDown in new KeyCode[]{KeyCode.Q, KeyCode.E, KeyCode.Z, KeyCode.C}) {
						if (Input.GetKey (keyDown)) {
							Movement movement = Movement.movements [key] [keyDown];
							movement.applyForceToPlayer (GameObject.FindWithTag ("Player"), jObjectsNearPlayer);
						}
					}
				}
			}
		} else { //if the player is grabbing something
			tryToJump ();

			if (hasNotPressedMovementKeys()) {
				return;
			}
				
			float pitch = 0f;
			float roll = 0f;

			if (Input.GetKey (KeyCode.W)) {
				pitch += 0.75f;
			}

			if (Input.GetKey (KeyCode.A)) {
				roll += 0.75f;
			}

			if (Input.GetKey (KeyCode.S)) {
				pitch += -0.75f;
			}

			if (Input.GetKey (KeyCode.D)) {
				roll += -0.75f;
			}

			if (shouldSmooth) {
				playerRot *= Quaternion.Euler (pitch, 0f, roll);
				player.transform.localRotation = Quaternion.Slerp (player.transform.localRotation, playerRot, playerRotationSmooth * Time.deltaTime);
			} else {
				player.transform.localRotation *= Quaternion.Euler (pitch, 0f, roll);
			}
		}
	}

	void tryToJump () {
		if (Input.GetKeyDown (KeyCode.Space) && grabbedObject != null) {
			Rigidbody rigidBody = this.GetComponent<Rigidbody> ();
			rigidBody.isKinematic = false;
			rigidBody.AddForce (mainCamera.transform.forward * jumpForce);
			rigidBody.angularVelocity = Vector3.zero;
			stopGrabbing ();
		}
	}

	void stopGrabbing () {
		grabbedObject = null;
		state.grabbingObject = false;
	}

	List<jumpableObject> getAllJumpableObjectsNearPlayer() {
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

	public static bool hasNotPressedMovementKeys () {
		return !Input.GetKey (KeyCode.W) && !Input.GetKey (KeyCode.A) && !Input.GetKey (KeyCode.S) && !Input.GetKey (KeyCode.D);
	}

	public static bool hasNotPressedMovementKeysExtended () {
		return hasNotPressedMovementKeys () && !Input.GetKey (KeyCode.LeftShift) && !Input.GetKey (KeyCode.Space);
	}
}
