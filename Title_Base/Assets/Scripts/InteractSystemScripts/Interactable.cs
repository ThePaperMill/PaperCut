/*********************************
 * Interactable.cs
 * Troy
 * Created 9/11/2015
 * Copyright © 2015 DigiPen Institute of Technology, All Rights Reserved
 *********************************/

using UnityEngine;
using System.Collections;

//Interactable ~ attached to objects that can be interacted with by the player?

public class Interactable : MonoBehaviour
{
	public GameObject LevelSettings;
	public GameObject InteractCollider;

	//InteractSizeScalar is how big the bounding box of the collidable detection object is.
	public float InteractSizeScalar = 1.5f;
	
	bool isInInteraction = false;
	
	void Start()
	{
    LevelSettings = GameObject.FindGameObjectWithTag("LevelSettings");

		//If the levelsettings does not have an interactable manager, add one and print an error message.
		InteractManager initial = LevelSettings.GetComponent("InteractManager") as InteractManager;
    
    this.gameObject.Connect(Events.Interact, OnInteractEvent);
		
    if(initial == null)
		{
			LevelSettings.AddComponent<InteractManager>();
			print("ERROR: LEVELSETTINGS DOES NOT HAVE INTERACTMANAGER COMPONENT. Adding to component list");
		}
		
		//Create a new interactable ghost collider for this object at it's position. if the player touches this, then this object can be interacted with
		GameObject childRigid = Instantiate(InteractCollider, gameObject.transform.position, Quaternion.identity) as GameObject;
		//Increase the size of the bounding box based on property interactablesize scalar.
		SphereCollider childSphere = childRigid.GetComponent("SphereCollider") as SphereCollider;
		childSphere.radius *= this.InteractSizeScalar;
		//Tell the new rigidbody to keep track of this object
		InteractRigid par = childRigid.GetComponent("InteractRigid") as InteractRigid;
		par.SetParent(gameObject);
	}

      public void OnInteractEvent(EventData eventData)
      {
        this.gameObject.DispatchEvent(Events.EngageConversation);
      }

	public bool GetIsInInteraction()
	{
		return isInInteraction;
	}
	public void SetIsInInteraction(bool toSet)
	{
		isInInteraction = toSet;

		// If the player presses the action button, send an interaction event
		if (toSet == true)
		{
			InteractManager sendAvent = LevelSettings.GetComponent("InteractManager") as InteractManager;
			sendAvent.OnInteractEvent();
		}
	}
}