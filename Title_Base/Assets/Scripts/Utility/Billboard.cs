/****************************************************************************/
/*!
\file   Billboard.cs
\author Steven Gallwas
\brief  
    This file contains the implementation to make an object always look at the
    camera.
 
  © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;

public class Billboard : MonoBehaviour
{
	// Use this for initialization
	void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        // saved for future reference, this lets us easily always look at the camera
        transform.rotation = Quaternion.LookRotation(Camera.main.transform.forward);
    }
}
