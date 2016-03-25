using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DispatchEventButton : MenuButton
{
  public List<string> DispatchEvents = new List<string>();
	
  void Start () 
  {
	
	}
	

	void Update () 
  {
	
	}

  public override void Activate()
  {
    foreach (var Event in DispatchEvents)
    {
      EventSystem.GlobalHandler.DispatchEvent(Event);
    }
  }
}
