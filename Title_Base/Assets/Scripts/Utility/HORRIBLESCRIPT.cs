/****************************************************************************/
/*!
\file   HORRIBLESCRIPT.cs
\author Ian Aemmer
\brief  
    "Can you tell by my shitty code that its me?" - Ian Aemmer.
    "Ian, it's just three lines of code.  Calm down." - Troy.
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class HORRIBLESCRIPT : MonoBehaviour
{
    // Horrible public variable for horrible script!
    public FMODAsset levelMusic;
    public bool doIt = true;

	// Use this for initialization
	void Start ()
    {
        if(doIt)
        {
            //Application.LoadLevelAdditive("CameraWorld");
            SceneManager.LoadScene("CameraWorld", LoadSceneMode.Additive);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}