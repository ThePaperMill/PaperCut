using UnityEngine;
using System.Collections;

public class TransformMachine : EventHandler
{

	// Use this for initialization
	void Start () 
  {
    EventSystem.GlobalHandler.Connect(Events.TransformItem, OnTransformItem);
	}

  void OnTransformItem(EventData eventData)
  {
    // cast as a recieved item event
    var data = eventData as RecievedItemEvent;
    
    //if the item is null
    if (data.Info.ItemName == "")
    {

    }
  }

	// Update is called once per frame
	void Update () 
  {
	
	}
}
