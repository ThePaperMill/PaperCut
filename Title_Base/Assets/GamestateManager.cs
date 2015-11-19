/****************************************************************************/
/*!
\file   GamestateManager.cs
\author Steven Gallwas
\brief  
       The gamestate manager to handle pause and cutscenes
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;

public enum GAME_STATE
{
  GS_PAUSE,
  GS_CINEMATIC,
  GS_GAME,
}

public class GamestateManager : Singleton<GamestateManager>
{
  public GAME_STATE CurState = GAME_STATE.GS_GAME;


  public bool IsPaused = false;
  
	// Use this for initialization
	void Start ()
  {
	
  }

  void OnLevelWasLoaded()
  {
      ResumeGame();
  }
	
  public void PauseGame()
  {
      print("pausing the game");

      EventSystem.GlobalHandler.DispatchEvent(Events.PauseGameEvent, null);
      IsPaused = true;
      Time.timeScale = 0.0f;
  }

  public void ResumeGame()
  {
      EventSystem.GlobalHandler.DispatchEvent(Events.ResumeGameEvent, null);
      IsPaused = false;
      Time.timeScale = 1.0f;
  }

  public void Initialize()
  {

  }

	// Update is called once per frame
	void Update ()
  {
	  if(InputManager.GetSingleton.IsKeyTriggered(KeyCode.Escape) || InputManager.GetSingleton.IsButtonTriggered(XINPUT_BUTTONS.BUTTON_START))
    {
      if (IsPaused == false)
          PauseGame();
    }
	}
}
