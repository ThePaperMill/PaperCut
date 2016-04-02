/****************************************************************************/
/*!
\file   TheMusicNeverEnds.cs
\author Troy
\brief  
    Controls the persistent music object.
  
    © 2016 DigiPen, All Rights Reserved.
*/
/****************************************************************************/

using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class TheMusicNeverEnds : MonoBehaviour
{
    FMODAsset tunez = null;
    FMOD_StudioEventEmitter bbox;

    [HideInInspector]
    public bool forcedStop = false;

    [HideInInspector]
    public bool AllStop = false;

    GameObject listener = null;
    GameObject musicBox = null;
    GameObject Camera = null;
    GameObject Player = null;
    GameObject goTo = null;
    HORRIBLESCRIPT tuneGet = null;

    bool lastPlayerCheckDone = false;

    // Use this for initial initialization (thanks Unity)
    void Awake()
    {
        DontDestroyOnLoad(this);
        listener = gameObject.transform.Find("FMOD_Listener").gameObject;
        musicBox = gameObject.transform.Find("MusicSource").gameObject;
    }
           
    // Use this for initialization (but only once, because code effeciency)
	void Start()
    {
        #if UNITY_EDITOR
        if (EditorUtility.audioMasterMute)
        {
          StopAllSound();
        }
        #endif

        bbox = musicBox.GetComponent<FMOD_StudioEventEmitter>();
        tuneGet = GameObject.FindGameObjectWithTag("LevelSettings").GetComponent<HORRIBLESCRIPT>();
        Camera = GameObject.FindGameObjectWithTag("MainCamera");

        // If there's a player in this level, go to them
        Player = null;
        Player = GameObject.FindGameObjectWithTag("Player");

        if (Player != null)
        {
            goTo = Player;
        }

        else // Use the camera, which is guarenteed to exist (but may result in reduced SFX due to distance)
        {
            goTo = Camera;
        }

        if (tuneGet.levelMusic == null)
        {
            bbox.Stop();
        }

        else if ((tunez == null && tuneGet.levelMusic != null)
        || (tunez != null && tuneGet.levelMusic.name != tunez.name))
        {
            bbox.Stop();
            bbox.CacheEventInstance(tuneGet.levelMusic, true);


            if (forcedStop == false)
                bbox.Play();

            /*if (tunez != null)
            {
                Debug.Log(tuneGet.levelMusic.name + " and " + tunez.name);
            }*/

            tunez = tuneGet.levelMusic;
        }
    }

    // Use this as Start because this calls at the Start of each level AFTER the first
    void OnLevelWasLoaded(int level)
    {
        bbox = musicBox.GetComponent<FMOD_StudioEventEmitter>();
        tuneGet = GameObject.FindGameObjectWithTag("LevelSettings").GetComponent<HORRIBLESCRIPT>();
        Camera = GameObject.FindGameObjectWithTag("MainCamera");

        // If there's a player in this level, go to them
        Player = null;
        Player = GameObject.FindGameObjectWithTag("Player");

        if (Player != null)
        {
            goTo = Player;
        }

        else // Use the camera, which is guarenteed to exist (but may result in reduced SFX due to distance)
        {
            goTo = Camera;
        }

        if (tuneGet.levelMusic == null)
        {
            bbox.Stop();
        }

        else if ((tunez == null && tuneGet.levelMusic != null)
        || (tunez != null && tuneGet.levelMusic.name != tunez.name))
        {
            bbox.Stop();
            bbox.CacheEventInstance(tuneGet.levelMusic, true);

            if(forcedStop == false)
                bbox.Play();

            /*if (tunez != null)
            {
                Debug.Log(tuneGet.levelMusic.name + " and " + tunez.name);
            }*/

            tunez = tuneGet.levelMusic;
        }
    }

    // Update is called once per frame, unlike the other three functions.  They're slackers.
    void Update()
    {
        // Make one last check for the player if they weren't found
        if(!lastPlayerCheckDone && Player == null)
        {
            Player = GameObject.FindGameObjectWithTag("Player");

            if (Player != null)
            {
                goTo = Player;
            }
        }

        // Go to the assigned position object
        gameObject.transform.position = goTo.transform.position;

        if((bbox.getPlaybackState() == FMOD.Studio.PLAYBACK_STATE.STOPPING
        || bbox.getPlaybackState() == FMOD.Studio.PLAYBACK_STATE.STOPPED)
        && tunez != null && !forcedStop)
        {
            bbox.Stop();
            //bbox.CacheEventInstance(tuneGet.levelMusic, true);
            bbox.Play();
        }
    }

    // Manually shut off the music.  Must use StartMusic() to turn it back on!
    public void StopMusic()
    {
        if(tunez)
        {
            bbox.Stop();
        }

        // Even if there's no music currently playing, this will prevent it from starting once there is music to play
        forcedStop = true;
    }

    // Manually turn on the music.
    public void StartMusic()
    {
        if (tunez)
        {
            bbox.Play();
        }

        // Even if there's no music currently loaded, this will make it start once there is music to play
        forcedStop = false;
    }

    // Manually shut off the music & SFX.  Indpendent from just turning off the music
    public void StopAllSound()
    {
		    listener.SetActive(false);
        AllStop = true;
    }

	// Manually turn on the music & SFX.  Indpendent from just turning on the music
    public void StartAllSound()
	{
		listener.SetActive(true);
        AllStop = false;
    }

    // Reduced sound for the pause menu
    public void PauseSound()
    {
        // Only pause if this level allows pausing
        if(tuneGet.canPause)
        {
            listener.transform.localPosition = new Vector3(0, 0, 10);
        }
    }

    // Make the sound "unpause" (i.e. go back to normal)
    public void UnpauseSound()
    {
        // Only unpause if this level allows pausing
        if (tuneGet.canPause)
        {
            listener.transform.localPosition = Vector3.zero;
        }
    }
}
