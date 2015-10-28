/****************************************************************************/
/*!
\file   OnCollideGoToLevel.cs
\author Steven Gallwas
\brief  
    This file contains the implementation for a simple go to level script
 
  © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;

public class OnCollideGoToLevel : MonoBehaviour
{
    public string LevelName;

    // Use this for initialization
    void Start()
    {

    }

    public void ChangeLevel()
    {
        Application.LoadLevel(this.LevelName);
    }
}