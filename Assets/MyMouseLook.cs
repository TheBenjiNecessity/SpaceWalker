using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class MyMouseLook : MonoBehaviour
{
	public float XSensitivity = 2f;
	public float YSensitivity = 2f;
	public bool smooth;
	public float smoothTime = 0.5f;
	public float rollSpeed = 3f;
	public bool clampVerticalRotation = true;
	public float MinimumX = -90F;
	public float MaximumX = 90F;
	public float MinimumY = -60F;
	public float MaximumY = 60F;

	private Quaternion m_CharacterTargetRot;
	private Quaternion m_CameraTargetRot;

	public void Init(Transform character, Transform camera)
	{
		m_CharacterTargetRot = character.localRotation;
		m_CameraTargetRot = camera.localRotation;
	}

	public void HandleRotation(Transform character, Transform camera)
	{
		float xRot = CrossPlatformInputManager.GetAxis("Mouse X") * XSensitivity;
		float yRot = CrossPlatformInputManager.GetAxis("Mouse Y") * YSensitivity;

		m_CharacterTargetRot *= Quaternion.Euler (0f, xRot, 0f);
		m_CameraTargetRot *= Quaternion.Euler (-yRot, 0f, 0f);

		if(clampVerticalRotation)
			m_CameraTargetRot = ClampRotationAroundXAxis (m_CameraTargetRot);

		m_CharacterTargetRot = ClampRotationAroundYAxis (m_CharacterTargetRot);

		if(smooth)
		{
			character.localRotation = Quaternion.Slerp (character.localRotation, m_CharacterTargetRot,
				smoothTime * Time.deltaTime);
			camera.localRotation = Quaternion.Slerp (camera.localRotation, m_CameraTargetRot,
				smoothTime * Time.deltaTime);
		}
		else
		{
			character.localRotation = m_CharacterTargetRot;
			camera.localRotation = m_CameraTargetRot;
		}

		//RotateBody (character);
	}

	Quaternion ClampRotationAroundXAxis(Quaternion q)
	{
		q.x /= q.w;
		q.y /= q.w;
		q.z /= q.w;
		q.w = 1.0f;

		float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.x);

		angleX = Mathf.Clamp (angleX, MinimumX, MaximumX);

		q.x = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleX);

		return q;
	}

	Quaternion ClampRotationAroundYAxis(Quaternion q)
	{
		q.x /= q.w;
		q.y /= q.w;
		q.z /= q.w;
		q.w = 1.0f;

		float angleY = 2.0f * Mathf.Rad2Deg * Mathf.Atan (q.y);

		angleY = Mathf.Clamp (angleY, MinimumY, MaximumY);

		q.y = Mathf.Tan (0.5f * Mathf.Deg2Rad * angleY);

		return q;
	}

//	private void RotateBody(Transform character)
//	{
//		if (Input.GetKey (KeyCode.A)) {
//			RollLeft (character);
//		} else if (Input.GetKey (KeyCode.D)) {
//			RollRight (character);
//		}
//
//		if (Input.GetKey (KeyCode.W)) {
//			PitchForward (character);
//		} else if (Input.GetKey (KeyCode.S)) {
//			PitchBack (character);
//		}
//	}

//	public void RollLeft(Transform character) {
//		Roll (rollSpeed, character);
//	}
//
//	public void RollRight(Transform character) {
//		Roll (-rollSpeed, character);
//	}
//
//	public void PitchForward(Transform character) {
//		Pitch (rollSpeed, character);
//	}
//
//	public void PitchBack(Transform character) {
//		Pitch (-rollSpeed, character);
//	}

//	private void Roll(float roll, Transform character) {
//		m_CharacterTargetRot *= Quaternion.Euler (0f, 0f, roll);
//		if(smooth)
//		{
//			character.localRotation = Quaternion.Slerp (character.localRotation, m_CharacterTargetRot,
//				smoothTime * Time.deltaTime);
//		}
//		else
//		{
//			character.localRotation = m_CharacterTargetRot;
//		}
//	}
//
//	private void Pitch(float pitch, Transform character) {
//		m_CharacterTargetRot *= Quaternion.Euler (pitch, 0f, 0f);
//		if(smooth)
//		{
//			character.localRotation = Quaternion.Slerp (character.localRotation, m_CharacterTargetRot,
//				smoothTime * Time.deltaTime);
//		}
//		else
//		{
//			character.localRotation = m_CharacterTargetRot;
//		}
//	}
}

