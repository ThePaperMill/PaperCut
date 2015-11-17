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
      IsPaused = true;
      //Time.timeScale = 0.0f;
  }

  public void ResumeGame()
  {
      IsPaused = false;
      //Time.timeScale = 1.0f;
  }

	// Update is called once per frame
	void Update ()
    {
	    if(InputManager.GetSingleton.IsKeyTriggered(KeyCode.Escape))
        {
            if (IsPaused == false)
                PauseGame();
            else
                ResumeGame();
        }
	}
}
