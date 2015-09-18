/*********************************
 * LogoKill.cs
 * Troy
 * Created 9/4/2015
 * Copyright © 2015 DigiPen Institute of Technology, All Rights Reserved
 *********************************/

using UnityEngine;
using System.Collections;
using System;

public class LogoKill : MonoBehaviour
{
	public RectTransform size;

	private void Start()
	{
		// Replace with direct assignment later?
		size = size.GetComponent<RectTransform> ();

		size.sizeDelta = new Vector2(1600, 1050);
	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
		{
			DestroyObject(gameObject);
		}
	}
}