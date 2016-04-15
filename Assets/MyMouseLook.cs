using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class MyMouseLook : MonoBehaviour
{
	public float XSensitivity = 2f;
	public float YSensitivity = 2f;
	public bool smooth;
	public float smoothTime = 5f;
	public float rollSpeed = 0.75f;

	private Quaternion m_CharacterTargetRot;
	private Quaternion m_CameraTargetRot;

	public void Init(Transform character, Transform camera)
	{
		m_CharacterTargetRot = character.localRotation;
		m_CameraTargetRot = camera.localRotation;
	}

	public void HandleRotation(Transform character, Transform camera)
	{
		float yRot = CrossPlatformInputManager.GetAxis("Mouse X") * XSensitivity;
		float xRot = CrossPlatformInputManager.GetAxis("Mouse Y") * YSensitivity;

		m_CharacterTargetRot *= Quaternion.Euler (0f, yRot, 0f);
		m_CameraTargetRot *= Quaternion.Euler (-xRot, 0f, 0f);

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

		RotateBody (character);
	}

	private void RotateBody(Transform character)
	{
		if (Input.GetKey (KeyCode.A)) {
			RollLeft (character);
		} else if (Input.GetKey (KeyCode.D)) {
			RollRight (character);
		}

		if (Input.GetKey (KeyCode.W)) {
			PitchForward (character);
		} else if (Input.GetKey (KeyCode.S)) {
			PitchBack (character);
		}
	}

	public void RollLeft(Transform character) {
		Roll (rollSpeed, character);
	}

	public void RollRight(Transform character) {
		Roll (-rollSpeed, character);
	}

	public void PitchForward(Transform character) {
		Pitch (rollSpeed, character);
	}

	public void PitchBack(Transform character) {
		Pitch (-rollSpeed, character);
	}

	private void Roll(float roll, Transform character) {
		m_CharacterTargetRot *= Quaternion.Euler (0f, 0f, roll);
		if(smooth)
		{
			character.localRotation = Quaternion.Slerp (character.localRotation, m_CharacterTargetRot,
				smoothTime * Time.deltaTime);
		}
		else
		{
			character.localRotation = m_CharacterTargetRot;
		}
	}

	private void Pitch(float pitch, Transform character) {
		m_CharacterTargetRot *= Quaternion.Euler (pitch, 0f, 0f);
		if(smooth)
		{
			character.localRotation = Quaternion.Slerp (character.localRotation, m_CharacterTargetRot,
				smoothTime * Time.deltaTime);
		}
		else
		{
			character.localRotation = m_CharacterTargetRot;
		}
	}
}

