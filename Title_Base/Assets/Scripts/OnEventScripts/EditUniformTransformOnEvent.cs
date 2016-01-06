using UnityEngine;
using System.Collections;
using ActionSystem;
using System.Collections.Generic;

public class EditUniformTransformOnEvent : EditOnEvent
{
    public bool Additive = true;
    public bool LocalPosition = true;
    public Vector3 TargetTransform = new Vector3();
    public float Duration = 1.0f;
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
        if(Additive)
        {
            TargetTransform += transform.position;
        }
    }

    public override void OnEventFunc(EventData data)
    {
        if (Seq.IsCompleted())
        {
            Seq = Action.Sequence(Actions);
            
        }
        if(LocalPosition)
        {
            Action.Property(Seq, transform.GetProperty(o => o.localPosition), TargetTransform, Duration, EasingCurve);
        }
        else
        {
            Action.Property(Seq, transform.GetProperty(o => o.position), TargetTransform, Duration, EasingCurve);
        }
        
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
