/****************************************************************************/
/*!
\file  SetActiveOnEvent.cs
\author Joshus Biggs 
\brief  
 
  
    © 2016 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;
using ActionSystem;
using System.Collections.Generic;

public enum ActiveState
{
    Inactive,
    Active
}

public class SetActiveOnEvent : OnEvent
{
    public OnEvent TargetComp;
    public ActiveState State = ActiveState.Active;

    void Start()
    {
        
    }


    public override void OnEventFunc(EventData data)
    {
        if(TargetComp != null)
        {
            if(State == ActiveState.Active)
            {
                TargetComp.Active = true;
            }
            else
            {
                TargetComp.Active = false;
            }
            
        }
    }

    void OnDestroy()
    {
        
    }
}
