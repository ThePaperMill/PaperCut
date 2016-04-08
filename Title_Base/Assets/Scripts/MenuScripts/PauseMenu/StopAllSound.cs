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
        if (MusicPlayer && MusicPlayer.AllStop == true)
        {
            print("starting all");
            MusicPlayer.StartAllSound();
        }
        else if (MusicPlayer && MusicPlayer.AllStop == false)
        {
            print("stopping all");
            MusicPlayer.StopAllSound();
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
