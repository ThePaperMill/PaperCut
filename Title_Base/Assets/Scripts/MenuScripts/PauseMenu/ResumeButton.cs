/****************************************************************************/
/*!
\file   ResumeButton.cs 
\author Steven Gallwas
\brief  
    Resumes the game.
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
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