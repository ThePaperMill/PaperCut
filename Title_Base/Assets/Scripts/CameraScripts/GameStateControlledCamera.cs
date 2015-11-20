/*********************************
 * GameStateControlledCamera.cs
 * Ian Aemmer
 * Created 9/7/2015
 * Copyright © 2015 DigiPen Institute of Technology, All Rights Reserved
 *********************************/

using UnityEngine;
using System.Collections;
using ActionSystem.Internal;
using ActionSystem; //You either must say "using ActionSystem" or put "ActionSystem." in front of anything having to do with actions.
using System.Reflection;
using System; //If you are usung System, you will need to put "ActionSystem." in front of any calls on the "Action" class.

public class GameStateControlledCamera : MonoBehaviour
{
  //AimTarget is the object the camera will be looking at starting from the beginning of the level. If it changes, what the camera will look at changes.
  public GameObject AimTarget = null;
  //PosTarget is the object the camera will be hovering around starting at the beginning of the level. The distance from the target will be saved.
  public GameObject PosTarget = null;

  //Aim Tracker is the object that camera will look at EVERY Update. It follows around the objects you're actually tracking via Actions.  
  GameObject AimTracker = null;
  //PositionVector is the vector away from the PosTarget the camera will attempt to stay around. It will interpolate to it EVERY update, and the camera follows it around via actions.
  Vector3 PositionVector = Vector3.zero;

  //AimGrp is an actiongroup covering the focus of the camera in every frame
  ActionGroup AimGrp = new ActionGroup();
  //PosGrp is an actiongroup covering the hover of the camera in every frame
  ActionGroup PosGrp = new ActionGroup();

  //Test use
  //public GameObject testPlsDelete = null;

  // Use this for initialization
  void Start()
  {
    //Create the TargetAimingBlock if it does not exist
    AimTracker = new GameObject();
    //Move the aimtracker's position to 5 in the direction of it's forward looking vector. this is to prevent "Jumping".
    AimTracker.transform.position = transform.localPosition + transform.forward * 5;
    //Print out an error if there is no camera target
    if (AimTarget == null)
    {
      //print("ERROR: Camera does not posess an AimTarget");
    }
    //otherwise, move the TargetTracker to the new target instantly
    else
    {
      //Move the AimTargetTracker to the new target over the course of 0 seconds
      SwitchAim(AimTarget, float.Epsilon);
    }
    //print out an error if there is no pos target
    if (PosTarget == null)
    {
      //print("ERROR: Camera does not posess a PosTarget");
    }
    //Otherwise, save the vector from the object the camera should be hovering.
    else
    {
      PositionVector = transform.position - PosTarget.transform.position;
    }
  }

  // Update is called once per frame
  void Update()
  {
    /*//Test Use
    if (Input.GetKeyUp(KeyCode.UpArrow))
    {
        SwitchAim(testPlsDelete, 5);
        ChangeHover(testPlsDelete);
    }*/

    //Call this every frame to move slowly towards the hover object (PosTarget)
    SwitchPosition();

    //You MUST call update on the master group every rame and pase in the desired for of Delta Time
    AimGrp.Update(Time.deltaTime);
    PosGrp.Update(Time.deltaTime);

    //Look at the targetAimingBlock everyFrame
    transform.LookAt(AimTracker.transform);
  }

  public void SwitchAim(GameObject newAimTarget, float timer)
  {
    //Change the AimTracker's Parent so that it's origin is the position of that object.
    AimTracker.transform.SetParent(newAimTarget.transform);
    //Clear the current action systems on the Aim if there is one. (this is so the object doesnt continue to move in it's current path and adopts the below one instead)
    AimGrp.Clear();
    //Declare a new sequence using AimGrp
    var seq = ActionSystem.Action.Sequence(AimGrp);
    //Interpolate the X position of the AimTracker so that it approaches zero over the course of Timer
    ActionSystem.Action.Property(seq, AimTracker.transform.GetProperty(x => x.localPosition), Vector3.zero, timer, Ease.QuadInOut);
  }

  public void SwitchPosition()
  {
    if (PosTarget == null)
      return;

    //Calculate how fast the camera should move to it's target
    var journeyLength = Vector3.Distance(transform.position, PosTarget.transform.position + PositionVector);
    float distCovered = Vector3.Distance(transform.position, PosTarget.transform.position + PositionVector);
    float fracJourney = distCovered / journeyLength + 100;
    //Slerp to the target position
    transform.position = Vector3.Slerp(transform.position, PosTarget.transform.position + PositionVector, Time.smoothDeltaTime);
  }

  public void UpdatePositionVector()
  {
    PositionVector = transform.position - PosTarget.transform.position;
  }

  public void ChangeHover(GameObject newHover)
  {
    //Change the object we're hovering around
    PosTarget = newHover;
  }
}
