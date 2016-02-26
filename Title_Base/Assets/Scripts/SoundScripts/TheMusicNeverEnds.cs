﻿using UnityEngine;
using System.Collections;

public class TheMusicNeverEnds : MonoBehaviour
{
    FMODAsset tunez = null;
    FMOD_StudioEventEmitter bbox;

    bool forcedStop = false;

    // Use this for initial initialization (thanks Unity)
    void Awake()
    {
        DontDestroyOnLoad(this);
    }
           
    // Use this for initialization (but only once, because code effeciency)
	void Start()
    {
        bbox = gameObject.GetComponent<FMOD_StudioEventEmitter>();
        HORRIBLESCRIPT tuneGet = GameObject.FindGameObjectWithTag("LevelSettings").GetComponent<HORRIBLESCRIPT>();

        if (tuneGet.levelMusic == null)
        {
            bbox.Stop();
        }

        else if ((tunez == null && tuneGet.levelMusic != null)
        || (tunez != null && tuneGet.levelMusic.name != tunez.name))
        {
            bbox.Stop();
            bbox.CacheEventInstance(tuneGet.levelMusic, true);
            bbox.Play();

            if (tunez != null)
            {
                Debug.Log(tuneGet.levelMusic.name + " and " + tunez.name);
            }

            tunez = tuneGet.levelMusic;
        }
    }

    // Use this as Start because this calls at the Start of each level AFTER the first
    void OnLevelWasLoaded(int level)
    {
        //DontDestroyOnLoad(this);

        bbox = gameObject.GetComponent<FMOD_StudioEventEmitter>();
        HORRIBLESCRIPT tuneGet = GameObject.FindGameObjectWithTag("LevelSettings").GetComponent<HORRIBLESCRIPT>();

        if (tuneGet.levelMusic == null)
        {
            bbox.Stop();
        }

        else if ((tunez == null && tuneGet.levelMusic != null)
        || (tunez != null && tuneGet.levelMusic.name != tunez.name))
        {
            bbox.Stop();
            bbox.CacheEventInstance(tuneGet.levelMusic, true);
            bbox.Play();

            if (tunez != null)
            {
                Debug.Log(tuneGet.levelMusic.name + " and " + tunez.name);
            }

            tunez = tuneGet.levelMusic;
        }
    }

    // Update is called once per frame, unlike the other three functions.  They're slackers.
    void Update()
    {
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

    // Manually shut off the music & SFX.  Both can be turned on & off individually
    public void StopAllSound()
    {
        StopMusic();
        //StopSFX();
    }

    // Manually turn on the music & SFX.  Both can be turned on & off individually
    public void StartAllSound()
    {
        StartMusic();
        //StartSFX();
    }
}
