/*********************************
 * HighlightController.cs
 * Troy
 * Created 9/11/2015
 * Copyright © 2015 DigiPen Institute of Technology, All Rights Reserved
 *********************************/

using UnityEngine;
using System.Collections;
using ActionSystem;

public class HighlightController : MonoBehaviour
{
	//public float Height = 1.0f;
	public float EasePercent = 1f;
	public float RotateSpeedDegrees = 40.0f;
	public Vector3 GoToPos = new Vector3(0,0,0);
	
	Vector3 goingTo = new Vector3(1,1,1);
	Vector3 lerpSpeed = new Vector3(0,0,0);
	float speed = 0.0f;
	float lastDist = float.MaxValue;
	bool updating = false;
	bool nearApocynthion = false;

	void Start()
	{
		// Sanity check:  EasePercent can't be above 100!
		if(EasePercent > 100.0f)
		{
			EasePercent = 100.0f;
		}

		//print("Work on making the Highlight drop out of the sky");
	}

	void Update()
	{
		// Always be rotating
		gameObject.transform.Rotate(new Vector3(0, RotateSpeedDegrees * Time.smoothDeltaTime, 0));

		// Move to a new locale if necessary
		if(updating)
		{
			UpdateHeight();
		}
	}

	void UpdateHeight()
	{
		// Determine our destination if it has changed
		if(goingTo != GoToPos)
		{        
			goingTo = GoToPos;
			speed = EasePercent * Time.smoothDeltaTime;

			// Sanity check:  Don't go over 100% of the distance at any time!
			if(speed > 1.0f)
			{
				speed = 1.0f;
			}

			lerpSpeed = Vector3.Lerp(gameObject.transform.position, GoToPos, speed) - gameObject.transform.position;
		}
		// Always move to the requested location
		gameObject.transform.position += lerpSpeed;
		lastDist = Vector3.Distance(gameObject.transform.position, GoToPos) - speed;

		// If we get close, prepare to stop (necessary for if the game is running at a lower FPS)
		if(lastDist < 0.2f)
		{
			nearApocynthion = true;
		}

		// Stupidity Counter:  At a low FPS the Highlight can overshoot its mark, so reverse & lessen its speed
		if(lastDist > 0.2f && nearApocynthion)
		{
			lerpSpeed = Vector3.Lerp(gameObject.transform.position, GoToPos, speed * 3) - gameObject.transform.position;
			nearApocynthion = false;
		}

		// Lock into place if we're close enough (we are NOT using Epsilon because it's too small of a distance
		if(Vector3.Distance(gameObject.transform.position, GoToPos) < speed * 1.1)
		{
			gameObject.transform.position = GoToPos;
			updating = false;
			lastDist = float.MaxValue;
			nearApocynthion = false;
		}
	}

	public void setUp(bool IsUp)
	{
		updating = IsUp;
	}
	
	public bool getUp()
	{
		return updating;
	}
}