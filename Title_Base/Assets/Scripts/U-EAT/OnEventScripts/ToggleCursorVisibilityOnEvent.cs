/****************************************************************************/
/*!
\file  ToggleCursorVisibilityOnEvent.cs
\author Joshus Biggs 
\brief  
 
  
    © 2016 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;
using ActionSystem;
using System.Collections.Generic;

public class ToggleCursorVisibilityOnEvent : OnEvent
{

    void Start()
    {

    }


    public override void OnEventFunc(EventData data)
    {
        Cursor.visible = !Cursor.visible;
    }

    void OnDestroy()
    {

    }
}

