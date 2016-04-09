/****************************************************************************/
/*!
\file   TheMusicNeverEnds.cs
\author Troy
\brief  
    Increases Troy's anger towards FMOD and all who coded it. 
  
    © 2016 DigiPen, All Rights Reserved.
*/
/****************************************************************************/

using UnityEngine;
using System.Collections;

public class StartSoundAfterStart : MonoBehaviour
{

    bool done = false;
	
	// Update is called once per frame
	void Update ()
    {
	    if(!done)
        {
            gameObject.GetComponent<FMOD_StudioEventEmitter>().StartEvent();
            done = true;
        }
	}
}
