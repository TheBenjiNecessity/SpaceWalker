using UnityEngine;
using System.Collections;

public class GravityRepeller : GravityVolume
{

	// Use this for initialization
	void Start ()
	{
	
	}
	
	// Update is called once per frame
	void Update ()
	{
	
	}

	public override void gravityForRigidbody (Rigidbody rigidbody)
	{
		throw new System.NotImplementedException ();
	}
}

