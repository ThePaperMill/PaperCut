/****************************************************************************/
/*!
\file   PlayerSpawnPoint.cs
\author Steven Gallwas
\brief  
    This file contains the implementation of the level manager
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSpawnPoint : MonoBehaviour
{
  public List<string> PreviousLevel;
  
  public Vector3 Position = new Vector3(); 

  public PlayerSpawnPoint()
  {

  }

    void Start()
    {
        MeshRenderer test = gameObject.GetComponent<MeshRenderer>();

        if(test)
        {
          test.enabled = false;
        }

        // Dispatch an event to the global handler.
        Position = gameObject.transform.position;

        PlayerSpawnEvent PE = new PlayerSpawnEvent(this);

        EventSystem.GlobalHandler.DispatchEvent(Events.AddSpawnPoint, PE);
	}
	
	// Update is called once per frame
	void Update ()
  {
	
	}
}
