using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PresentationSkip : Singleton<PresentationSkip>
{
    string FirstLevel = "Player_House";
    string SecondLevel = "Jerry's_PrettyLevel";




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
            SceneManager.LoadScene(FirstLevel);
        }
        else if (InputManager.GetSingleton.IsKeyTriggered(KeyCode.Alpha3))
        {
            SceneManager.LoadScene(FirstLevel);
        }
        else if (InputManager.GetSingleton.IsKeyTriggered(KeyCode.Alpha4))
        {
            SceneManager.LoadScene(FirstLevel);
        }
        else if (InputManager.GetSingleton.IsKeyTriggered(KeyCode.Alpha5))
        {
            SceneManager.LoadScene(FirstLevel);
        }
        else if (InputManager.GetSingleton.IsKeyTriggered(KeyCode.Alpha6))
        {
            SceneManager.LoadScene(FirstLevel);
        }
        else if (InputManager.GetSingleton.IsKeyTriggered(KeyCode.Alpha7))
        {
            SceneManager.LoadScene(FirstLevel);
        }
        else if (InputManager.GetSingleton.IsKeyTriggered(KeyCode.Alpha8))
        {
            SceneManager.LoadScene(FirstLevel);
        }
        else if (InputManager.GetSingleton.IsKeyTriggered(KeyCode.Alpha9))
        {
            SceneManager.LoadScene(FirstLevel);
        }
        else if (InputManager.GetSingleton.IsKeyTriggered(KeyCode.Alpha0))
        {
            SceneManager.LoadScene(LastLevel);
        }
    }
}
