/****************************************************************************/
/*!
\file   DropDown.cs
\author Steven Gallwas 
\brief  
	makes an object move up or down based on events.
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/

using UnityEngine;
using System.Collections;
using ActionSystem;
using System.Collections.Generic;
using Assets.Scripts.ConversationSystem;

public class DropDown : MonoBehaviour
{
    public bool UseLocalPosition = false;
    public bool MaintainZPos     = false;
    public bool StartRaised      = false;

    public List<string> LowerDownEvents = new List<string>();
    public List<string> RaiseUpEvents   = new List<string>();

    public Vector3 RaisePosition = new Vector3();

    ActionGroup grp = new ActionGroup();
    Vector3 StartingPosition = new Vector3();

    public float Duration = 1.0f;

    // Use this for initialization
    void Start()
    {
        foreach(var s in LowerDownEvents)
        {
            EventSystem.GlobalHandler.Connect(s, OnLowerEvent);
        }

        foreach (var s in RaiseUpEvents)
        {
            EventSystem.GlobalHandler.Connect(s, OnRaiseEvent);
        }

        StartingPosition = transform.localPosition;

        if(StartRaised == true)
        {
            if(UseLocalPosition)
            {
                Vector3 pos = StartingPosition + RaisePosition;

                if (MaintainZPos)
                    pos = new Vector3(pos.x, pos.y, StartingPosition.z);

                transform.localPosition = pos;
            }
            else
            {
                Vector3 pos = StartingPosition + RaisePosition;

                if (MaintainZPos)
                    pos = new Vector3(pos.x, pos.y, StartingPosition.z);

                transform.localPosition = pos;

            }
        }
    }

    void OnLowerEvent(EventData data)
    {
        if(UITextManager.ConversationText != null)
        {
                UITextManager.ConversationText.Disappear();
        }

        ActionSequence temp = Action.Sequence(grp);

        Vector3 pos = StartingPosition;

        if (UseLocalPosition)
        {
            Action.Property(temp, this.gameObject.transform.GetProperty(o => o.localPosition), pos, Duration, Ease.Linear);
        }
        else
        {
            Action.Property(temp, this.gameObject.transform.GetProperty(o => o.position), pos, Duration, Ease.Linear);
        }
    }

    void OnRaiseEvent(EventData data)
    {
        ActionSequence temp = Action.Sequence(grp);

        Vector3 pos = StartingPosition + RaisePosition;

        if (MaintainZPos)
            pos = new Vector3(pos.x, pos.y, StartingPosition.z);

        if (UseLocalPosition)
        {
            Action.Property(temp, this.gameObject.transform.GetProperty(o => o.localPosition), pos, Duration, Ease.Linear);
        }
        else
        {
            Action.Property(temp, this.gameObject.transform.GetProperty(o => o.position), pos, Duration, Ease.Linear);
        }
    }


    // Update is called once per frame
    void Update()
    {
        grp.Update(Time.unscaledDeltaTime);
    }

    void OnDestroy()
    {
        foreach (var s in LowerDownEvents)
        {
            EventSystem.GlobalHandler.Disconnect(s, OnLowerEvent);
        }

        foreach (var s in RaiseUpEvents)
        {
            EventSystem.GlobalHandler.Disconnect(s, OnRaiseEvent);
        }
    }
}
