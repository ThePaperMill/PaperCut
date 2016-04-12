/****************************************************************************/
/*!
\file  LoadSceneOnButtonPress.cs
\author Ian Aemmer 
\brief  
 
  
    © 2016 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


public class LoadSceneOnButtonPress : MonoBehaviour {
    public string SceneName;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        //var LeftStickPosition = InputManager.GetSingleton.GetLeftStickValues();
        bool JumpHeld = InputManager.GetSingleton.IsInputDown(GlobalControls.JumpKeys);
        bool startheld = InputManager.GetSingleton.IsButtonTriggered(XINPUT_BUTTONS.BUTTON_START);
        bool selectheld = InputManager.GetSingleton.IsButtonTriggered(XINPUT_BUTTONS.BUTTON_BACK);

        bool jump = InputManager.GetSingleton.IsKeyDown(KeyCode.E);
        bool space = InputManager.GetSingleton.IsKeyDown(KeyCode.Space);
        bool enter = InputManager.GetSingleton.IsKeyDown(KeyCode.End);


        if ( JumpHeld || jump || startheld || selectheld || space || enter)
        {
            LoadSCene();
        }

    }

    void LoadSCene()
    {
        //LoadSceneMode.)
        SceneManager.LoadScene(SceneName);
    }
}
