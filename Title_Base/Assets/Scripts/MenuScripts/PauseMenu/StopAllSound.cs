using UnityEngine;
using System.Collections;

public class StopAllSound : MenuButton
{
    GameObject SoundPlayer = null;
    TheMusicNeverEnds MusicPlayer = null;

    // Use this for initialization
    void Start()
    {
        SoundPlayer = GameObject.FindGameObjectWithTag("PersistentMusic");

        if (SoundPlayer)
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

        if (MusicPlayer && MusicPlayer.AllStop == true)
        {
            MusicPlayer.StartAllSound();
        }
        else if (MusicPlayer && MusicPlayer.AllStop == false)
        {
            MusicPlayer.StopAllSound();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
