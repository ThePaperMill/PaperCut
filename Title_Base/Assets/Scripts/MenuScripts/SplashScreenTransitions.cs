/****************************************************************************/
/*!
\file  SplashScreenTransitions.cs
\author Steven Gallwas 
\brief  
 
  
    © 2016 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class SplashScreenTransitions : MonoBehaviour 
{
    public List<GameObject> Screens = new List<GameObject>();

    float ScreenDelay = 3.0f;
    float ScreenTimer = 0.0f;
    int CurrentScreen = 0;
    bool complete = false;
	bool Faded = false;
	bool Unfocused = false;

	// Use this for initialization
    void Start () 
    {
        InputManager.GetSingleton.Initialize();

        Cursor.visible = false;

        #if UNITY_EDITOR
        Cursor.visible = true;
        #endif



        if (Screens.Count > 0)
        {
            foreach (var i in Screens)
            i.SetActive(false);

            Screens[CurrentScreen].SetActive(true);
        }
    }

	void OnApplicationFocus(bool focusStatus)
	{
		Unfocused = !(focusStatus);
		ScreenTimer += Time.deltaTime;
	}
	
	// Update is called once per frame
    void Update () 
    {
        if (InputManager.GetSingleton.IsKeyTriggered(KeyCode.Escape) && complete == false)
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

		//print(ScreenTimer);

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
