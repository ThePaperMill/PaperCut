/*********************************
 * GoToNextLevel.cs
 * Troy
 * Created 9/28/2015
 * Copyright © 2015 DigiPen Institute of Technology, All Rights Reserved
 *********************************/

using UnityEngine;
using System.Collections;

public class GoToNextLevel : MonoBehaviour
{
	public string NextLevel;

	bool delay = true;

    public bool PrevLevelTrigger = false;
    public bool NextLevelTrigger = false;

	void Update()
	{		
		// Give the game a frame to load before interacting
		if(delay)
		{
			delay = false;
		}

        if(InputManager.GetSingleton.IsKeyTriggered(KeyCode.Alpha1) && PrevLevelTrigger)
        {
            Application.LoadLevel(NextLevel);
        }
        else if (InputManager.GetSingleton.IsKeyTriggered(KeyCode.Alpha2) && NextLevelTrigger)
        {
            Application.LoadLevel(NextLevel);
        }

    }
	
	void OnTriggerEnter(Collider collision)
	{
		// If the object colliding is the player, then add the parent object to the interact manager's array of currently colliding objects. 
		if(collision.gameObject.GetComponent("CustomDynamicController") != null && !delay)
		{
			Application.LoadLevel(NextLevel);
		}
	}
}