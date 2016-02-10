﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PresentationSkip : Singleton<PresentationSkip>
{
    string FirstLevel = "Player_House";

    string SecondLevel = "Jerry's_PrettyLevel";
    string ThirdLevel = "MilestonePretest";
    string FourthLevel = "HighRiseApartments";

    // mechanics test
    string FifthLevel = "Sewer";

    string SixthLevel = "NewCityCenter";
    string SeventhLevel = "HighRiseApartments";
    string EighthLevel = "HighRiseApartments";



    string NinthLevel = "CityCenter";
    string LastLevel = "ScientistStreet";

    // Use this for initialization
    public void Initialize()
    {

    }

    // Use this for initialization
    void Start ()
    { 

	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(InputManager.GetSingleton.IsKeyTriggered(KeyCode.Alpha1))
        {
            SceneManager.LoadScene(FirstLevel);
        }
        else if (InputManager.GetSingleton.IsKeyTriggered(KeyCode.Alpha2))
        {
            SceneManager.LoadScene(SecondLevel);
        }
        else if (InputManager.GetSingleton.IsKeyTriggered(KeyCode.Alpha3))
        {
            SceneManager.LoadScene(ThirdLevel);
        }
        else if (InputManager.GetSingleton.IsKeyTriggered(KeyCode.Alpha4))
        {
            SceneManager.LoadScene(FourthLevel);
        }
        else if (InputManager.GetSingleton.IsKeyTriggered(KeyCode.Alpha5))
        {
            SceneManager.LoadScene(FifthLevel);
        }
        else if (InputManager.GetSingleton.IsKeyTriggered(KeyCode.Alpha6))
        {
            SceneManager.LoadScene(SixthLevel);
        }
        else if (InputManager.GetSingleton.IsKeyTriggered(KeyCode.Alpha7))
        {
            SceneManager.LoadScene(SeventhLevel);
        }
        else if (InputManager.GetSingleton.IsKeyTriggered(KeyCode.Alpha8))
        {
            SceneManager.LoadScene(EighthLevel);
        }
        else if (InputManager.GetSingleton.IsKeyTriggered(KeyCode.Alpha9))
        {
            SceneManager.LoadScene(NinthLevel);
        }
        else if (InputManager.GetSingleton.IsKeyTriggered(KeyCode.Alpha0))
        {
            SceneManager.LoadScene(LastLevel);
        }
    }
}
