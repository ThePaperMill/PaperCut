/****************************************************************************/
/*!
\file  Game.cs
\author Joshus Biggs 
\brief  
 
  
    © 2016 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;
using System;

public class Game : MonoBehaviour
{
    static public GameObject GameSession;
    static public LevelInfo CurrentLevel;
    public LevelInfo LevelInformation;
	// Use this for initialization
	void Awake ()
    {
        var obj = GameObject.FindGameObjectWithTag("GameSession");
        if(obj.Equals(null))
        {
            obj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/GameSession"));
        }
        GameSession = obj;
        CurrentLevel = LevelInformation;
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}

//[Serializable]
//public struct LevelInfo
//{
//    public string Name;
//}