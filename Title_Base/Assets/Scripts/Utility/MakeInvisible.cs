/****************************************************************************/
/*!
\file   MakeInvisible.cs
\author Steven Gallwas
\brief  
    Makes things invisible.
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/

using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer))]
public class MakeInvisible : MonoBehaviour
{

  MeshRenderer MRenderer = null;

	// Use this for initialization
	void Start () 
  {
	  MRenderer = gameObject.GetComponent<MeshRenderer>();
    MRenderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
