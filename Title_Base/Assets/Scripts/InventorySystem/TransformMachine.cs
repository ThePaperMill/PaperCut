/****************************************************************************/
/*!
\file    TransformMachine.cs
\author Steven Gallwas
\brief  
       The machine that transforms objects.
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;
using ActionSystem;

public class TransformMachine : EventHandler
{
  public GameObject LightningBoltPrefab = null;
  private ActionGroup grp = new ActionGroup();

	// Use this for initialization
	void Start () 
  {
    EventSystem.GlobalHandler.Connect(Events.TransformItem, OnTransformItem);
	}

  void OnTransformItem(EventData eventData)
  {
    // cast as a recieved item event
    var data = eventData as RecievedItemEvent;
    
    //if the item is cardboard, create the real version
    if (data.Info.CurStatus == ITEM_STATUS.IS_CARDBOARD)
    {

    }
    // if the item is real, create the cardboard version.
    else
    {

    }
  }

	// Update is called once per frame
	void Update () 
  {
    grp.Update(Time.deltaTime);
	}
}