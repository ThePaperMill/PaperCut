/****************************************************************************/
/*!
\file  GameInfo.cs
\author Steven Gallwas 
\brief  
 
  
    © 2016 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameInfo : Singleton<GameInfo>
{
   public Material PlayerColor = null;
    public bool LabDestroyed = false;
    public bool FinaleReady = false;

    // Use this for initialization
    void Start ()
    {
	
	}

    public void Initialize()
    {

    }

    void OnLevelWasLoaded(int level)
    {
        if (SceneManager.GetActiveScene().name == "MainMenu")
        {
            //print("Resetting Game");
            ResetBools();
            InventorySystem.GetSingleton.ClearInventory();
        }

    }

    public void TriggerBools()
    {
        PresentationSkip.GetSingleton.cheatUsed = false;
        LabDestroyed = false;
        FinaleReady = false;
    }

    public void ResetBools()
    {
        PresentationSkip.GetSingleton.cheatUsed = false;
        LabDestroyed = false;
        FinaleReady = false;
    }

	// Update is called once per frame
	void Update ()
    {
	}
}
