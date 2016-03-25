using UnityEngine;
using System.Collections;
using ActionSystem;
using System.Collections.Generic;

public class EditPositionOnEvent : EditOnEvent
{
    public bool Additive = false;
    public bool LocalPosition = true;
    public Vector3 TargetTransform = new Vector3();
    public float Duration = 1.0f;
    public Curve EasingCurve = Ease.Linear;
    // Use this for initialization
    ActionSequence Seq = null;
    //USE START NOT AWAKE
    public override void Awake()
    {
        base.Awake();
        Seq = Action.Sequence(this.GetActions());
        
        
        if(Additive)
        {
            if(LocalPosition)
            {
                TargetTransform += transform.localPosition;
            }
            else
            {
                TargetTransform += transform.position;
            }
            
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

        EditChecks(Seq);
    }

    void OnDestroy()
    {
        
    }
}
