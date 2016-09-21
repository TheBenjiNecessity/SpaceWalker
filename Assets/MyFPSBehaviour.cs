using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class MyFPSBehaviour : MonoBehaviour {

	private States state;
	private Camera m_Camera;
	private GameObject m_Head;
	private float m_YRotation;
	private float m_XRotation;
	private Vector2 m_Input;
	private CollisionFlags m_CollisionFlags;
	private GameObject m_player;

	private MyMouseLook m_MouseLook;

	// Use this for initialization
	void Start () {
		state = States.Instance;
		m_Camera = GameObject.FindWithTag ("FPSCamera").GetComponent<Camera> ();
		m_Head = GameObject.FindWithTag ("Head");
		m_player = GameObject.FindWithTag ("Player");
		m_MouseLook = new MyMouseLook ();
		m_MouseLook.Init(m_Head.transform, m_Camera.transform);
	}
	
	// Update is called once per frame
	void Update () {
		m_MouseLook.allowBodyRotationWithHead = state.grabbingObject;

		if (!state.rotatingObject && !state.draggingDoor) {
			m_MouseLook.HandleRotation (m_Head.transform, m_Camera.transform, m_player.transform);
		}
	}
}