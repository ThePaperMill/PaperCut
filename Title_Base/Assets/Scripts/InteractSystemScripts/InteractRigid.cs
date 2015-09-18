/*********************************
 * InteractRigid.cs
 * Troy
 * Created 9/11/2015
 * Copyright © 2015 DigiPen Institute of Technology, All Rights Reserved
 *********************************/

using UnityEngine;
using System.Collections;

//InteractRigid is attached to objects that will be created around interactible objects. It should only be attached to an object archetype. 
public class InteractRigid : MonoBehaviour
{
	public Transform LevelSettings;

	//Declare a parent object to keep track of. 
	public GameObject Parent;
	
	void Update()
	{
		//If the parent object exists, move to it's position every frame
		if(this.Parent != null)
		{
			gameObject.transform.position = Parent.transform.position;
		}
	}
	
	public void SetParent(GameObject newParent)
	{
		//Set this object's parent. 
		Parent = newParent;
	}
	
	void OnCollisionEnter(Collision collision)
	{
		//If the object colliding is the player, then add the parent object to the interact manager's array of currently colliding objects. 
		if(collision.gameObject.GetComponent("CharacterController3D") != null)
		{
			InteractManager toAdd = LevelSettings.GetComponent("InteractManager") as InteractManager;
			toAdd.AddInteractableObject(Parent);
		}
	}
	
	void OnCollisionExit(Collision collision)
	{
		//If the object no longer colliding is the player, then remove the parent object from the interact mnager's array of currently colliding objects.
		if(collision.gameObject.GetComponent("CharacterController3D") != null)
		{
			if(this.LevelSettings != null)
			{
				InteractManager toRemove = LevelSettings.GetComponent("InteractManager") as InteractManager;
				toRemove.RemoveInteractableObject(Parent);
			}
		}
		//And send a "Disinteract event" to this object's owner. 
		
	}
}