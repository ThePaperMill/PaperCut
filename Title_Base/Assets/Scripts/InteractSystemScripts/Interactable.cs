/*********************************
 * Interactable.cs
 * Troy
 * Created 9/11/2015
 *
 * Attach to objects that can be interacted with by the player!
 *
 * Copyright © 2015 DigiPen Institute of Technology, All Rights Reserved
 *********************************/

using UnityEngine;
using System.Collections;

public class Interactable : MonoBehaviour
{
	public GameObject LevelSettings;
	public GameObject InteractCollider;

    GameObject childRigid = null;

	//InteractSizeScalar is how big the spherical trigger of the collidable detection object is.
	public float InteractSizeScalar = 1.5f;
	
	bool isInInteraction = false;
	
	void Start()
	{
        // If the devs were too lazy to give us the InteractCollider script, get it
        if (!InteractCollider)
        {
            InteractCollider = Resources.Load("InteractCollider", typeof(GameObject)) as GameObject;
        }

        // If the user didn't set the level settings, find it
        if (!LevelSettings)
        {
            LevelSettings = GameObject.FindGameObjectWithTag("LevelSettings");
        }
	
        //If the levelsettings does not have an interactable manager, add one (use to print an error message, disabled that for now).
	    InteractManager initial = LevelSettings.GetComponent("InteractManager") as InteractManager;
    
        gameObject.Connect(Events.Interact, OnInteractEvent);
		
        if(initial == null)
        {
            //print("ERROR: LEVELSETTINGS DOES NOT HAVE INTERACTMANAGER COMPONENT. Adding to component list");
            LevelSettings.AddComponent<InteractManager>();
		}

        //Create a new interactable ghost collider for this object at it's position. if the player touches this, then this object can be interacted with
        if (InteractCollider)
        {
            childRigid = Instantiate(InteractCollider, gameObject.transform.position, Quaternion.identity) as GameObject;
            childRigid.transform.parent = gameObject.transform;
        }
        // resouce load it if we don't have it set.
        else
        {
            childRigid = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("InteractCollider"));
        }
    
        //Increase the size of the bounding box based on property interactablesize scalar.
		SphereCollider childSphere = childRigid.GetComponent("SphereCollider") as SphereCollider;
		childSphere.radius *= InteractSizeScalar;
		
        //Tell the new rigidbody to keep track of this object
		InteractRigid par = childRigid.GetComponent("InteractRigid") as InteractRigid;
		par.SetParent(gameObject);
	}

    public void OnInteractEvent(EventData eventData)
    {
        gameObject.DispatchEvent(Events.EngageConversation);
        gameObject.DispatchEvent(Events.InteractedWith);
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

    void OnDestroy()
    {
        gameObject.Disconnect(Events.Interact, OnInteractEvent);

        if (childRigid)
        {
            Destroy(childRigid);
        }
    }
}