/*********************************
 * InteractManager.cs
 * Troy
 * Created 9/11/2015
 * Copyright © 2015 DigiPen Institute of Technology, All Rights Reserved
 *********************************/

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using ActionSystem;

public class InteractManager : MonoBehaviour
{
	List<GameObject> AllInteractableObjects = new List<GameObject>{};
	GameObject Player;
	public Object HighlightArchetype;
	GameObject Highlight;
	GameObject Closest = null;
	public float HighlightHeight = 1.0f;
	public float EaseSpeed = 0.5f;
	GameObject CurrentlyInteractedObject;
	
	void Awake()
	{
        //Store the player object
        Player = GameObject.FindGameObjectWithTag("Player");

        if(Player == null)
        {
            Debug.Log("If we have no player, then how can we have a game?");
        }
	}
	
	void Update()
	{
        if (Player == null)
        {
            //Store the player object
            Player = GameObject.FindGameObjectWithTag("Player");
        }

        //grab the position of the closest object
        GameObject closestobj = FindClosestObjectToPlayer();
		//if(closestobj){print (closestobj.name + " is closest");}

		//If there are objects in the array and there is no highlight object,
		if(AllInteractableObjects.Count > 0 && Highlight == null)
		{
			Closest = closestobj;
			Vector3 pos = closestobj.transform.position;
			pos += new Vector3(0, HighlightHeight, 0);
			// We'll need a new highlight 
			Highlight = Instantiate(HighlightArchetype, pos, Quaternion.identity) as GameObject;
		}
		
		//otherwise, If there are no objects in the array and the highlight object exists,
		else if(AllInteractableObjects.Count <= 0 && Highlight != null)
		{
			Destroy(Highlight);
			closestobj = null;
			Closest = null;
			//print ("ded.");
		}
		//otherwise, interpolate the highlight to it's proper position. 
		else if(AllInteractableObjects.Count > 0 && closestobj != Closest && closestobj != null)
		{
			Closest = closestobj;
			Vector3 pos = closestobj.transform.position;
			pos += new Vector3(0, HighlightHeight, 0);
			//print (closestobj.name + " is king of team");

			// Setup the interpolation
			HighlightController getAG = Highlight.GetComponent("HighlightController") as HighlightController;
			getAG.setUp(true);
			getAG.GoToPos = pos;

		}

		//if the currently closest object is being interacted with, then hide the icon
		if(closestobj != null)
		{
			Interactable toCheck = closestobj.GetComponent("Interactable") as Interactable;
			MeshRenderer visible = Highlight.GetComponent("MeshRenderer") as MeshRenderer;

			if(toCheck.GetIsInInteraction() == true)
			{
				visible.enabled = false;
			}
			else
			{
				visible.enabled = true;
			}
		}
		
		// Remove all of the null objects (done here in case objects are destroyed in level)
		AllInteractableObjects.RemoveAll(GameObject => GameObject == null);
    //print ("Can talk with " + AllInteractableObjects.Count + " people.");

    
         
    if(Closest)
    {
        if (InputManager.GetSingleton.IsInputTriggered(GlobalControls.InteractKeys))
        {
            Closest.DispatchEvent(Events.Interact);
        }
    }
  }

    public GameObject GetClosestObj()
    {
        return Closest;
    }

	public void OnInteractEvent()
	{
		// Find the nearest object
		GameObject closest = FindClosestObjectToPlayer();
		
		// If the currently interacted object is not the closest object anymore,
		if(CurrentlyInteractedObject != closest && CurrentlyInteractedObject != null)
		{
			// Then we can tell it to go away
			Interactable deInteract = CurrentlyInteractedObject.GetComponent("Interactable") as Interactable;
			deInteract.SetIsInInteraction(false);
		}
		
		// Bring the actual closest object in		
		if(closest != null)
		{
			// Create a ScriptEvent
			Interactable inter = closest.GetComponent("Interactable") as Interactable;
			inter.SetIsInInteraction(true);
		}
		
		// Then make the last dispatched object the new currently interacted
		CurrentlyInteractedObject = closest;
	}

	GameObject FindClosestObjectToPlayer()
	{
		GameObject closest = null;
		float bestDist;
		if(Closest != null)
		{
			bestDist = Vector3.Distance(Closest.transform.position, Player.transform.position);
			closest = Closest;
		}

		else
		{
			bestDist = float.MaxValue;
		}

		for(int i = 0; i < AllInteractableObjects.Count; ++i)
		{
			GameObject obj = AllInteractableObjects[i];
			float dist = Vector3.Distance(obj.transform.position, Player.transform.position);
			
			if(dist < bestDist)
			{
				bestDist = dist;
				closest = obj;
			}
		}
		//if(closest != null){print(closest.name);}
		return closest;
	}
	
	public void AddInteractableObject(GameObject cog)
	{
		//print("Add? " + AllInteractableObjects.Contains(cog));
		// Only add unique objects
		if(!AllInteractableObjects.Contains(cog))
		{
			AllInteractableObjects.Add(cog);
			//print("Added " + cog.name + " at " + cog.transform.position);
		}
	}
	public void RemoveInteractableObject(GameObject cog)
	{
		// Tell the object to go away
		Interactable deInteract = cog.GetComponent("Interactable") as Interactable;
		deInteract.SetIsInInteraction(false);
		
		for(int i = 0; i < AllInteractableObjects.Count; ++i)
		{
			GameObject obj = AllInteractableObjects[i];
			if(obj == cog)
			{
				AllInteractableObjects.RemoveAt(i);
				//print("Removed Object!");
				return;
			}
		}
	}
}