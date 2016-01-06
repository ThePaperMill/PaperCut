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
using UnityEngine.SceneManagement;
using System.Collections;

public class LevelManager : Singleton<LevelManager>
{
  public string PrevLevel = "DefaultPrev";

  public string CurLevel = "DefaultCur";

  void OnLevelWasLoaded(int level)
  {
    // we don't want previous and current to be the same, this will only happen with reloads
    if(SceneManager.GetActiveScene().name == CurLevel)
    {
      return;
    }

    // store the previous level and get the new level
    PrevLevel = CurLevel;
    CurLevel = SceneManager.GetActiveScene().name;
  }

	// Use this for initialization
	void Start () 
  {
	
	}
	
	// Update is called once per frame
	void Update () 
  {
    CurLevel = SceneManager.GetActiveScene().name;
	}
}
