/****************************************************************************/
/*!
\file   PhysicsCharacterController.cs
\author Steven Gallwas
\brief  
    This file contains the implementation of a simple physics based character
 * controller.  This was devised when the sweptcontrller, ran into tunneling
 * issues.
 
  © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;

public class PhysicsCharacterController : MonoBehaviour
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
    float JumpPower = 275.0f;

    public float MoveSpeed = 10.0f;

    public float maxspeed = 15.5f;

    public Vector3 WorldUp = new Vector3(0.0f, 1.0f, 0.0f);

    Rigidbody RBody;

    CapsuleCollider CCollider = null;

    float GroundContactDistance = 0.1f;

    GameObject Cam = null;

    int CastFilter = 0;

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

    // Use this for initialization
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

    void FixedUpdate()
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

    void MovementUpdate()
    {
        Vector3 movement = new Vector3(0, 0, 0);

        var LeftStickPosition = InputManager.GetSingleton.GetLeftStickValues();

        // check interact / jump first
        if (InteractPressed)
        {
            if (Grounded)
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

        // remove any upward velocity and normalize the movement vector
        movement = movement - Vector3.Project(movement, WorldUp);
        movement.Normalize();

        //RaycastHit RayHit = new RaycastHit();

        //bool SweepResults = RBody.SweepTest(movement, out RayHit, movement.magnitude * MoveSpeed * Time.fixedDeltaTime);

        var SweepResults = Physics.CapsuleCast(transform.position - transform.up * 0.5f * CCollider.height, transform.position + transform.up * 0.5f * CCollider.height, CCollider.radius, movement, movement.magnitude * MoveSpeed * Time.fixedDeltaTime, CastFilter);

        //var hitColliders = Physics.OverlapSphere(CCollider.center, CCollider.height, CastFilter);

        //foreach(var test in hitColliders)
        //{
        //  print(test.gameObject.name);
        //}

        Vector3 curVel = RBody.velocity;

        Vector3 newVel = (MoveSpeed * movement * Time.fixedDeltaTime) + curVel;

        newVel *= 0.99f;

        RBody.velocity = newVel;

        print("Starting vel" + RBody.velocity);

        // we have to check if we are colliding against a wall in the air, if so cancel our x velocity, i.e. don't stick to wall
        if (!Grounded)
        {
          //var hitColliders = Physics.OverlapSphere(CCollider.center, CCollider.radius, CastFilter);

          //foreach (var test in hitColliders)
          //{
          //  RBody.velocity = new Vector3(0.0f, RBody.velocity.y, 0.0f);
          //  break;
          //}
        }  

        // clamp velocity
        //ClampVelocity();

        print(RBody.velocity);

        CheckGround();
    }

    /****************************************************************************/
    /*!
    \brief  
        Checks the ground to see if the player is grounded
    */
    /****************************************************************************/
    void CheckGround()
    {
        Ray GroundRay = new Ray();

        GroundRay.direction = -WorldUp;
        GroundRay.origin = transform.position; // +(-WorldUp * CCollider.height);

        RaycastHit Hitinfo = new RaycastHit();

        bool GroundCheck = Physics.SphereCast(GroundRay, CCollider.radius, out Hitinfo, GroundContactDistance + CCollider.height * 0.5f);

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
        clamps the players velocity to the max value
    */
    /*************************************************************************/
    private void ClampVelocity()
    {
        Vector3 ClampSpeed = RBody.velocity;

        if (RBody.velocity.x > maxspeed)
        {
            ClampSpeed.x = maxspeed;
        }
        if (RBody.velocity.y > maxspeed)
        {
            ClampSpeed.y = maxspeed;
        }
        if (RBody.velocity.z > maxspeed)
        {
            ClampSpeed.z = maxspeed;
        }

        RBody.velocity = ClampSpeed;
    }

    /****************************************************************************/
    /*!
      \brief
       Function to be called when the menu is updated
    */
    /****************************************************************************/
    void MenuUpdate()
    {
        // currently nothing, when we have a conversation system, the code could go here
    }

    /****************************************************************************/
    /*!
      \brief
       Called each frame to get the updated input for the player
    */
    /****************************************************************************/
    void UpdateInput()
    {
        MoveForward = InputManager.GetSingleton.IsButtonDown(XINPUT_BUTTONS.BUTTON_DPAD_UP) || InputManager.GetSingleton.IsKeyDown(KeyCode.UpArrow) || InputManager.GetSingleton.IsKeyDown(KeyCode.W);
        MoveBack = InputManager.GetSingleton.IsButtonDown(XINPUT_BUTTONS.BUTTON_DPAD_DOWN) || InputManager.GetSingleton.IsKeyDown(KeyCode.DownArrow) || InputManager.GetSingleton.IsKeyDown(KeyCode.S);
        MoveLeft = InputManager.GetSingleton.IsButtonDown(XINPUT_BUTTONS.BUTTON_DPAD_LEFT) || InputManager.GetSingleton.IsKeyDown(KeyCode.LeftArrow) || InputManager.GetSingleton.IsKeyDown(KeyCode.A);
        MoveRight = InputManager.GetSingleton.IsButtonDown(XINPUT_BUTTONS.BUTTON_DPAD_RIGHT) || InputManager.GetSingleton.IsKeyDown(KeyCode.RightArrow) || InputManager.GetSingleton.IsKeyDown(KeyCode.D);
        InteractPressed = InputManager.GetSingleton.IsButtonTriggered(XINPUT_BUTTONS.BUTTON_A) || InputManager.GetSingleton.IsKeyTriggered(KeyCode.Space);
    }
}
