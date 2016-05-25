/****************************************************************************/
/*!
\file  DuckBob.cs
\author Troy 
\brief  
     An edit of ItemSpin that isolates the Bounce effect.

© 2016 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;
using ActionSystem;

public class DuckBob : MonoBehaviour
{
	public float BounceAmount = 0.015f;
	public float BounceSpeed = 0.5f;
	public float Delay = 0.0f;
	public float InitDelay = 0.0f;

	bool initDelayed = false;

	[HideInInspector]
	public Vector3 StartingPostion = new Vector3();

	public float HeightOffset { get; set; }

	void Start()
	{
		StartingPostion = transform.position;

		HeightOffset = 0.0f;

		if (InitDelay == 0.0f)
		{
			ActionSequence temp = Action.Sequence (this.GetActions ());
			Action.Property (temp, this.GetProperty (o => o.HeightOffset), BounceAmount, BounceSpeed, Ease.Linear);
			Action.Delay (temp, Delay);
			Action.Call (temp, ActionBounce);
		}
		else
		{
			initDelayed = true;
		}
	}

	void ActionBounce()
	{
		BounceAmount *= -1;
		ActionSequence temp = Action.Sequence(this.GetActions());
		Action.Property(temp, this.GetProperty(o => o.HeightOffset), 2.0f * BounceAmount, BounceSpeed, Ease.Linear);
		Action.Delay(temp, Delay);
		Action.Call(temp, ActionBounce);
	}

	void Update()
	{
		transform.position = new Vector3(transform.position.x, StartingPostion.y + HeightOffset, transform.position.z);

		if (initDelayed)
		{
			InitDelay -= Time.smoothDeltaTime;

			if (InitDelay <= 0.0f)
			{
				ActionSequence temp = Action.Sequence (this.GetActions ());
				Action.Property (temp, this.GetProperty (o => o.HeightOffset), BounceAmount, BounceSpeed, Ease.Linear);
				Action.Delay (temp, Delay);
				Action.Call (temp, ActionBounce);

				initDelayed = false;
			}
		}
	}
}