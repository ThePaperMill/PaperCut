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
	//public float HeightOfHighlight = 1.5f;
	public float EaseSpeed = 0.5f;
	GameObject CurrentlyInteractedObject;
	
	void Start()
	{
		//Store the player object
		Player = GameObject.Find("Player");
	}
	
	void Update()
	{
		//grab the position of the closest object
		GameObject closestobj = FindClosestObjectToPlayer();

		//If there are objects in the array and there is no highlight object,
		if(AllInteractableObjects.Count > 0 && Highlight == null)
		{
			Vector3 pos = closestobj.transform.position;
			//pos += new Vector3(0, HeightOfHighlight, 0);
			//create one. 
			Highlight = Instantiate(HighlightArchetype, pos, Quaternion.identity) as GameObject;
			HighlightController getAG = Highlight.GetComponent("HighlightController") as HighlightController;
			Action.Property(getAG.Grp, Highlight.transform.GetProperty(o => o.position), pos, this.EaseSpeed, Ease.SinOut);
		}
		
		//otherwise, If there are no objects in the array and the highlight object exists,
		else if(AllInteractableObjects.Count <= 0 && Highlight != null)
		{
			Destroy(Highlight);
		}
		//otherwise, interpolate the highlight to it's proper position. 
		else if(AllInteractableObjects.Count > 0)
		{
			Vector3 pos = closestobj.transform.position;
			//pos += new Vector3(0, HeightOfHighlight, 0);
			print (closestobj.name + " is king of team");

			// Remake the Highlight, because action system ate the ability to move it
			Destroy(Highlight);
			Highlight = Instantiate(HighlightArchetype, pos, Quaternion.identity) as GameObject;

			// Use Josh's Action replacement system here
			HighlightController getAG = Highlight.GetComponent("HighlightController") as HighlightController;
			Action.Property(getAG.Grp, Highlight.transform.GetProperty(o => o.position), pos, this.EaseSpeed, Ease.SinOut);
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
		
		//remove all of the null objects
		AllInteractableObjects.RemoveAll(GameObject => GameObject == null);
	}

	public void OnInteractEvent()
	{
		//find the nearest object
		GameObject closest = FindClosestObjectToPlayer();
		
		//If the currently interacted object is not the closest object anymore,
		if(CurrentlyInteractedObject != closest && CurrentlyInteractedObject != null)
		{
			//then we can tell it to go away
			Interactable deInteract = CurrentlyInteractedObject.GetComponent("Interactable") as Interactable;
			deInteract.SetIsInInteraction(false);
		}
		
		//Bring the actual closest object in
		
		if(closest != null)
		{
			//Create a ScriptEvent
			Interactable inter = closest.GetComponent("Interactable") as Interactable;
			inter.SetIsInInteraction(true);
		}
		
		//then make the last dispatched object the new currently interacted
		CurrentlyInteractedObject = closest;
	}

	GameObject FindClosestObjectToPlayer()
	{
		GameObject closest = null;
		float bestDist = float.MaxValue;

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
		if(closest != null){print(closest.name);}
		return closest;
	}
	
	public void AddInteractableObject(GameObject cog)
	{
		AllInteractableObjects.Add(cog);
		print("Added Object!");
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
				print("Removed Object!");
				return;
			}
		}
	}
}