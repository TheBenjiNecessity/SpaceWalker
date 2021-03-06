﻿using UnityEngine;
using System.Collections;
using UnityStandardAssets.CrossPlatformInput;

public class States
{
	public bool draggingDoor;
	public bool rotatingObject;
	public bool grabbingObject;

	private static States instance;

	private States() {
		draggingDoor = false;
		rotatingObject = false;
		grabbingObject = false;
		Debug.Log ("States init");
	}

	public static States Instance
	{
		get 
		{
			if (instance == null)
			{
				instance = new States();
			}
			return instance;
		}
	}

	public static float getMouseXAxis () {
		return CrossPlatformInputManager.GetAxis ("Mouse X");
	}

	public static float getMouseYAxis () {
		return CrossPlatformInputManager.GetAxis ("Mouse Y");
	}
}

