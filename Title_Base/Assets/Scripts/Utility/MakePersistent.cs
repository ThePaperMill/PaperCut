/****************************************************************************/
/*!
\file   SimpleCharacterController.cs
\author Steven Gallwas
\brief  
    This contains a script for a component that makes an object persist between
    level loads.
 
  © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;

public class MakePersistent : MonoBehaviour 
{
  public bool Persist = true;

	// Use this for initialization
	void Start () 
  {
    if (Persist)
    {
      DontDestroyOnLoad(gameObject);
    }
	}
	
	// Update is called once per frame
	void Update () 
  {
	
	}
}
