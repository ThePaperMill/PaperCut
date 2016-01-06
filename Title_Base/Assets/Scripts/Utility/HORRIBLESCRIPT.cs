/****************************************************************************/
/*!
\file   HORRIBLESCRIPT.cs
\author Ian Aemmer
\brief  
    "Can you tell by my shitty code that its me?" - Ian Aemmer.
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class HORRIBLESCRIPT : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        //Application.LoadLevelAdditive("CameraWorld");
        SceneManager.LoadScene("CameraWorld", LoadSceneMode.Additive);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
