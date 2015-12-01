/****************************************************************************/
/*!
\file   InitiateQuitButton.cs
\author Steven Gallwas 
\brief  
    Imitates the quit sequence.
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
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