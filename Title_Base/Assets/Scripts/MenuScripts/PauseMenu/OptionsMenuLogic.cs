/****************************************************************************/
/*!
\file  OptionsMenuLogic.cs
\author Steven Gallwas 
\brief  
 
  
    © 2016 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;
using ActionSystem;

public class OptionsMenuLogic : MonoBehaviour
{
    ActionGroup grp = new ActionGroup();
    Vector3 StartingPosition = new Vector3();
    public float FinalPositionZModifier = 0.5f;

    // Use this for initialization
    void Start()
    {
        EventSystem.GlobalHandler.Connect(Events.OptionsDropDown, OnOptionsDropDown);
        EventSystem.GlobalHandler.Connect(Events.OptionsRaise, OnOptionsRaise);
        StartingPosition = transform.localPosition;
    }

    void OnOptionsDropDown(EventData data)
    {
        ActionSequence temp = Action.Sequence(grp);
        Action.Property(temp, this.gameObject.transform.GetProperty(o => o.localPosition), new Vector3(0, 0, FinalPositionZModifier * StartingPosition.z), 1.0f, Ease.Linear);
    }

    void OnOptionsRaise(EventData data)
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
        EventSystem.GlobalHandler.Disconnect(Events.OptionsDropDown, OnOptionsDropDown);
        EventSystem.GlobalHandler.Disconnect(Events.OptionsRaise, OnOptionsRaise);
    }
}
