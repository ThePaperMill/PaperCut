/****************************************************************************/
/*!
\file  ObjectActions.cs
\author Joshus Biggs 
\brief  
 
  
    © 2016 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;
using ActionSystem;

public class ObjectActions : MonoBehaviour
{
    public ActionGroup Actions = new ActionGroup();
    
	// Use this for initialization
	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Debug.Log(Actions.IsEmpty());
        Actions.Update(Time.smoothDeltaTime);
	}
}
