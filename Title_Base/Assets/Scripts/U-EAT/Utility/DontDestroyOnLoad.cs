/****************************************************************************/
/*!
\file  DontDestroyOnLoad.cs
\author Joshus Biggs 
\brief  
 
  
    © 2016 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;

public class DontDestroyOnLoad : MonoBehaviour 
{

    // Use this for initialization
    void Awake ()
    {
        DontDestroyOnLoad(gameObject);
	}
}


