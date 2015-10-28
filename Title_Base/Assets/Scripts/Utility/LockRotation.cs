/****************************************************************************/
/*!
\file   LockRotation
\author Steven Gallwas
\brief  
    This file contains the implementation of a component that locks an objects
    rotation.
 
  © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;

public class LockRotation : MonoBehaviour 
{
  private Quaternion BaseRotation;

  public bool Lock_Rotation = false;

	void Start () 
  {
    BaseRotation = new Quaternion();
	}
	
	// Update is called once per frame
	void Update () 
  {
	  if(Lock_Rotation)
    {
      transform.rotation = BaseRotation;
    }
	}
}
