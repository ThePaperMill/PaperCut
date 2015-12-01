/****************************************************************************/
/*!
\file   ToggleFullscreenButton
\author steven Gallwas
\brief  
    Toggles Fullscreen.
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/

using UnityEngine;
using System.Collections;

public class ToggleFullscreenButton : MenuButton
{
  public override void Activate()
  {
    Screen.fullScreen = !Screen.fullScreen; 
  }

  // Update is called once per frame
  void Update()
  {

  }
}
