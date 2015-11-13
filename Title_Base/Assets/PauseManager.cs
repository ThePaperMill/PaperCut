using UnityEngine;
using System.Collections;

public class PauseManager : Singleton<PauseManager>
{
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
        Time.timeScale = 0.0f;
    }

    public void ResumeGame()
    {
        IsPaused = false;
        Time.timeScale = 1.0f;
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
