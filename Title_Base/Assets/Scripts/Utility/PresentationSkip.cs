using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PresentationSkip : Singleton<PresentationSkip>
{
    string FirstLevel = "Player_House";
    string SecondLevel = "PlayerStreet";
    string ThirdLevel  = "CityCenter";
    string FourthLevel = "ScientistStreet";

    // mechanics test
    string FifthLevel = "ScientistLab";
    string SixthLevel = "Sewer";
    string SeventhLevel = "WaterWorks";
    string EighthLevel = "Jerry's_Level1";

    string NinthLevel = "Jerry's_Level1";
    string LastLevel = "Jerry's_Level1";

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
            LevelTransitionManager.GetSingleton.ChangeLevel(FirstLevel, true, 1.0f);
            //SceneManager.LoadScene(FirstLevel);
        }
        else if (InputManager.GetSingleton.IsKeyTriggered(KeyCode.Alpha2))
        {
            LevelTransitionManager.GetSingleton.ChangeLevel(SecondLevel, true, 1.0f);
            //SceneManager.LoadScene(SecondLevel);
        }
        else if (InputManager.GetSingleton.IsKeyTriggered(KeyCode.Alpha3))
        {
            LevelTransitionManager.GetSingleton.ChangeLevel(ThirdLevel, true, 1.0f);
        }
        else if (InputManager.GetSingleton.IsKeyTriggered(KeyCode.Alpha4))
        {
            LevelTransitionManager.GetSingleton.ChangeLevel(FourthLevel, true, 1.0f);
        }
        else if (InputManager.GetSingleton.IsKeyTriggered(KeyCode.Alpha5))
        {
            LevelTransitionManager.GetSingleton.ChangeLevel(FifthLevel, true, 1.0f);
        }
        else if (InputManager.GetSingleton.IsKeyTriggered(KeyCode.Alpha6))
        {
            LevelTransitionManager.GetSingleton.ChangeLevel(SixthLevel, true, 1.0f);
        }
        else if (InputManager.GetSingleton.IsKeyTriggered(KeyCode.Alpha7))
        {
            LevelTransitionManager.GetSingleton.ChangeLevel(SeventhLevel, true, 1.0f);
        }
        else if (InputManager.GetSingleton.IsKeyTriggered(KeyCode.Alpha8))
        {
            LevelTransitionManager.GetSingleton.ChangeLevel(EighthLevel, true, 1.0f);
        }
        else if (InputManager.GetSingleton.IsKeyTriggered(KeyCode.Alpha9))
        {
            LevelTransitionManager.GetSingleton.ChangeLevel(NinthLevel, true, 1.0f);
        }
        else if (InputManager.GetSingleton.IsKeyTriggered(KeyCode.Alpha0))
        {
            LevelTransitionManager.GetSingleton.ChangeLevel(LastLevel, true, 1.0f);
        }
    }
}
