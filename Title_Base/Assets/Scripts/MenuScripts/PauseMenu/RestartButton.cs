/****************************************************************************/
/*!
\file  RestartButton.cs
\author Steven Gallwas 
\brief  
 
  
    © 2016 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;

public class RestartButton : MenuButton
{
    public string StartingLevel = "";

    public override void Activate()
    {
        LevelTransitionManager.GetSingleton.ChangeLevel(StartingLevel, true, 1.5f);
        GameInfo.GetSingleton.ResetBools();
        InventorySystem.GetSingleton.ClearInventory();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
