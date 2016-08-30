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

public class KeyCombination {
	public KeyCode key;
	public List<KeyCode> keysDown;
	public List<Movement> movements;

	#region statics
	private static List<KeyCode> QEZC = new List<KeyCode>(){KeyCode.Q, KeyCode.E, KeyCode.Z, KeyCode.C};

	private static List<KeyCode> QEZ = new List<KeyCode>(){KeyCode.Q, KeyCode.E, KeyCode.Z};
	private static List<KeyCode> QEC = new List<KeyCode>(){KeyCode.Q, KeyCode.E, KeyCode.C};
	private static List<KeyCode> QZC = new List<KeyCode>(){KeyCode.Q, KeyCode.Z, KeyCode.C};
	private static List<KeyCode> QE = new List<KeyCode>(){KeyCode.Q, KeyCode.E};
	private static List<KeyCode> QZ = new List<KeyCode>(){KeyCode.Q, KeyCode.Z};
	private static List<KeyCode> QC = new List<KeyCode>(){KeyCode.Q, KeyCode.C};
	private static List<KeyCode> Q = new List<KeyCode>(){KeyCode.Q};

	private static List<KeyCode> EZC = new List<KeyCode>(){KeyCode.E, KeyCode.Z, KeyCode.C};
	private static List<KeyCode> EZ = new List<KeyCode>(){KeyCode.E, KeyCode.Z};
	private static List<KeyCode> EC = new List<KeyCode>(){KeyCode.E, KeyCode.C};
	private static List<KeyCode> E = new List<KeyCode>(){KeyCode.E};

	private static List<KeyCode> ZC = new List<KeyCode>(){KeyCode.Z, KeyCode.C};
	private static List<KeyCode> Z = new List<KeyCode>(){KeyCode.Z};

	private static List<KeyCode> C = new List<KeyCode>(){KeyCode.C};

	private static List<Movement> thrustUp = new List<Movement>(){new Movement(Movement.vectorType.THRUST, Movement.vectorDirection.UP)};
	private static List<Movement> thrustDown = new List<Movement>(){new Movement(Movement.vectorType.THRUST, Movement.vectorDirection.DOWN)};
	private static List<Movement> thrustLeft = new List<Movement>(){new Movement(Movement.vectorType.THRUST, Movement.vectorDirection.UP)};
	private static List<Movement> thrustRight = new List<Movement>(){new Movement(Movement.vectorType.THRUST, Movement.vectorDirection.DOWN)};
	private static List<Movement> thrustForward = new List<Movement>(){new Movement(Movement.vectorType.THRUST, Movement.vectorDirection.UP)};
	private static List<Movement> thrustBack = new List<Movement>(){new Movement(Movement.vectorType.THRUST, Movement.vectorDirection.DOWN)};

	private static List<Movement> torqueForward = new List<Movement>(){new Movement(Movement.vectorType.TORQUE, Movement.vectorDirection.FORWARD)};
	private static List<Movement> torqueBack = new List<Movement>(){new Movement(Movement.vectorType.TORQUE, Movement.vectorDirection.BACK)};
	private static List<Movement> torqueLeft = new List<Movement>(){new Movement(Movement.vectorType.TORQUE, Movement.vectorDirection.LEFT)};
	private static List<Movement> torqueRight = new List<Movement>(){new Movement(Movement.vectorType.TORQUE, Movement.vectorDirection.RIGHT)};
	private static List<Movement> torqueUp = new List<Movement>(){new Movement(Movement.vectorType.TORQUE, Movement.vectorDirection.LEFT)};
	private static List<Movement> torqueDown = new List<Movement>(){new Movement(Movement.vectorType.TORQUE, Movement.vectorDirection.RIGHT)};

	public static List<KeyCombination> possibleForwardCombinations = new List<KeyCombination>(){
		new KeyCombination(GrabObject.pointForward, QEZC, thrustForward),
		new KeyCombination(GrabObject.pointForward, QEZ, thrustForward),
		new KeyCombination(GrabObject.pointForward, QEC, thrustForward),
		new KeyCombination(GrabObject.pointForward, QZC, thrustForward),
		new KeyCombination(GrabObject.pointForward, QE, torqueRight),
		new KeyCombination(GrabObject.pointForward, QZ, torqueUp),
		new KeyCombination(GrabObject.pointForward, QC, thrustForward),
		new KeyCombination(GrabObject.pointForward, Q, new List<Movement>(){new Movement(Movement.vectorType.TORQUE, Movement.vectorDirection.RIGHT),
																			new Movement(Movement.vectorType.TORQUE, Movement.vectorDirection.UP)}),
		new KeyCombination(GrabObject.pointForward, EZC, thrustForward),
		new KeyCombination(GrabObject.pointForward, EZ, thrustForward),
		new KeyCombination(GrabObject.pointForward, EC, torqueDown),
		new KeyCombination(GrabObject.pointForward, E, new List<Movement>(){new Movement(Movement.vectorType.TORQUE, Movement.vectorDirection.RIGHT),
																			new Movement(Movement.vectorType.TORQUE, Movement.vectorDirection.DOWN)}),
		new KeyCombination(GrabObject.pointForward, ZC, torqueLeft),
		new KeyCombination(GrabObject.pointForward, Z, new List<Movement>(){new Movement(Movement.vectorType.TORQUE, Movement.vectorDirection.LEFT),
																			new Movement(Movement.vectorType.TORQUE, Movement.vectorDirection.UP)}),
		new KeyCombination(GrabObject.pointForward, C, new List<Movement>(){new Movement(Movement.vectorType.TORQUE, Movement.vectorDirection.LEFT),
																			new Movement(Movement.vectorType.TORQUE, Movement.vectorDirection.DOWN)})
	};

	public static List<KeyCombination> possibleBackCombinations = new List<KeyCombination>(){
		new KeyCombination(GrabObject.pointBack, QEZC, thrustBack),
		new KeyCombination(GrabObject.pointBack, QEZ, thrustBack),
		new KeyCombination(GrabObject.pointBack, QEC, thrustBack),
		new KeyCombination(GrabObject.pointBack, QZC, thrustBack),
		new KeyCombination(GrabObject.pointBack, QE, torqueLeft),
		new KeyCombination(GrabObject.pointBack, QZ, torqueDown),
		new KeyCombination(GrabObject.pointBack, QC, thrustBack),
		new KeyCombination(GrabObject.pointBack, Q, new List<Movement>(){new Movement(Movement.vectorType.TORQUE, Movement.vectorDirection.LEFT),
																		 new Movement(Movement.vectorType.TORQUE, Movement.vectorDirection.DOWN)}),
		new KeyCombination(GrabObject.pointBack, EZC, thrustBack),
		new KeyCombination(GrabObject.pointBack, EZ, thrustBack),
		new KeyCombination(GrabObject.pointBack, EC, torqueUp),
		new KeyCombination(GrabObject.pointBack, E, new List<Movement>(){new Movement(Movement.vectorType.TORQUE, Movement.vectorDirection.LEFT),
																		 new Movement(Movement.vectorType.TORQUE, Movement.vectorDirection.UP)}),
		new KeyCombination(GrabObject.pointBack, ZC, torqueRight),
		new KeyCombination(GrabObject.pointBack, Z, new List<Movement>(){new Movement(Movement.vectorType.TORQUE, Movement.vectorDirection.RIGHT),
																		 new Movement(Movement.vectorType.TORQUE, Movement.vectorDirection.DOWN)}),
		new KeyCombination(GrabObject.pointBack, C, new List<Movement>(){new Movement(Movement.vectorType.TORQUE, Movement.vectorDirection.RIGHT),
																		 new Movement(Movement.vectorType.TORQUE, Movement.vectorDirection.UP)})
	};

	public static List<KeyCombination> possibleLeftCombinations = new List<KeyCombination>(){
		new KeyCombination(GrabObject.pointLeft, QEZC, thrustLeft),
		new KeyCombination(GrabObject.pointLeft, QEZ, torqueForward),
		new KeyCombination(GrabObject.pointLeft, QEC, thrustLeft),
		new KeyCombination(GrabObject.pointLeft, QZC, torqueBack),
		new KeyCombination(GrabObject.pointLeft, QE, torqueForward),
		new KeyCombination(GrabObject.pointLeft, QZ, null),
		new KeyCombination(GrabObject.pointLeft, QC, torqueBack),
		new KeyCombination(GrabObject.pointLeft, Q, null),
		new KeyCombination(GrabObject.pointLeft, EZC, thrustLeft),
		new KeyCombination(GrabObject.pointLeft, EZ, torqueForward),
		new KeyCombination(GrabObject.pointLeft, EC, thrustLeft),
		new KeyCombination(GrabObject.pointLeft, E, torqueForward),
		new KeyCombination(GrabObject.pointLeft, ZC, torqueBack),
		new KeyCombination(GrabObject.pointLeft, Z, null),
		new KeyCombination(GrabObject.pointLeft, C, torqueBack)
	};

	public static List<KeyCombination> possibleRightCombinations = new List<KeyCombination>(){
		new KeyCombination(GrabObject.pointRight, QEZC, thrustRight),
		new KeyCombination(GrabObject.pointRight, QEZ, thrustRight),
		new KeyCombination(GrabObject.pointRight, QEC, torqueBack),
		new KeyCombination(GrabObject.pointRight, QZC, thrustRight),
		new KeyCombination(GrabObject.pointRight, QE, torqueBack),
		new KeyCombination(GrabObject.pointRight, QZ, thrustRight),
		new KeyCombination(GrabObject.pointRight, QC, torqueBack),
		new KeyCombination(GrabObject.pointRight, Q, torqueBack),
		new KeyCombination(GrabObject.pointRight, EZC, torqueForward),
		new KeyCombination(GrabObject.pointRight, EZ, torqueForward),
		new KeyCombination(GrabObject.pointRight, EC, null),
		new KeyCombination(GrabObject.pointRight, E, null),
		new KeyCombination(GrabObject.pointRight, ZC, torqueForward),
		new KeyCombination(GrabObject.pointRight, Z, torqueForward),
		new KeyCombination(GrabObject.pointRight, C, null)
	};

	public static List<KeyCombination> possibleUpCombinations = new List<KeyCombination>(){
		new KeyCombination(GrabObject.pointUp, QEZC, thrustUp),
		new KeyCombination(GrabObject.pointUp, QEZ, torqueBack),
		new KeyCombination(GrabObject.pointUp, QEC, torqueForward),
		new KeyCombination(GrabObject.pointUp, QZC, thrustUp),
		new KeyCombination(GrabObject.pointUp, QE, null),
		new KeyCombination(GrabObject.pointUp, QZ, torqueBack),
		new KeyCombination(GrabObject.pointUp, QC, torqueForward),
		new KeyCombination(GrabObject.pointUp, Q, null),
		new KeyCombination(GrabObject.pointUp, EZC, thrustUp),
		new KeyCombination(GrabObject.pointUp, EZ, torqueBack),
		new KeyCombination(GrabObject.pointUp, EC, torqueForward),
		new KeyCombination(GrabObject.pointUp, E, null),
		new KeyCombination(GrabObject.pointUp, ZC, thrustUp),
		new KeyCombination(GrabObject.pointUp, Z, torqueBack),
		new KeyCombination(GrabObject.pointUp, C, torqueForward)
	};

	public static List<KeyCombination> possibleDownCombinations = new List<KeyCombination>(){
		new KeyCombination(GrabObject.pointDown, QEZC, thrustDown),
		new KeyCombination(GrabObject.pointDown, QEZ, thrustDown),
		new KeyCombination(GrabObject.pointDown, QEC, thrustDown),
		new KeyCombination(GrabObject.pointDown, QZC,torqueForward),
		new KeyCombination(GrabObject.pointDown, QE, thrustDown),
		new KeyCombination(GrabObject.pointDown, QZ, torqueForward),
		new KeyCombination(GrabObject.pointDown, QC, torqueForward),
		new KeyCombination(GrabObject.pointDown, Q, torqueForward),
		new KeyCombination(GrabObject.pointDown, EZC, torqueBack),
		new KeyCombination(GrabObject.pointDown, EZ, torqueBack),
		new KeyCombination(GrabObject.pointDown, EC, torqueBack),
		new KeyCombination(GrabObject.pointDown, E, torqueBack),
		new KeyCombination(GrabObject.pointDown, ZC, null),
		new KeyCombination(GrabObject.pointDown, Z, null),
		new KeyCombination(GrabObject.pointDown, C, null)
	};

	public static Dictionary<KeyCode, List<KeyCombination>> possibleCombinations = new Dictionary<KeyCode, List<KeyCombination>>(){
		{KeyCode.W, possibleForwardCombinations},
		{KeyCode.S, possibleBackCombinations},
		{KeyCode.A, possibleLeftCombinations},
		{KeyCode.D, possibleRightCombinations},
		{KeyCode.Space, possibleUpCombinations},
		{KeyCode.LeftShift, possibleDownCombinations}
	};
	#endregion

	public KeyCombination (KeyCode key, List<KeyCode> keysDown, List<Movement> movements) {
		this.key = key;
		this.keysDown = keysDown;
		this.movements = movements;
	}

	public bool compare (KeyCombination otherCombination){
		return key == otherCombination.key && otherCombination.keysDown.Except (keysDown).Count() == 0;
	}
}

public class Movement {
	public enum vectorType {THRUST, TORQUE, NONE};
	public enum vectorDirection {FORWARD, BACK, LEFT, RIGHT, UP, DOWN};

	public vectorType type;
	public vectorDirection direction;

	public Movement (vectorType type, vectorDirection direction) {
		this.type = type;
		this.direction = direction;
	}

	public void applyForceToPlayer (GameObject gameObject, List<jumpableObject> jObjectsNearPlayer) {
		//TODO: there should be a limit to the amount of force added
		Transform transform = gameObject.transform;
		Rigidbody body = gameObject.GetComponent<Rigidbody> ();

		Vector3 vector = translateVectorDirectionToTransformVector (transform);
		//if (hasValidJObject (vector, jObjectsNearPlayer)) {
			body.isKinematic = false;
			if (type == vectorType.THRUST) {
				body.AddForce (vector * 10f);
			} else if (type == vectorType.TORQUE) {
				body.AddTorque (vector * 10f * Time.deltaTime, ForceMode.VelocityChange);
			}
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

	private Vector3 translateVectorDirectionToTransformVector (Transform transform) {
		switch (direction) {
			case vectorDirection.FORWARD:
				return transform.forward;
			case vectorDirection.BACK:
				return -transform.forward;
			case vectorDirection.LEFT:
				return transform.right;
			case vectorDirection.RIGHT:
				return -transform.right;
			case vectorDirection.UP:
				return transform.up;
			case vectorDirection.DOWN:
				return -transform.up;
			default:
			return Vector3.zero;
		}
	}
}

public class GrabObject : MonoBehaviour {

	enum MouseButtons {LEFT = 0, RIGHT = 1, MIDDLE = 2};

	GameObject mainCamera;
	GameObject grabbedObject;
	public float maxGrabDistance;
	public float smooth;
	public float jumpForce = 250f;
	public float jumpableRadius = 2.0f;
	public float vectorAngleTolerance = 10.0f;
	public float torque;
	public float force;

	private enum vectorType {THRUST, TORQUE};
	private enum vectorDirection {UP, DOWN, FORWARD, BACK, LEFT, RIGHT};

	public static KeyCode rightFoot = KeyCode.C;
	public static KeyCode leftFoot = KeyCode.Z;
	public static KeyCode leftHand = KeyCode.Q;
	public static KeyCode rightHand = KeyCode.E;

	public static KeyCode pointForward = KeyCode.W;
	public static KeyCode pointBack = KeyCode.S;
	public static KeyCode pointLeft = KeyCode.A;
	public static KeyCode pointRight = KeyCode.D;
	public static KeyCode pointDown = KeyCode.LeftShift;
	public static KeyCode pointUp = KeyCode.Space;

	private static KeyCode[] pointingKeysArray = new KeyCode[]{pointForward, pointLeft, pointBack, pointRight, pointDown, pointUp};

	public static Dictionary<KeyCode, List<KeyCode>> possibleCombinations = new Dictionary<KeyCode, List<KeyCode>>() {
		{pointForward, new List<KeyCode>(){leftFoot, rightFoot, leftHand, rightHand}},
		{pointBack, new List<KeyCode>(){leftFoot, rightFoot, leftHand, rightHand}},
		{pointLeft, new List<KeyCode>(){leftFoot, rightFoot, leftHand, rightHand}},
		{pointRight, new List<KeyCode>(){leftFoot, rightFoot, leftHand, rightHand}},
		{pointDown, new List<KeyCode>(){leftHand, rightHand}},
		{pointUp, new List<KeyCode>(){leftFoot, rightFoot}}
	};
		
	void Start () {
		mainCamera = GameObject.FindWithTag ("FPSCamera");
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

	private Vector3 getDirectionVectorForKey (KeyCode key) {
		Transform playerTransform = getPlayerTransform ();

		if (key == pointForward) {
			return playerTransform.forward;
		} else if (key == pointBack) {
			return -playerTransform.forward;
		} else if (key == pointLeft) {
			return -playerTransform.right;
		} else if (key == pointRight) {
			return playerTransform.right;
		} else if (key == pointDown) {
			return -playerTransform.up;
		} else if (key == pointUp) {
			return playerTransform.up;
		} else {
			throw new System.ArgumentException("Parameter cannot be anything other than W,A,S,D,Left Shift, or Space", "key");
		}
	}
	#endregion

//	private vectorType getVectorTypeForKeyCombination (KeyCode key, List<KeyCode> keysDown) {
//
//	}

	private void movePlayer(Dictionary<KeyCode, List<KeyCode>> combinations) {
		foreach (KeyValuePair<KeyCode, List<KeyCode>> combination in combinations) {
			KeyCode key = combination.Key;
			List<KeyCode> keysDown = combination.Value;


		}
	}

//	private Dictionary<KeyCode, List<KeyCode>> getAllKeyCombinationsForCurrentFrame (KeyCode[] keys, List<KeyCode> keysDown) {
//		Dictionary<KeyCode, List<KeyCode>> result = new Dictionary<KeyCode, List<KeyCode>> ();
//
//		foreach (KeyCode key in keys) {
//			List <KeyCode> keysDownForKey = new List <KeyCode> ();
//			List<KeyCode> possibleCombination = possibleCombinations[key];
//			foreach (KeyCode keyDown in keysDown) {
//				if (possibleCombination.Contains(keyDown)) {
//					keysDownForKey.Add (keyDown);
//				}
//			}
//
//			result.Add (key, keysDownForKey);
//		}
//
//		return result;
//	}

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
			if (!Input.GetKey (KeyCode.W)
			    && !Input.GetKey (KeyCode.A)
			    && !Input.GetKey (KeyCode.S)
			    && !Input.GetKey (KeyCode.D)
			    && !Input.GetKey (KeyCode.LeftShift)
			    && !Input.GetKey (KeyCode.Space)) {
				return;
			}

			List<KeyCode> keysDown = new List<KeyCode> ();

			//get which 'push' keys have just been pressed
			foreach (KeyCode key in new KeyCode[]{KeyCode.Q, KeyCode.E, KeyCode.Z, KeyCode.C}) {
				if (Input.GetKey (key)) {
					keysDown.Add (key);
				}
			}

			if (keysDown.Count == 0) {
				return;
			}

			List<jumpableObject> jObjectsNearPlayer = getAllJumpableObjectsNearPlayer ();

			foreach (KeyCode key in new KeyCode[]{KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D, KeyCode.LeftShift, KeyCode.Space}) {
				if (Input.GetKey (key)) {
					List<KeyCombination> combinations = KeyCombination.possibleCombinations [key];
					foreach (KeyCombination combination in combinations) {
						if (combination.keysDown.Except (keysDown).Count () == 0) {
							//this combination is the one we go with
							foreach (Movement movement in combination.movements) {
								//TODO: check if the right in jumpable object exists
								movement.applyForceToPlayer (GameObject.FindWithTag ("Player"), jObjectsNearPlayer);
							}
							break;
						}
					}
				}
			}
		} else { //if the player is grabbing something
			tryToJump ();
		}
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
	}
}
