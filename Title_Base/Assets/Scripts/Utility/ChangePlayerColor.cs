/****************************************************************************/
/*!
\file ChangePlayerColor.cs
\author Steven Gallwas
\brief  
 
  
    © 2016 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;

public class ChangePlayerColor : MonoBehaviour
{

	// Use this for initialization
	void Start ()
    {
	    if(GameInfo.GetSingleton.PlayerColor != null)
        {
            GetComponent<MeshRenderer>().material = GameInfo.GetSingleton.PlayerColor;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
