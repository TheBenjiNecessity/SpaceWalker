using UnityEngine;
using System.Collections;

public class GravityAttractor : GravityVolume
{

	// Use this for initialization
	protected override void Start() {
		base.Start ();
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public override void gravityForRigidbody (Rigidbody rigidbody) {
		rigidbody.AddForce(gravity * rigidbody.mass);
	}
}

