/****************************************************************************/
/*!
\file  LevelTransitionManager.cs
\author Steven Gallwas 
\brief  
 
  
    © 2016 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public enum FadeState
{
  FS_FADE_IN,
  FS_FADE_OUT,
  FS_IDLE,
}

public class LevelTransitionManager : Singleton<LevelTransitionManager>
{
  GUITexture Gtxt = null;
  public GameObject GtxtObj = null;

  public bool Fading = false;
  bool FTB = false;
  bool FTC = false;
  bool loadLevel;
  bool FadeComplete = false;
  bool LoadLevelOnComplete = false;
  
  bool m_FadeOutIn = false;

  float FadeSpeed = 0.0f;
  float StartTimer = 0.0f;

  Color StartColor = Color.clear;
  Color TargetColor = Color.clear;

  string LevelToLoad = "";

    GameObject HudCam = null;
    Camera CamScriptHud = null;
    int StartingMaskHud = 0;


  public void Initialize()
  {

  }

  public void ChangeLevel(string LevelName, bool Fade = true, float FadeTime = 1.0f)
  {
    GamestateManager.GetSingleton.ResumeGame();

    if (Fade == false)
    {
      LoadLevel(LevelName);
      return;
    }
    else
    {
      FadeComplete = false;
      FadeThenLoadLevel(LevelName, FadeTime);
    }
  }

  public void FadeOutIn(float Timelimit = 1.0f)
  {
    FadeSpeed = Timelimit;
    m_FadeOutIn = true;
    FadeOut(Timelimit);
  }

  void OnLevelWasLoaded(int level)
  {
    if(Gtxt && Gtxt.enabled)
    FadeIn(FadeSpeed);
  }

  public void LoadLevelAdditive(string LevelName)
  {
    SceneManager.LoadScene(LevelName, LoadSceneMode.Additive);
  }

  void LoadLevel(string LevelName)
  {
     SceneManager.LoadScene(LevelName,LoadSceneMode.Single);
  }

  void FadeThenLoadLevel(string LevelName, float FadeTime)
  {
    LoadLevelOnComplete = true;
    LevelToLoad = LevelName;

    FadeOut(FadeTime);
  }

  void FadeOut(float Timelimit)
  {
    ActivateGtxt();
    StartTimer = Time.time;
    FadeSpeed = Timelimit;
    TargetColor = Color.black;
    StartColor = Color.clear;
    Fading = true;
    FTB = true;
    FTC = false;
    Gtxt.color = StartColor;

    HudCam = GameObject.FindGameObjectWithTag("HudCamera");

    if (HudCam)
    {
        CamScriptHud = HudCam.GetComponent<Camera>();
        StartingMaskHud = CamScriptHud.cullingMask;
        CamScriptHud.cullingMask = (1 << LayerMask.NameToLayer("TransitionObject"));
    }
  }

  void FadeIn(float Timelimit)
  {
    Fading = true;
    StartTimer = Time.time;
    FadeSpeed = Timelimit;
    TargetColor = Color.clear;
    StartColor = Color.black;
    FTB = false;
    FTC = true;
    Gtxt.color = StartColor;
 }

  void FadeUpdate()
  {
    float percentFade = (Time.time - StartTimer) / FadeSpeed;

    Gtxt.color = Color.Lerp(StartColor, TargetColor, percentFade);

    if (FTC)
    {
      if (Gtxt.color.a <= 0.01f)
      {
        Fading = false;
        Gtxt.enabled = false;
      }
    }

    else if (FTB)
    {
      if (Gtxt.color.a >= 0.99f)
      {
        Fading = false;
        FadeComplete = true;

        if(m_FadeOutIn)
        {
          m_FadeOutIn = false;
          FadeIn(FadeSpeed);
        }

      }
    }
  }

  void ActivateGtxt ()
  {
    if (Gtxt && GtxtObj)
    {
      GtxtObj.transform.position = Camera.main.transform.position;

      GtxtObj.transform.localScale = new Vector3(1, 1, 1);

      GtxtObj.transform.position = new Vector3(0.5f,0.5f,0.0f);

      Gtxt.enabled = true;
    }
  }

	// Use this for initialization
    void Awake () 
    {
        GtxtObj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("GuiTextureObj"));

        Gtxt = GtxtObj.GetComponent<GUITexture>();

        Gtxt.enabled = false;

        DontDestroyOnLoad(GtxtObj);
    }
	
	// Update is called once per frame
    void Update () 
    {
	    if(Fading)
        {
            FadeUpdate();
        }
        else if(FadeComplete && LoadLevelOnComplete)
        {
            if (HudCam)
            {
                CamScriptHud = HudCam.GetComponent<Camera>();
                StartingMaskHud = CamScriptHud.cullingMask;
                CamScriptHud.cullingMask = (1 << LayerMask.NameToLayer("TransitionObject"));
            }

            FadeComplete = false;
            LoadLevelOnComplete = false;
            LoadLevel(LevelToLoad);
        }

        if(InputManager.GetSingleton.IsKeyTriggered(KeyCode.R))
        {
            ChangeLevel(SceneManager.GetActiveScene().name, true, 1.0f);
        }

    }
}
