using UnityEngine;
using System.Collections;
using ActionSystem;
using System.Collections.Generic;

public class EditUniformScaleOnEvent : EditOnEvent
{
    
    public Vector3 TargetScale = new Vector3(1, 1, 1);
    public float ScaleDuration = 1.0f;
    public Curve EasingCurve = Ease.Linear;
    // Use this for initialization
    static ActionSequence Seq;
    //USE START NOT AWAKE
    void Start()
    {
        
        if (Seq == null)
        {
            Seq = Action.Sequence(Actions);
        }
    }

    public override void OnEventFunc(EventData data)
    {
        if (Seq.IsCompleted())
        {
            Seq = Action.Sequence(Actions);
            
        }
        Action.Property(Seq, transform.GetProperty(o => o.localScale), TargetScale, ScaleDuration, EasingCurve);
        if (DeactivateUntilFinished)
        {
            Active = false;
            Action.Call(Seq, SetActive, true);
        }
        if (DispatchOnFinish)
        {
            Action.Call(Seq, DispatchEvent);
        }
    }

    void OnDestroy()
    {
        
    }
}
