/*********************************
 * HighlightController.cs
 * Troy
 * Created 9/11/2015
 * Copyright © 2015 DigiPen Institute of Technology, All Rights Reserved
 *********************************/

using UnityEngine;
using System.Collections;

public class HighlightController : MonoBehaviour
{
	public float Height = 1.5f;
	public float EaseSpeed = 1f;
	/*public float EaseType : Ease = Ease.Linear;*/ /* For Actions once those are ready */
	public float RotateSpeedDegrees = 40.0f;
	
	Vector3 Position = new Vector3(0,0,0);
	
	void Start()
	{
		//It appears below the position it's supposed to.
		Position = gameObject.transform.position;
		gameObject.transform.position += new Vector3(0, Height, 0);
		
		print("Work on making the Highlight drop out of the sky");
		
		//Call a void that updates the sequence naturally
		this.UpdateHeight();
	}
	void Update()
	{
		gameObject.transform.position = Position + new Vector3(0, Height, 0);
		
		gameObject.transform.Rotate(new Vector3(0, RotateSpeedDegrees * Time.deltaTime, 0));
		//this.Owner.Transform.Rotation = Math.ToQuaternion(this.Owner.Transform.EulerAngles);
	}
	void UpdateHeight()
	{
		// Use Josh's Action replacement system here   
		
		float altHeight = -this.Height;

		/*var seq = Action.Sequence(this.Owner.Actions);
		
		Action.Property(seq, @this.Height, height2, this.EaseSpeed, this.EaseType);
		Action.Call(seq, this.UpdateHeight);*/
	}
}