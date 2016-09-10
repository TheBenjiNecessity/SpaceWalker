using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class MyMouseLook : MonoBehaviour
{
	private enum TurnType {LEFT, RIGHT, NONE};

	public float XSensitivity = 2f;
	public float YSensitivity = 2f;
	public bool smooth = false;
	public float smoothTime = 10f;
	public float rollSpeed = 3f;
	public bool clampVerticalRotation = true;
	public bool clampHorizontalRotation = true;
	public float MinimumX = -90F;
	public float MaximumX = 90F;
	public float MinimumY = -60F;
	public float MaximumY = 60F;

	public bool allowBodyRotationWithHead = false;
	private bool shouldRotateBodyWithHead = false;

	private Quaternion m_CharacterTargetRot;
	private Quaternion m_CameraTargetRot;

	public void Init(Transform character, Transform camera)
	{
		m_CharacterTargetRot = character.localRotation;
		m_CameraTargetRot = camera.localRotation;
	}

	public void HandleRotation(Transform head, Transform camera, Transform body)
	{
		float xRot = CrossPlatformInputManager.GetAxis("Mouse X") * XSensitivity;
		float yRot = CrossPlatformInputManager.GetAxis("Mouse Y") * YSensitivity;

		m_CharacterTargetRot *= Quaternion.Euler (0f, xRot, 0f);
		m_CameraTargetRot *= Quaternion.Euler (-yRot, 0f, 0f);

		if(clampVerticalRotation)
			m_CameraTargetRot = ClampRotationAroundXAxis (m_CameraTargetRot);

		if(clampHorizontalRotation)
			m_CharacterTargetRot = ClampRotationAroundYAxis (m_CharacterTargetRot);

		if(smooth)
		{
			head.localRotation = Quaternion.Slerp (head.localRotation, m_CharacterTargetRot,
				smoothTime * Time.deltaTime);
			camera.localRotation = Quaternion.Slerp (camera.localRotation, m_CameraTargetRot,
				smoothTime * Time.deltaTime);

			if (allowBodyRotationWithHead && shouldRotateBodyWithHead) {
				body.localRotation = Quaternion.Slerp (body.localRotation, Quaternion.Euler (0f, xRot, 0f), smoothTime * Time.deltaTime);
			}
		}
		else
		{
			head.localRotation = m_CharacterTargetRot;
			camera.localRotation = m_CameraTargetRot;

			if (allowBodyRotationWithHead && shouldRotateBodyWithHead) {
				body.localRotation *= Quaternion.Euler (0f, xRot, 0f);
			}
		}
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

		shouldRotateBodyWithHead = angleY == MinimumY || angleY == MaximumY;
								
		return q;
	}
}