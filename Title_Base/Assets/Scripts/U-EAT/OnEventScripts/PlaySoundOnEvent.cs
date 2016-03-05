using UnityEngine;
using System.Collections;
using ActionSystem;
public class PlaySoundOnEvent : EditOnEvent
{
    public AudioClip SoundClip = null;

    public float Delay = 0;

    ActionGroup Grp;
    AudioSource Source;
	// Use this for initialization
	void Start()
    {
        Grp = this.GetActions();
        Source = this.GetAudioSource();
	}

    public override void OnEventFunc(EventData data)
    {
        if(!SoundClip)
        {
            return;
        }
        Source.clip = SoundClip;
        var Seq = Action.Sequence(Grp);
        Action.Delay(Seq, Delay);
        Action.Call(Seq, Source.Play);
        if(DispatchOnFinish)
        {
            Action.Delay(Seq, SoundClip.length);
            Action.Call(Seq, DispatchEvent);
        }
        
    }

    // Update is called once per frame
    void Update ()
    {
	
	}
}
