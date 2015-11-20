using UnityEngine;
using System.Collections;

public class InitiateQuitButton : MenuButton
{
  public override void Activate()
  {
    EventSystem.GlobalHandler.DispatchEvent(Events.InitiateQuitEvent);
  }

  // Update is called once per frame
  void Update()
  {

  }
}