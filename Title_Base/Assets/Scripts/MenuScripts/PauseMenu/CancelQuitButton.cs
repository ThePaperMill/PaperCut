using UnityEngine;
using System.Collections;

public class CancelQuitButton : MenuButton
{
  public override void Activate()
  {
    EventSystem.GlobalHandler.DispatchEvent(Events.CancelQuitEvent);
  }

  // Update is called once per frame
  void Update()
  {

  }
}