/****************************************************************************/
/*!
\file  BasicDropDown.cs
\author Steven Gallwas 
\brief  
 
  
    © 2016 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;
using ActionSystem;

public class BasicDropDown : EventHandler
{
    ActionGroup grp = new ActionGroup();
    Vector3 StartingPosition = new Vector3();

    public string DropDownEvent = "";
    public string RaiseEvent = "";

    // Use this for initialization
    void Start()
    {
        EventSystem.GlobalHandler.Connect(DropDownEvent, OnInitiateQuit);
        EventSystem.GlobalHandler.Connect(RaiseEvent, OnCancelQuit);
        StartingPosition = transform.localPosition;
    }

    void OnInitiateQuit(EventData data)
    {
        ActionSequence temp = Action.Sequence(grp);
        Action.Property(temp, this.gameObject.transform.GetProperty(o => o.localPosition), new Vector3(0, 0, 0.5f * StartingPosition.z), 1.0f, Ease.Linear);
    }

    void OnCancelQuit(EventData data)
    {
        ActionSequence temp = Action.Sequence(grp);
        Action.Property(temp, this.gameObject.transform.GetProperty(o => o.localPosition), StartingPosition, 0.5f, Ease.Linear);
    }


    // Update is called once per frame
    void Update()
    {
        grp.Update(Time.unscaledDeltaTime);
    }

    void OnDestroy()
    {
        EventSystem.GlobalHandler.Disconnect(Events.InitiateQuitEvent, OnInitiateQuit);
        EventSystem.GlobalHandler.Disconnect(Events.CancelQuitEvent, OnCancelQuit);
    }
}
