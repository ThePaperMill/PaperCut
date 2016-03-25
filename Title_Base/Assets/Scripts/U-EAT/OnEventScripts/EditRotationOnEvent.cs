using UnityEngine;
using System.Collections;
using ActionSystem;
using System.Collections.Generic;

public class EditRotationOnEvent : EditOnEvent
{

    public bool Additive = false;
    public bool LocalRotation = true;
    public Vector3 TargetRotation = new Vector3(1, 1, 1);
    public float Duration = 1.0f;
    public Curve EasingCurve = Ease.Linear;
    // Use this for initialization
    ActionSequence Seq;
    //USE START NOT AWAKE
    public override void Awake()
    {
        base.Awake();
        if(Additive)
        {
            if(LocalRotation)
            {
                TargetRotation += gameObject.transform.localEulerAngles;
            }
            else
            {
                TargetRotation += gameObject.transform.eulerAngles;
            }
            
        }
        Seq = Action.Sequence(Actions);
        
    }

    public override void OnEventFunc(EventData data)
    {
        if (Seq.IsCompleted())
        {
            Seq = Action.Sequence(Actions);

        }
        if (LocalRotation)
        {
            Action.Property(Seq, transform.GetProperty(o => o.localEulerAngles), TargetRotation, Duration, EasingCurve);
        }
        else
        {
            Action.Property(Seq, transform.GetProperty(o => o.eulerAngles), TargetRotation, Duration, EasingCurve);
        }

        EditChecks(Seq);
    }

    void OnDestroy()
    {
        
    }
}
