/****************************************************************************/
/*!
\file   OnCollideSwapBool.cs
\author Ian Aemmer
\brief  
    read name.
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;

public class OnCollideSwapBool : MonoBehaviour {

    public bool IsOn = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            IsOn = true;
        }
    }
}
