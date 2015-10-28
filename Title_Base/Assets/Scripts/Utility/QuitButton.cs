/****************************************************************************/
/*!
\file   QuitButton.cs
\author Steven Gallwas
\brief  
    This file contains the implementation for a simple quit button
 
  © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;

public class QuitButton : MonoBehaviour 
{

	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKey(KeyCode.Escape))
        {
            Application.Quit();
        }

        else if(Input.GetKey(KeyCode.R) || InputManager.GetSingleton.IsButtonTriggered(XINPUT_BUTTONS.BUTTON_BACK))
        {
            Application.LoadLevel(Application.loadedLevel);
        }
    }
}
