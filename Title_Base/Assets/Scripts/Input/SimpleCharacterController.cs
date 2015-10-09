/****************************************************************************/
/*!
\file   SimpleCharacterController.cs
\author Steven Gallwas
\brief  
    This file contains the implementation of a self contained character controller
    this is translation based, and still needs refinement to be smooth.
 
  © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class SimpleCharacterController : MonoBehaviour 
{
  // basic move booleans 
  private bool MoveForward;
  private bool MoveBack;
  private bool MoveLeft;
  private bool MoveRight;

  // special buttons for triggering specific actions.
  private bool InteractPressed;
  private bool StartPressed;

  // used to determine when the menu is active, we may want something different for this.
  bool MenuActive;

  // boolean for checking if we are grounded or not
  private bool Grounded = true;
  private bool Jumping;

  // how strong of a jump do we want.
  float JumpPower = 175.0f;
  //int MaxJumps = 1;

  public float MoveSpeed = 2.5f;

  public Vector3 WorldUp = new Vector3(0.0f, 1.0f, 0.0f);

  Rigidbody RBody;

  CapsuleCollider CCollider = null;

  float GroundContactDistance = 1.1f;

  GameObject Cam = null;

  int CastFilter = 0;

  private Vector3 Movement = new Vector3(0, 0, 0);

  /*************************************************************************/
  /*!
    \brief
     returns true if the player is grounded
  */
  /*************************************************************************/
  public bool isGrounded()
  {
    return Grounded;
  }

  /*************************************************************************/
  /*!
    \brief
     Initialize the class, store camera, body, and collider
  */
  /*************************************************************************/
  void Start()
  {
    // initialize booleans to false
    MoveForward = false;
    MoveBack = false;
    MoveLeft = false;
    MoveRight = false;
    MenuActive = false;

    RBody = (Rigidbody)GetComponent<Rigidbody>();
    Cam = GameObject.FindGameObjectWithTag("MainCamera");
    CCollider = (CapsuleCollider)GetComponent<CapsuleCollider>();

    CastFilter = 1 << 8;

    CastFilter = ~CastFilter;
  }

  /*************************************************************************/
  /*!
    \brief
     main update loop, called each frame
  */
  /*************************************************************************/
  void Update()
  {
    // this checks what input has been given this frame
    UpdateInput();

    if (!MenuActive)
    {
      MovementUpdate();
    }
    else
    {
      MenuUpdate();
    }

  }

  /*************************************************************************/
  /*!
    \brief
     update the players movement, should be called each update
  */
  /*************************************************************************/
  void MovementUpdate()
  {
    Vector3 movement = new Vector3(0, 0, 0);

    var LeftStickPosition = InputManager.GetSingleton.GetLeftStickValues();

    // check interact / jump first
    if (InteractPressed)
    {
      if (Jumping == false && Grounded)
      {
        RBody.AddForce(0, this.JumpPower, 0);
        Jumping = true;
        Grounded = false;
      }
    }

    if (MoveForward || LeftStickPosition.YPos > 0.3)
    {
        movement += Cam.transform.forward;
    }

    else if (MoveBack || LeftStickPosition.YPos < -0.3)
    {
        movement -= Cam.transform.forward;
    }

    if (MoveLeft || LeftStickPosition.XPos < -0.3)
    {
        movement -= Cam.transform.right;
    }

    else if (MoveRight || LeftStickPosition.XPos > 0.3)
    {
        movement += Cam.transform.right;
    }

    movement = movement - Vector3.Project(movement, WorldUp);
    movement.Normalize();

    RaycastHit HitInfo = new RaycastHit() ;

    float sweepTime = Time.deltaTime;

    // sweep in the direction we are moving in if we detecta collsion move a specific ammount
    if (RBody.SweepTest(movement, out HitInfo, movement.magnitude * MoveSpeed * sweepTime))
    {
        // move forward only to where we collided
        transform.position += movement * MoveSpeed * sweepTime;
    }

    // if we aren't colliding, move unhindered.
    else
    {
      transform.position += movement * MoveSpeed * sweepTime;
    }

    CheckGround();
  }

  /*************************************************************************/
  /*!
    \brief
     raycasts the ground to check grounded status
  */
  /*************************************************************************/
  void CheckGround()
  {
    Ray GroundRay = new Ray();

    GroundRay.direction = -WorldUp;
    GroundRay.origin = transform.position + (-WorldUp * 0.49f * CCollider.height);

    RaycastHit Hitinfo = new RaycastHit();

    //bool GroundCheck = Physics.SphereCast(GroundRay, CCollider.radius, out Hitinfo, GroundContactDistance, CastFilter);

    bool GroundCheck = RBody.SweepTest(-WorldUp, out Hitinfo, GroundContactDistance);

    print(GroundCheck);

    if (GroundCheck)
    {
        Grounded = true;
        Jumping = false;
    }
    else
    {
        Grounded = false;
    }
  }

  /*************************************************************************/
  /*!
    \brief
     should be called when the menu is open 
  */
  /*************************************************************************/
  void MenuUpdate()
  {
    // currently nothing, when we have a conversation system, the code could go here
  }

  /*************************************************************************/
  /*!
    \brief
     Checks input for each of the desired booleans.
  */
  /*************************************************************************/
  void UpdateInput()
  {
    MoveForward = InputManager.GetSingleton.IsButtonDown(XINPUT_BUTTONS.BUTTON_DPAD_UP)    || InputManager.GetSingleton.IsKeyDown(KeyCode.UpArrow)    || InputManager.GetSingleton.IsKeyDown(KeyCode.W);
    MoveBack = InputManager.GetSingleton.IsButtonDown(XINPUT_BUTTONS.BUTTON_DPAD_DOWN)     || InputManager.GetSingleton.IsKeyDown(KeyCode.DownArrow)  || InputManager.GetSingleton.IsKeyDown(KeyCode.S);
    MoveLeft = InputManager.GetSingleton.IsButtonDown(XINPUT_BUTTONS.BUTTON_DPAD_LEFT)     || InputManager.GetSingleton.IsKeyDown(KeyCode.LeftArrow)  || InputManager.GetSingleton.IsKeyDown(KeyCode.A);
    MoveRight = InputManager.GetSingleton.IsButtonDown(XINPUT_BUTTONS.BUTTON_DPAD_RIGHT)   || InputManager.GetSingleton.IsKeyDown(KeyCode.RightArrow) || InputManager.GetSingleton.IsKeyDown(KeyCode.D);
    InteractPressed = InputManager.GetSingleton.IsButtonTriggered(XINPUT_BUTTONS.BUTTON_A) || InputManager.GetSingleton.IsKeyTriggered(KeyCode.Space);
  }
}
