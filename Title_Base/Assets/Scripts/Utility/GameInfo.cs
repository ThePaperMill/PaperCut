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

public class GameInfo : Singleton<GameInfo>
{
   public Material PlayerColor = null;
    public bool LabDestroyed = false;
    public bool FinaleReady = false;

    // Use this for initialization
    void Start ()
    {
	
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
