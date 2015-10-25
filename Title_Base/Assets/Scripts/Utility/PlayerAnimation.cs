/****************************************************************************/
/*!
\file   PlayerAnimation.cs
\author Steven Gallwas
\brief  
    This file contains the implementation of a self contained character controller
    this is translation based, and still needs refinement to be smooth.
 
  © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;
using ActionSystem;


[RequireComponent(typeof(MeshRenderer))]
public class PlayerAnimation : MonoBehaviour 
{
  MeshRenderer MRenderer   = null;  // the player's mesh renderer
  GameObject PlayerModel   = null;  // the Player's model
  GameObject Cam           = null;  // the main camera in the level
  ActionGroup grp          = new ActionGroup();
  Vector3 StartRotation    = new Vector3();

  /****************************************************************************/
  /*!
  \brief  
      Initialize the class, make ourselved invisible and create a stand
      in for the player to animate.
  */
  /****************************************************************************/
    void Start () 
    {
        MRenderer = GetComponent<MeshRenderer>();

        // hide the original renderer
        MRenderer.enabled = false;

        // create a new gameobject that only has a model and 
        PlayerModel = new GameObject();

        PlayerModel.transform.localScale = gameObject.transform.lossyScale;

        var tempMRender = PlayerModel.AddComponent<MeshRenderer>();
        var tempModel   = PlayerModel.AddComponent<MeshFilter>();

        tempModel.mesh = GetComponent<MeshFilter>().mesh;

        tempMRender.sharedMaterial = MRenderer.sharedMaterial;

        // find the camera so we can look at it
        Cam = GameObject.FindGameObjectWithTag("MainCamera");

        StartRotation = transform.localEulerAngles;
    }
	
    public void RotatePlayer()
    {
        // lerp up away from the camera
        var test = ActionSystem.Action.Sequence(grp);
        var finalPos = new Vector3(0.0f, 90.0f, 0.0f);
        finalPos.z = transform.localPosition.z;

        Action.Property(test, gameObject.transform.GetProperty(o => o.localEulerAngles), StartRotation + finalPos, 0.5, Ease.Linear);
    }

	void Update () 
    {
        // get the camera's position and look at it.
        Vector3 Lookposition = new Vector3(Cam.transform.position.x, transform.position.y, Cam.transform.position.z);
        
        // immediately look at the camera
        //PlayerModel.transform.LookAt(Lookposition);

        var newRotation = Quaternion.LookRotation(PlayerModel.transform.position - Lookposition, Vector3.forward);
        newRotation.x = 0.0f;
        newRotation.z = 0.0f;
        PlayerModel.transform.rotation = Quaternion.Slerp(PlayerModel.transform.rotation, newRotation, Time.deltaTime * 8);

        // update our position to be the same as the players
        PlayerModel.transform.position = transform.position;

        grp.Update(Time.deltaTime);
    }

   
}
