/****************************************************************************/
/*!
\file   CreditsScroll.cs
\author Steven Gallwas
\brief  
    Makes the credits scroll.
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class CreditsScroll : MonoBehaviour 
{
  public float ScrollSpeed = 25.0f;

  float Timer = 0.0f;

  public float Delay = 30.0f;

    bool escape = false;

    bool CreditsComplete = false;

	// Use this for initialization
	void Start () 
  {
	
	}
	
	// Update is called once per frame
  void Update () 
  {
    if (CreditsComplete)
        return;

        UpdateInput();

    if (Timer > Delay || escape)
    {
        CreditsComplete = true;
        LevelTransitionManager.GetSingleton.ChangeLevel("MainMenu");
        //SceneManager.LoadScene("MainMenu");
    }

    Timer += Time.deltaTime;

      transform.position += Vector3.up * ScrollSpeed * Time.deltaTime;
  }

    void UpdateInput()
    {
        escape = InputManager.GetSingleton.IsButtonTriggered(XINPUT_BUTTONS.BUTTON_START) || InputManager.GetSingleton.IsKeyTriggered(KeyCode.Escape);
    }
}
