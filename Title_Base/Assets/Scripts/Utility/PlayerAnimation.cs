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


public enum FLIP_MODEL
{
    FLIP_POSITIVE,
    FLIP_NEGATIVE,
}

[RequireComponent(typeof(MeshRenderer))]
public class PlayerAnimation : MonoBehaviour 
{
  MeshRenderer MRenderer        = null;  // the player's mesh renderer
  public GameObject PlayerModel = null;  // the Player's model
  GameObject Cam                = null;  // the main camera in the level
  ActionGroup grp               = new ActionGroup();
  public Vector3 RotationAngle  = new Vector3();

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
    }
	
    public void RotateModel(Vector3 Rotation)
    {
        RotationAngle = Rotation;

        //return;
        var test = ActionSystem.Action.Sequence(grp);

        ActionSystem.Action.Property(test, this.PlayerModel.transform.GetProperty(x => x.localEulerAngles), Rotation, 0.25f, Ease.Linear);
    }

    /****************************************************************************/
    /*!
        \brief
          Really Hacky way to do this.
    */
    /****************************************************************************/
    public void FlipModel (FLIP_MODEL val)
    {
        if (val == FLIP_MODEL.FLIP_POSITIVE && PlayerModel.transform.localScale.x < 0)
        {
            var curscale = PlayerModel.transform.localScale;

            PlayerModel.transform.localScale = new Vector3(-curscale.x,curscale.y,curscale.z);
        }
        else if (val == FLIP_MODEL.FLIP_NEGATIVE && PlayerModel.transform.localScale.x > 0)
        {
            var curscale = PlayerModel.transform.localScale;

            PlayerModel.transform.localScale = new Vector3(-curscale.x, curscale.y, curscale.z);
        }
    }

    void Update () 
    {
        // get the camera's position and look at it.
        Vector3 Lookposition = new Vector3(Cam.transform.position.x, transform.position.y, Cam.transform.position.z);

        Quaternion newRotation = new Quaternion();

        newRotation = Quaternion.LookRotation((PlayerModel.transform.position - Lookposition), Vector3.forward);

        //Vector3 EulerAngle = newRotation.eulerAngles;

        newRotation.x = 0.0f;
        newRotation.z = 0.0f;

        PlayerModel.transform.rotation = Quaternion.Slerp(PlayerModel.transform.rotation, newRotation, Time.deltaTime * 8);

        // update our position to be the same as the players
        PlayerModel.transform.position = transform.position;

        grp.Update(Time.deltaTime);
    }
}
