using UnityEngine;
using System.Collections;

public class PlayerBehaviour : MonoBehaviour {
	public Texture2D crosshairTexture;
	Rect position;
	static bool OriginalOn = true;

	void Start()
	{
		position = new Rect((Screen.width - 2) / 2, (Screen.height - 
			2) /2, 2, 2);

		lockCursor ();
	}

	void Update()
	{
		lockCursor ();
	}

	void OnGUI()
	{
		if(OriginalOn == true)
		{
			GUI.DrawTexture(position, crosshairTexture);
		}
	}

	void lockCursor () {
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}
}


//Notes:
//the mouse controls yaw and pitch of the camera.
//the 'A' and 'D' keys control the roll
//the 'W' and 'S' keys do ...