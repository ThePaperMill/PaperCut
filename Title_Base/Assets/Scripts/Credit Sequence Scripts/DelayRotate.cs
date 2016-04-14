/****************************************************************************/
/*!
\file  DelayRotate.cs
\author Ian Aemmer
\brief  
 
  
    © 2016 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;

public class DelayRotate : MonoBehaviour {

    Vector3 rotationtracker = Vector3.zero;
    public Vector3 AddVector = Vector3.zero;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {


        // if anyone cares that this is hacky let it be known that I asked before doing this.
        rotationtracker += 60.0f * AddVector * Time.deltaTime;

        transform.localRotation = Quaternion.Euler(rotationtracker);
	}
}
