/*********************************
 * InteractRigid.cs
 * Troy
 * Created 9/11/2015
 * Copyright © 2015 DigiPen Institute of Technology, All Rights Reserved
 *********************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//InteractRigid is attached to objects that will be created around interactable objects. It should only be attached to an object archetype. 
public class InteractRigid : MonoBehaviour
{
	public GameObject LevelSettings;
	bool delay = true;
	bool secondition = false;
	List<Collider> past = new List<Collider>{};

	//Declare a parent object to keep track of. 
	public GameObject Parent;
	
	void Start()
	{
		//Store the player object
		LevelSettings = GameObject.Find("LevelSettings");
	}


	void Update()
	{
		//If the parent object exists, move to it's position every frame
		if(this.Parent != null && !delay)
		{
			gameObject.transform.position = Parent.transform.position;
		}

		// Give the game a frame to load before interacting
		if(delay)
		{
			delay = false;
		}

		else if(secondition && past != null)
		{
			secondition = false;
			SecondFrameResponse();
		}
	}
	
	public void SetParent(GameObject newParent)
	{
		//Set this object's parent
		Parent = newParent;
	}
	
	public void SetLevelSettings(GameObject lvlSetts)
	{
		//Set this object's level settings
		LevelSettings = lvlSetts;
	}
	
	void SecondFrameResponse()
	{
		foreach(Collider collision in past)
		{
			// If the object colliding is the player, then add the parent object to the interact manager's array of currently colliding objects. 
			if(collision.gameObject.name == "Player" && !delay)
			{
				if(this.LevelSettings != null)
				{
					InteractManager toAdd = LevelSettings.GetComponent("InteractManager") as InteractManager;
					if (toAdd == null) {print ("Turns out you can't interact with something that doesn't exist.");}
					toAdd.AddInteractableObject(Parent);
				}
			}
		};
	}
	
	void OnTriggerEnter(Collider collision)
	{
		// If the object colliding is the player, then add the parent object to the interact manager's array of currently colliding objects. 
		if(collision.gameObject.name == "Player" && !delay)
		{
			if(this.LevelSettings != null)
			{
				InteractManager toAdd = LevelSettings.GetComponent("InteractManager") as InteractManager;
				if (toAdd == null) {print ("Turns out you can't interact with something that doesn't exist.");}
				toAdd.AddInteractableObject(Parent);
			}
		}
		
		// Add to the list next frame if this is the first frame
		else if(delay)
		{
			secondition = true;
			past.Add(collision);
		}
	}
	
	void OnTriggerExit(Collider collision)
	{
		//If the object no longer colliding is the player, then remove the parent object from the interact mnager's array of currently colliding objects.
		if(collision.gameObject.name == "Player" && !delay)
		{
			if(this.LevelSettings != null)
			{
				InteractManager toRemove = LevelSettings.GetComponent("InteractManager") as InteractManager;
				toRemove.RemoveInteractableObject(Parent);
			}
		}
		//And send a "Disinteract event" to this object's owner (Never Done?) 
		
	}
}