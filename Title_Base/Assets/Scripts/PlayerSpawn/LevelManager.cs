/****************************************************************************/
/*!
\file   LevelManager.cs
\author Steven Gallwas
\brief  
    This file contains the implementation of the level manager
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;

public class LevelManager : Singleton<LevelManager>
{
  public string PrevLevel = "";

  public string CurLevel = "";

  void OnLevelWasLoaded(int level)
  {
    // we don't want previous and current to be the same, this will only happen with reloads
    if(Application.loadedLevelName == CurLevel)
    {
      return;
    }

    // store the previous level and get the new level
    PrevLevel = CurLevel;
    CurLevel = Application.loadedLevelName;
  }

	// Use this for initialization
	void Start () 
  {
	
	}
	
	// Update is called once per frame
	void Update () 
  {
	
	}
}
