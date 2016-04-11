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
    string FifthLevel = "Sewer";
    string SixthLevel = "WaterWorks";
    string SeventhLevel = "RachelsStreet";
    string EighthLevel = "Jerry_RachelHouse";

    string NinthLevel = "DuckPondAlternate";
    string LastLevel = "DuckPondAlternate";

    GameObject Item1 = null;
    GameObject Item2 = null;
    ItemInfo ItemToGive = null;
    ItemInfo ItemToGive2 = null;

    public bool cheatUsed = false;

    // Use this for initialization
    public void Initialize()
    {

    }

    // Use this for initialization
    void Start ()
    { 

	}
	
    void GiveItems()
    {
        if(cheatUsed)
        {
            return;
        }

        cheatUsed = true;

        Item1 = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("FinalObject1"));
        Item2 = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("FinalObject2"));

        ItemToGive = Item1.GetComponent<CollectableObject>().ItemToGive;
        ItemToGive2 = Item2.GetComponent<CollectableObject>().ItemToGive;

        // we have to init the item we want to give because of unity 
        ItemToGive.InitializeItem();
        ItemToGive2.InitializeItem();

        RecievedItemEvent test = new RecievedItemEvent(ItemToGive);
        EventSystem.GlobalHandler.DispatchEvent(Events.RecievedItem, test);

        RecievedItemEvent test2 = new RecievedItemEvent(ItemToGive2);
        EventSystem.GlobalHandler.DispatchEvent(Events.RecievedItem, test2);

        GameObject.Destroy(Item1);
        GameObject.Destroy(Item2);
    }

	// Update is called once per frame
	void Update ()
    {
        if(InputManager.GetSingleton.IsKeyTriggered(KeyCode.Return))
        {
            GiveItems();
        }

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
