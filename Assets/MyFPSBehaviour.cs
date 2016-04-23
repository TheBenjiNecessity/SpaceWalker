﻿using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class MyFPSBehaviour : MonoBehaviour {

	private Camera m_Camera;
	private float m_YRotation;
	private float m_XRotation;
	private Vector2 m_Input;
	private CollisionFlags m_CollisionFlags;

	private MyMouseLook m_MouseLook;

	// Use this for initialization
	void Start () {
		m_Camera = GameObject.FindWithTag ("FPSCamera").GetComponent<Camera> ();
		m_MouseLook = new MyMouseLook ();
		m_MouseLook.Init(transform, m_Camera.transform);
	}
	
	// Update is called once per frame
	void Update () {
		m_MouseLook.HandleRotation (transform, m_Camera.transform);
	}
}