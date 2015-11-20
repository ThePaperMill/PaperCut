using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class SplashScreenLogic : MonoBehaviour 
{
  public string LevelToLoad = "";

  public List<Sprite> Sprites = new List<Sprite>();

  public float ScreenTime = 3.0f;

  float TransitionTimer = 0.0f;

  int CurImage = 0;

  Image SplashImage = null;

	// Use this for initialization
	void Start () 
  {
    SplashImage = GetComponent<Image>();

	}
	
	// Update is called once per frame
  void Update () 
  {
    TransitionTimer += Time.deltaTime;

    if (TransitionTimer > ScreenTime)
    {
      ++CurImage;
      TransitionTimer = 0.0f;

      if (CurImage >= Sprites.Count)
      {
        // load level 
        Application.LoadLevel(LevelToLoad);
      }
      else
      {
        SplashImage.overrideSprite = Sprites[CurImage];
      }
    }
	}
}
