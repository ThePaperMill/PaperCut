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
	public float Height = 1.0f;
	public float HeightGet { get; set;}
	public float EaseSpeed = 1f;
	public Ease EaseType = Ease.Linear;
	public float RotateSpeedDegrees = 40.0f;
	public ActionGroup Grp = new ActionGroup();
	
	Vector3 Position = new Vector3(0,0,0);
	
	void Start()
	{
		HeightGet = Height;
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
		//old code --> this.Owner.Transform.Rotation = Math.ToQuaternion(this.Owner.Transform.EulerAngles);
	}
	void UpdateHeight()
	{
		// Use Josh's Action replacement system here   
		
		float altHeight = -HeightGet;

		ActionSequence seq = ActionSystem.Action.Sequence(Grp);
		
		Action.Property(seq, this.GetProperty(o => o.HeightGet), altHeight, this.EaseSpeed, this.EaseType);
		Action.Call(seq, this.UpdateHeight);
	}
}