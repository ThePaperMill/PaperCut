/****************************************************************************/
/*!
\file   Bounce.cs
\author Jerry Nacier
\brief  
    =]

    Makes sure player always faces the camera. This script is attached to the Middleman object between the Player and the DynamicPlayer object

    Thanks to all those who died to make this possible.
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;

public class FaceTheCamera : MonoBehaviour
{
    // the main camera in the level
    GameObject Cam = null;  

    void Start ()
    {
        // find the camera so we can look at it
        Cam = GameObject.FindGameObjectWithTag("MainCamera");
    }
	
	void Update ()
    {
        // Create a vec3 var and make it's xyz equal to the Camera's x, Your own y, and the Camera's z. You won't flip on the y axis, but you'll always face the camera
        Vector3 Temp = new Vector3(Cam.transform.position.x, this.transform.position.y, Cam.transform.position.z); // CamX, your y, CamZ
        // Call transform's LookAt function and pass in Temp
        transform.LookAt(Temp);
    }
}
