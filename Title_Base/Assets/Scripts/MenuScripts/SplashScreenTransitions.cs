using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SplashScreenTransitions : MonoBehaviour 
{
    public List<GameObject> Screens = new List<GameObject>();

    float ScreenDelay = 3.0f;
    float ScreenTimer = 0.0f;
    int CurrentScreen = 0;
    bool complete = false;
    bool Faded = false;

	// Use this for initialization
    void Start () 
    {
        InputManager.GetSingleton.Initialize();

        if (Screens.Count > 0)
        {
            foreach (var i in Screens)
            i.SetActive(false);

            Screens[CurrentScreen].SetActive(true);
        }
    }
	
	// Update is called once per frame
    void Update () 
    {
        if (InputManager.GetSingleton.IsKeyTriggered(KeyCode.Escape))
        {
            LevelTransitionManager.GetSingleton.ChangeLevel("MainMenu", true, 1.0f);
            complete = true;
        }



        if (complete == true)
            return;

    ScreenTimer += Time.deltaTime;

    if (ScreenTimer > ScreenDelay - 0.5f && Faded == false && CurrentScreen < Screens.Count - 1)
    {
      Faded = true;
      LevelTransitionManager.GetSingleton.FadeOutIn(0.5f);
    }

    if(ScreenTimer > ScreenDelay)
    {
      int prevScreen = CurrentScreen;
      CurrentScreen++;
      ScreenTimer = 0.0f;
      Faded = false;

      if (CurrentScreen >= Screens.Count)
      {
        complete = true;
        LevelTransitionManager.GetSingleton.ChangeLevel("MainMenu", true, 1.5f);
        return;
      }
      else
      {
        Screens[prevScreen].SetActive(false);
        Screens[CurrentScreen].SetActive(true);
      }
    }
  }
}
