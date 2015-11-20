using UnityEngine;
using System.Collections;

public class ResumeButton : MenuButton
{
  public override void Activate()
  {
    GamestateManager.GetSingleton.ResumeGame();
  }

  // Update is called once per frame
  void Update()
  {

  }
}