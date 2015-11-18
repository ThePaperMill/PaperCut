using UnityEngine;
using System.Collections;
using ActionSystem;

public class PauseMenuLogic : EventHandler
{
    ActionGroup grp = new ActionGroup();
    Vector3 StartingPosition = new Vector3();

    // Use this for initialization
    void Start ()
    {
        EventSystem.GlobalHandler.Connect(Events.PauseGameEvent, OnPauseGameEvent1);
        EventSystem.GlobalHandler.Connect(Events.ResumeGameEvent, OnResumeGameEvent1);
        StartingPosition = transform.localPosition;
    }

    void OnPauseGameEvent1(EventData data)
    {
        ActionSequence temp = Action.Sequence(grp);
        Action.Property(temp, this.gameObject.transform.GetProperty(o => o.localPosition), new Vector3(0, 0, 0.5f * StartingPosition.z), 1.0f, Ease.Linear);
    }

    void OnResumeGameEvent1(EventData data)
    {
        ActionSequence temp = Action.Sequence(grp);
        Action.Property(temp, this.gameObject.transform.GetProperty(o => o.localPosition), StartingPosition, 0.5f, Ease.Linear);
    }


    // Update is called once per frame
    void Update ()
    {
        grp.Update(Time.unscaledDeltaTime);
	}

    void OnDestroy()
    {
        EventSystem.GlobalHandler.Disconnect(Events.PauseGameEvent, OnPauseGameEvent1);
        EventSystem.GlobalHandler.Disconnect(Events.ResumeGameEvent, OnResumeGameEvent1);
    }
}
