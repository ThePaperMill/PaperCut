/****************************************************************************/
/*!
\file   CollectableObject.cs
\author Steven Gallwas
\brief  
    Logic for an object that can be picked up by the player.
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class CollectableObject :MonoBehaviour
{
    public ItemInfo ItemToGive = new ItemInfo();

    void Start()
    {
        this.gameObject.Connect(Events.InteractedWith, OnInteractedWith);
    }

    void OnInteractedWith(EventData data)
    {
        // we have to init the item we want to give because of unity 
        ItemToGive.InitializeItem();

        RecievedItemEvent test = new RecievedItemEvent(ItemToGive);
        EventSystem.GlobalHandler.DispatchEvent(Events.RecievedItem, test);

        //transform.parent
        if(transform.parent && transform.parent.gameObject != null)
          GameObject.Destroy(transform.parent.gameObject);
        
        GameObject.Destroy(gameObject);
    }
}


