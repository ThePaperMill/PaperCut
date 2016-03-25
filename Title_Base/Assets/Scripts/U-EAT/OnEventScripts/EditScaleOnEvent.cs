using UnityEngine;
using System.Collections;
using ActionSystem;
using System.Collections.Generic;

public class EditScaleOnEvent : EditOnEvent
{
    public bool Additive = false;
    bool LocalScale = true;
    public Vector3 TargetScale = new Vector3(1, 1, 1);
    public float Duration = 1.0f;
    public Curve EasingCurve = Ease.Linear;
    
    // Use this for initialization
    ActionSequence Seq;
    public override void Awake()
    {
        base.Awake();
        if (Additive)
        {
            if (LocalScale)
            {
                TargetScale.x *= gameObject.transform.localScale.x;
                TargetScale.y *= gameObject.transform.localScale.y;
                TargetScale.z *= gameObject.transform.localScale.z;
            }
            //else
            //{
                
            //    TargetScale.x *= gameObject.transform.lossyScale.x;
            //    TargetScale.y *= gameObject.transform.lossyScale.y;
            //    TargetScale.z *= gameObject.transform.lossyScale.z;
            //}

        }
        Seq = Action.Sequence(Actions);
        
    }

    public override void OnEventFunc(EventData data)
    {
        if (Seq.IsCompleted())
        {
            Seq = Action.Sequence(Actions);
        }
        
        if (LocalScale)
        {
            Action.Property(Seq, transform.GetProperty(o => o.localScale), TargetScale, Duration, EasingCurve);
        }
        //else
        //{
        //    Debug.Log(transform.GetProperty(o => o.lossyScale));
        //    Action.Property(Seq, transform.GetProperty(o => o.lossyScale), TargetScale, Duration, EasingCurve);
        //}
        
        EditChecks(Seq);
    }

    void OnDestroy()
    {
        
    }
}
