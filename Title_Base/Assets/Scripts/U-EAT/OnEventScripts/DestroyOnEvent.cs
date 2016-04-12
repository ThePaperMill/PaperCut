/****************************************************************************/
/*!
\file  DestroyOnEvent.cs
\author Joshus Biggs 
\brief  
 
  
    © 2016 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;
using ActionSystem;
public class DestroyOnEvent : OnEvent
{
	void Start()
    {
        
	}

    public override void OnEventFunc(EventData data)
    {
        Destroy(gameObject);
    }

}
