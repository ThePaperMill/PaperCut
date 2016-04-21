using UnityEngine;
using System.Collections;

public enum TEXT_CHANGE_TYPE
{
    TEXT_MUTE_MUSIC,
    TEXT_MUTE_ALL,
    TEXT_TOGGLE_FULLSCREEN,
}

public class MuteMusicTextChange : MonoBehaviour
{
    GameObject SoundPlayer = null;
    TheMusicNeverEnds MusicPlayer = null;
    TextMesh TMesh = null;

    public TEXT_CHANGE_TYPE Type = TEXT_CHANGE_TYPE.TEXT_MUTE_ALL;

    // Use this for initialization
    void Start()
    {
        SoundPlayer = GameObject.FindGameObjectWithTag("PersistentMusic");
        TMesh = GetComponent<TextMesh>();

        if (SoundPlayer)
        {
            MusicPlayer = SoundPlayer.GetComponent<TheMusicNeverEnds>();
        }
    }


    // Update is called once per frame
    void Update ()
    {
        if(!SoundPlayer || !MusicPlayer)
        {
            SoundPlayer = GameObject.FindGameObjectWithTag("PersistentMusic");

            if (SoundPlayer)
            {
                MusicPlayer = SoundPlayer.GetComponent<TheMusicNeverEnds>();
            }
        }


        if (Type == TEXT_CHANGE_TYPE.TEXT_MUTE_MUSIC)
        {
            if (MusicPlayer && MusicPlayer.forcedStop)
            {
                TMesh.text = "Resume Music";
            }
            else if (MusicPlayer && MusicPlayer.forcedStop == false)
            {
                TMesh.text = "Mute Music";
            }
        }
        else if(Type == TEXT_CHANGE_TYPE.TEXT_MUTE_ALL)
        {
            if (MusicPlayer && MusicPlayer.AllStop == true)
            {
                TMesh.text = "Resume Sound";
            }
            else if (MusicPlayer && MusicPlayer.AllStop == false)
            {
                TMesh.text = "Mute Sound";
            }
        }
        else if (Type == TEXT_CHANGE_TYPE.TEXT_TOGGLE_FULLSCREEN)
        {
            if (Screen.fullScreen)
            {
                TMesh.text = "Windowed";
            }
            else if (!Screen.fullScreen)
            {
                TMesh.text = "Fullscreen";
            }
        }


    }
}
