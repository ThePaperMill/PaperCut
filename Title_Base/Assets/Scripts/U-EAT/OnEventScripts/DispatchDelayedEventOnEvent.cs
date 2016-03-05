using UnityEngine;
using System.Collections;
using ActionSystem;
public class DispatchDelayedEventOnEvent : OnEvent
{
    public float Delay = 1.0f;


    ActionGroup Actions = null;
	// Use this for initialization
	public override void Awake ()
    {
        base.Awake();
        Actions = this.GetActions();
        this.DelayedDispatch = true;
        
    }

    public override void OnEventFunc(EventData data)
    {
        var Seq = Action.Sequence(Actions);
        Action.Delay(Seq, Delay);
        Action.Call(Seq, this.DispatchEvent);
    }

    
}