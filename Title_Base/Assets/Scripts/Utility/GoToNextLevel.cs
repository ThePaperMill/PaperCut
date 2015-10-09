/*********************************
 * GoToNextLevel.cs
 * Troy
 * Created 9/28/2015
 * Copyright © 2015 DigiPen Institute of Technology, All Rights Reserved
 *********************************/

using UnityEngine;
using System.Collections;

//InteractRigid is attached to objects that will be created around interactable objects. It should only be attached to an object archetype. 
public class GoToNextLevel : MonoBehaviour
{
	public string NextLevel;

	bool delay = true;
	
	void Update()
	{		
		// Give the game a frame to load before interacting
		if(delay)
		{
			delay = false;
		}
	}
	
	void OnTriggerEnter(Collider collision)
	{
		// If the object colliding is the player, then add the parent object to the interact manager's array of currently colliding objects. 
		if(collision.gameObject.GetComponent("CharacterController3d") != null && !delay)
		{
			Application.LoadLevel(NextLevel);
		}
	}
}