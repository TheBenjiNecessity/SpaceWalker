using UnityEngine;
using System.Collections;

public abstract class GravityVolume : MonoBehaviour {

	public Vector3 gravity;//

	// Use this for initialization
	protected virtual void Start() {
		MeshRenderer renderer = this.gameObject.GetComponent<MeshRenderer> ();
		renderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {}

	void OnTriggerStay (Collider other) {
		Rigidbody rigidbody = other.gameObject.GetComponent<Rigidbody> ();
		gravityForRigidbody (rigidbody);
	}

	public abstract void gravityForRigidbody (Rigidbody rigidbody);
}
