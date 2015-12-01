/****************************************************************************/
/*!
\file   Menu_QuitButton.cs
\author Steven Gallwas 
\brief  
    initiates the quit sequence
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/

using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class Menu_QuitButton : MenuButton
{
  public override void Activate()
  {
   #if UNITY_EDITOR
    EditorApplication.isPlaying = false;
#endif


     GamestateManager.GetSingleton.AllowQuit = true;
    Application.Quit();
  }

	// Update is called once per frame
	void Update () 
  {
	
	}
}
