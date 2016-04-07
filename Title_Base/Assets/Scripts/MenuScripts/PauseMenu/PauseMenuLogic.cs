/****************************************************************************/
/*!
\file   PauseMenuLogic.cs
\author Steven Gallwas
\brief  
    Drops down the pause menu.
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
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
        EventSystem.GlobalHandler.Connect(Events.PauseGameEvent, OnPauseGameEvent);
        EventSystem.GlobalHandler.Connect(Events.ResumeGameEvent, OnResumeGameEvent);
        EventSystem.GlobalHandler.Connect(Events.InitiateQuitEvent, OnResumeGameEvent);
        EventSystem.GlobalHandler.Connect(Events.CancelQuitEvent, OnPauseGameEvent);
        
        EventSystem.GlobalHandler.Connect("RestartConfirmation", OnResumeGameEvent);
        EventSystem.GlobalHandler.Connect(Events.OptionsRaise, OnPauseGameEvent);
        EventSystem.GlobalHandler.Connect(Events.OptionsDropDown, OnResumeGameEvent);

        StartingPosition = transform.localPosition;
    }

    void OnPauseGameEvent(EventData data)
    {
        grp.Clear();
        ActionSequence temp = Action.Sequence(grp);
        Action.Property(temp, this.gameObject.transform.GetProperty(o => o.localPosition), new Vector3(0, 0, 0.5f * StartingPosition.z), 1.0f, Ease.Linear);
    }

    void OnResumeGameEvent(EventData data)
    {
        grp.Clear();
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
        EventSystem.GlobalHandler.Disconnect(Events.PauseGameEvent, OnPauseGameEvent);
        EventSystem.GlobalHandler.Disconnect(Events.ResumeGameEvent, OnResumeGameEvent);
        EventSystem.GlobalHandler.Disconnect(Events.InitiateQuitEvent, OnResumeGameEvent);
        EventSystem.GlobalHandler.Disconnect(Events.CancelQuitEvent, OnPauseGameEvent);
        EventSystem.GlobalHandler.Disconnect("RestartConfirmation", OnPauseGameEvent);
        EventSystem.GlobalHandler.Disconnect(Events.OptionsRaise, OnPauseGameEvent);
        EventSystem.GlobalHandler.Disconnect(Events.OptionsDropDown, OnResumeGameEvent);
    }
}
