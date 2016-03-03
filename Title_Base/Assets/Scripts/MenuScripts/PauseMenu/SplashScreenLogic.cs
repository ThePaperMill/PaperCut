/****************************************************************************/
/*!
\file   SplashScreenLogic.cs
\author Steven Gallwas
\brief  
    Controls the transitions between the splash screens.
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SplashScreenLogic : MonoBehaviour 
{
  public string LevelToLoad = "";

  public List<Sprite> Sprites = new List<Sprite>();

  public float ScreenTime = 3.0f;

  float TransitionTimer = 0.0f;

  int CurImage = 0;

  Image SplashImage = null;

    bool finished = false;

	// Use this for initialization
    void Start () 
    {
        SplashImage = GetComponent<Image>();
        Cursor.visible = false;
    }
	
	// Update is called once per frame
  void Update () 
  {

    if (finished == true)
    {
        return;
    }

    if (InputManager.GetSingleton.IsKeyTriggered(KeyCode.Escape))
    {
        LevelTransitionManager.GetSingleton.ChangeLevel(LevelToLoad, true, 1.0f);
        finished = true;
    }

    TransitionTimer += Time.deltaTime;

    if (TransitionTimer > ScreenTime)
    {
      ++CurImage;
      TransitionTimer = 0.0f;

      if (CurImage >= Sprites.Count)
      {
        // load level 
        LevelTransitionManager.GetSingleton.ChangeLevel(LevelToLoad, true, 1.0f);
        finished = true;
      }
      else
      {
        SplashImage.overrideSprite = Sprites[CurImage];
      }
    }
	}
}
