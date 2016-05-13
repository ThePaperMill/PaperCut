﻿/****************************************************************************/
/*!
\file   HoverSpin.cs
\author Jerry Nacier
\brief  
    The best feature of the game.  4 out of 4 instructors approve.
  
    © 2016 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using ActionSystem;
using System.Collections;

public class HoverSpin : MonoBehaviour
{
    private bool JumpReleased;
    private bool Grounded;
    private bool JumpHeld;
    private bool Spinning = false;

    public Vector3 Prone = new Vector3();
    public float HoverDelay = 0.0f;

	private FMOD_StudioEventEmitter HoverSounds;
	private FMOD_StudioEventEmitter DelaySounds;
    float JumpTimer;
    ActionGroup grp = new ActionGroup();
    //Rigidbody RB = new Rigidbody();

    // Use this for initialization
    void Start ()
    {
        JumpTimer = 0.0f;
        Grounded = GetComponent<CustomDynamicController>().OnGround;

		HoverSounds = gameObject.transform.Find("HoverSoundBank").gameObject.GetComponent<FMOD_StudioEventEmitter>();
		DelaySounds = gameObject.transform.Find("HoverSounds2").gameObject.GetComponent<FMOD_StudioEventEmitter>();
    }
	
	// Update is called once per frame
	void Update ()
    {
        Grounded = GetComponent<CustomDynamicController>().OnGround;
        JumpReleased = InputManager.GetSingleton.IsInputReleased(GlobalControls.JumpKeys);

        Spin();
            
        if (Grounded == true || JumpReleased) // cancel spin when we land or when jump is released with addition of "|| JumpReleased"
        {
            JumpTimer = 0.0f;
        }

        // Stop any sounds when not spinning
		if(!Spinning && HoverSounds != null)
        {
			HoverSounds.Stop();
			DelaySounds.Stop();
        }
    }

    //This should be renamed to Prone or something?
    void Spin()
    {
        bool JumpHeld = InputManager.GetSingleton.IsInputDown(GlobalControls.JumpKeys);

        if (Grounded == false && JumpHeld == true) 
        {
            JumpTimer += Time.deltaTime;
        }


        if(JumpTimer >= HoverDelay)
        {
            Prone = new Vector3(90.0f, 0, 0);
            var seq = ActionSystem.Action.Sequence(grp);
			Action.Property(seq, transform.GetProperty(x => x.localEulerAngles), Prone, 0.05f, Ease.Linear);

			// Start playing whooshing sounds if we aren't already
			if (!Spinning && HoverSounds != null)
			{
				HoverSounds.Play();
				Invoke("PlayDelayedSound", 0.5f);
			}

			if (Spinning && HoverSounds != null &&
				(HoverSounds.getPlaybackState() == FMOD.Studio.PLAYBACK_STATE.STOPPING
				|| HoverSounds.getPlaybackState() == FMOD.Studio.PLAYBACK_STATE.STOPPED))
			{
				HoverSounds.Stop();
				HoverSounds.Play();
			}

			if (Spinning && HoverSounds != null &&
				(DelaySounds.getPlaybackState() == FMOD.Studio.PLAYBACK_STATE.STOPPING
				|| DelaySounds.getPlaybackState() == FMOD.Studio.PLAYBACK_STATE.STOPPED))
			{
				DelaySounds.Stop();
				DelaySounds.Play();
			}

            Spinning = true;
        }

        grp.Update(Time.deltaTime);

        // Check to see if we need to stop
        if (JumpTimer == 0)
        {
            Prone = new Vector3(0, 0, 0);
			gameObject.transform.localEulerAngles = Prone;

			Spinning = false;
        }
    }

    public bool IsSpinning()
    {
        return Spinning;
    }

	void PlayDelayedSound()
	{
		DelaySounds.Play();
	}
}
