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
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelChangeButton : MenuButton
{
  public string Level = "";

  public override void Activate()
  {
    LevelTransitionManager.GetSingleton.ChangeLevel(Level, true, 1.5f);
    //SceneManager.LoadScene(Level);
  }
	
	// Update is called once per frame
	void Update () 
  {
	
	}
}
