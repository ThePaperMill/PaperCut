/****************************************************************************/
/*!
\file   LevelChangeButton.cs
\author Steven Gallwas
\brief  
    Changes levels when activated.
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/

using UnityEngine;
using System.Collections;

public class LevelChangeButton : MenuButton
{
  public string Level = "";

  public override void Activate()
  {
    Application.LoadLevel(Level);
  }
	
	// Update is called once per frame
	void Update () 
  {
	
	}
}
