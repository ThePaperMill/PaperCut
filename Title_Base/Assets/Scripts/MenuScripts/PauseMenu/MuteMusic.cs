/****************************************************************************/
/*!
\file  MuteMusic.cs
\author Steven Gallwas 
\brief  
 
  
    © 2016 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;

public class MuteMusic : MenuButton
{
    GameObject SoundPlayer = null;
    TheMusicNeverEnds MusicPlayer = null;

	// Use this for initialization
	void Start ()
    {
        SoundPlayer = GameObject.FindGameObjectWithTag("PersistentMusic");

        if(SoundPlayer)
        {
            MusicPlayer = SoundPlayer.GetComponent<TheMusicNeverEnds>();
        }
    }

    public override void Activate()
    {
        if(SoundPlayer == null || MusicPlayer == null)
        {
            SoundPlayer = GameObject.FindGameObjectWithTag("PersistentMusic");

            if (SoundPlayer)
            {
                MusicPlayer = SoundPlayer.GetComponent<TheMusicNeverEnds>();
            }
        }

        if (MusicPlayer && MusicPlayer.forcedStop == false)
        {
            MusicPlayer.StopMusic();
        }
        else if (MusicPlayer && MusicPlayer.forcedStop)
        {
            MusicPlayer.StartMusic();
        }
    }

    // Update is called once per frame
    void Update ()
    {
	
	}
}
