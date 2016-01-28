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
    public string PlayerName = "DynamicPlayer(Clone)";
	public GameObject LevelSettings;
	bool delay = true;
	bool secondition = false;
	List<Collider> past = new List<Collider>{};

	//Declare a parent object to keep track of. 
	public GameObject Parent;

    private bool retry = false;
    private bool inCol = false;
    private GameObject player;
    private InteractManager toAdd;

    void Start()
	{
		//Store the player object
		LevelSettings = GameObject.Find("LevelSettings");
        toAdd = LevelSettings.GetComponent("InteractManager") as InteractManager;
    }


	void Update()
    {
        //If the parent object exists, move to it's position every frame
        if (this.Parent != null && !delay)
		{
			gameObject.transform.position = Parent.transform.position;
		}

		// Give the game a frame to load before interacting
		if(delay)
		{
			delay = false;
		}

        // Kill the list if the player starts hovering
        if(player && player.GetComponent<HoverSpin>().IsSpinning() && !retry)
        {
            HoverExit();
        }

        //print("SPC1:  " + (player && !player.GetComponent<HoverSpin>().IsSpinning() && inCol && !toAdd.AllInteractableObjects.Find(predishit => predishit == Parent)));

        // Special Case:  We're in an interactable collision, not hovering, and there's no list
        if(player && !player.GetComponent<HoverSpin>().IsSpinning() && inCol && !toAdd.AllInteractableObjects.Find(predishit => predishit == Parent))
        {
            OnTriggerEnter(player.GetComponent<Collider>());
        }

        // Special Case:  Starting out in an interactable collision
        else if (secondition && past != null)
		{
			secondition = false;
            SecondFrameResponse();
        }

        // Special Case:  Waiting for the player to stop hovering
        else if (retry)
        {
            retry = false;
            RetryResponse();
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
            print("SFR!");

            // If the object colliding is the player, then add the parent object to the interact manager's array of currently colliding objects. 
            if ((collision.gameObject.name == PlayerName && !delay))
			{
                inCol = true;

                // If currently spinning, retry later
                if(collision.gameObject.GetComponent<HoverSpin>() && collision.gameObject.GetComponent<HoverSpin>().IsSpinning())
                {
                    retry = true;
                    player = collision.gameObject;
                }

                else if (this.LevelSettings != null)
				{
					InteractManager toAdd = LevelSettings.GetComponent("InteractManager") as InteractManager;

					if (toAdd == null) {print ("Turns out you can't interact with something that doesn't exist.");}

					toAdd.AddInteractableObject(Parent);
				}
			}
		};
    }

    void RetryResponse()
    {
        // If the object colliding is the player, then add the parent object to the interact manager's array of currently colliding objects. 
        if (inCol)
        {
            inCol = true;

            // If currently spinning, retry later
            if (player.GetComponent<HoverSpin>() && player.GetComponent<HoverSpin>().IsSpinning())
            {
                retry = true;
            }

            else if (this.LevelSettings != null)
            {
                InteractManager toAdd = LevelSettings.GetComponent("InteractManager") as InteractManager;

                if (toAdd == null) { print("Turns out you can't interact with something that doesn't exist."); }

                toAdd.AddInteractableObject(Parent);
            }
        }
    }

    void OnTriggerEnter(Collider collision)
    {
        // If the object colliding is the player, then add the parent object to the interact manager's array of currently colliding objects.
        if ((collision.gameObject.name == PlayerName && !delay))
        {
            inCol = true;

            // If currently spinning, retry later
            if (collision.gameObject.GetComponent<HoverSpin>() && collision.gameObject.GetComponent<HoverSpin>().IsSpinning())
            {
                retry = true;
                player = collision.gameObject;
            }

            else if(this.LevelSettings != null)
			{
				InteractManager toAdd = LevelSettings.GetComponent("InteractManager") as InteractManager;

				if (toAdd == null) {print ("Turns out you can't interact with something that doesn't exist.");}

				toAdd.AddInteractableObject(Parent);
			}
		}
		
		// Add to the list next frame if this is the first frame
		else if(collision.gameObject.GetComponent<CustomDynamicController>() != null && delay)
		{
			secondition = true;
			past.Add(collision);
		}
	}
	
	void OnTriggerExit(Collider collision)
	{
        inCol = false;

        // If the object no longer colliding is the player, then remove the parent object from the interact mnager's array of currently colliding objects.
        if (collision.gameObject.GetComponent<CustomDynamicController>() != null && !delay)
		{
			if(this.LevelSettings != null)
			{
				toAdd.RemoveInteractableObject(Parent);
			}
		}
        // And send a "Disinteract event" to this object's owner (Never Done?) 

    }

    void HoverExit()
    {
        // If the object no longer colliding is the player, then remove the parent object from the interact mnager's array of currently colliding objects.
        if (player.GetComponent<CustomDynamicController>() != null && !delay)
        {
            if (this.LevelSettings != null)
            {
                toAdd.RemoveInteractableObject(Parent);
            }
        }
        // And send a "Disinteract event" to this object's owner (Never Done?) 

    }
}