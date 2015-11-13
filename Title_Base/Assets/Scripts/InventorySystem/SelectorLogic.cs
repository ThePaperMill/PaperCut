using UnityEngine;
using System.Collections;
using ActionSystem;

public class SelectorLogic : EventHandler
{
    private ActionGroup Seq   = new ActionGroup();
    public  AnimationCurve Curve = AnimationCurve.EaseInOut(0, 0, 1, 1);

    // Use this for initialization
    void Start ()
    {
        EventSystem.GlobalHandler.Connect(Events.ActivateSelector, OnActivateSelector);
        EventSystem.GlobalHandler.Connect(Events.DeactivateSelector, OnDeactivateSelector);
    }

    void OnActivateSelector(EventData data)
    {
        // lerp down to the camera
        var test = ActionSystem.Action.Sequence(Seq);
        var finalPos = new Vector3(0.0f,0.0f,0.0f);
        finalPos.z = transform.localPosition.z;
        
        Action.Property(test, gameObject.transform.GetProperty(o => o.localPosition), finalPos, 1.5, Curve);
    }

    void OnDeactivateSelector(EventData data)
    {
        // lerp up away from the camera
        var test = ActionSystem.Action.Sequence(Seq);
        var finalPos = new Vector3(0.0f, 5.0f, 0.0f);
        finalPos.z = transform.localPosition.z;
        
        Action.Property(test, gameObject.transform.GetProperty(o => o.localPosition), finalPos, 1.5, Curve);
    }

    void Update ()
    {
        // update our sequence
        Seq.Update(Time.deltaTime);
    }

    void OnDestroy()
    {
        EventSystem.GlobalHandler.Disconnect(Events.ActivateSelector, OnActivateSelector);
        EventSystem.GlobalHandler.Disconnect(Events.DeactivateSelector, OnDeactivateSelector);
    }
}
