/****************************************************************************/
/*!
\file   PauseMenuManager.cs
\author Steven Gallwas
\brief  
    Manages the pause menu.
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/

using UnityEngine;
using System.Collections;
using ActionSystem;

public class PauseMenuManager : EventHandler 
{
    GUITexture Gtxt = null;
    GameObject GtxtObj = null;
    TheMusicNeverEnds FMODSound = null;
    ActionGroup grp = new ActionGroup();

    public float alpha { get; set; }

	// Use this for initialization
  void Start () 
  {
    GtxtObj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("GTexture"));

    GtxtObj.transform.position = new Vector3(0,0,-50);

    alpha = 0.0f;

    GameObject temp = GameObject.FindGameObjectWithTag("PersistentMusic");

    if(temp)
        FMODSound = temp.GetComponent<TheMusicNeverEnds>();

    if (GtxtObj != null)
    {
      Gtxt = GtxtObj.GetComponent<GUITexture>();

      // Set the texture so that it is the the size of the screen and covers it.
      Gtxt.pixelInset = new Rect(0f, 0f, Screen.width, Screen.height);

      // disable it until needed
      Gtxt.enabled = false;
    }

    EventSystem.GlobalHandler.Connect(Events.PauseGameEvent, OnPauseGameEvent);
    EventSystem.GlobalHandler.Connect(Events.ResumeGameEvent, OnResumeGameEvent);
	}
	
  void OnPauseGameEvent(EventData data)
  {
    if (Gtxt)
    {
      Gtxt.enabled = true;

      ActionSequence temp = Action.Sequence(grp);
      Action.Property(temp, this.GetProperty(o => o.alpha), 0.38f, 0.5, Ease.Linear);

      if (FMODSound)
      {
            FMODSound.PauseSound();
      }
    }
  }

  void OnResumeGameEvent(EventData data)
  {
    if (Gtxt)
    {
      Gtxt.enabled = false;

      ActionSequence temp = Action.Sequence(grp);
      Action.Property(temp, this.GetProperty(o => o.alpha), 0.0f, 0.5, Ease.Linear);
      
      if(FMODSound)
         FMODSound.UnpauseSound();
    }
  }


	// Update is called once per frame
	void Update () 
    {
        grp.Update(Time.unscaledDeltaTime);
        var col = Gtxt.color;

        Gtxt.color = new Vector4(col.r, col.g, col.b, alpha);

        // Make sure we actually found the UndyingMusic
        if(!FMODSound)
        {
            GameObject temp = GameObject.FindGameObjectWithTag("PersistentMusic");

            if (temp)
                FMODSound = temp.GetComponent<TheMusicNeverEnds>();
        }
    }

    void OnDestroy()
    {
        EventSystem.GlobalHandler.Disconnect(Events.PauseGameEvent, OnPauseGameEvent);
        EventSystem.GlobalHandler.Disconnect(Events.ResumeGameEvent, OnResumeGameEvent);
    }
}
