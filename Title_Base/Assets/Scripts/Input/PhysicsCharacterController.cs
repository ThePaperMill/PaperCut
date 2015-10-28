/****************************************************************************/
/*!
\file   PhysicsCharacterController.cs
\author Steven Gallwas
\brief  
    This file contains the implementation of a simple physics based character
 * controller.  This was devised when the sweptcontrller, ran into tunneling
 * issues.
 
 * Always Lerp camera to be behind the player*
 * 
 * 
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
    private bool Jumping  = false;

    // vector 3 to represent the current ground normal, needed for slopes.
    Vector3 GroundNormal = new Vector3(0,0,0);

    // how strong of a jump do we want.
    float JumpPower = 275.0f;

    // how fast we can move
    public float MoveSpeed = 10.0f;

    public float maxspeed = 15.5f;

    public Vector3 WorldUp = new Vector3(0.0f, 1.0f, 0.0f);

    Rigidbody RBody;

    CapsuleCollider CCollider = null;

    float GroundContactDistance = 0.1f;

    GameObject Cam = null;

    int CastFilter = 0;

    public float MaxGroundSlopeDegreeAngle = 45.0f;

    public float MaxCeilingSlope = 45.0f;

    float Epsilon = 0.001f;

    Vector3 DesiredVelocity = new Vector3();

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
       returns true if the player is grounded
    */
    /*************************************************************************/
    void UpdateVelocity(Vector3 movement)
    {
      // remove any upward velocity and normalize the movement vector
      movement = movement - Vector3.Project(movement, WorldUp);

      // we also want to project our movement onto slopes, so we can climb ramps
      movement = Vector3.ProjectOnPlane(movement, GroundNormal);

      // normalize the move vector.
      movement.Normalize();

      // calculate the force we want to move at.
      Vector3 newVel = (MoveSpeed * movement * Time.deltaTime);

      // apply the force as an impulse
      RBody.AddForce(newVel, ForceMode.Impulse);


      DesiredVelocity = MoveSpeed * movement * Time.fixedDeltaTime;
      DesiredVelocity.Normalize();      
    }


    /*************************************************************************/
    /*!
      \brief
        clamps the players velocity to the max value
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
        
      
        CastFilter = 1 << 9;
        CastFilter = ~CastFilter;
    }

    /*************************************************************************/
    /*!
      \brief
        clamps the players velocity to the max value
    */
    /*************************************************************************/
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

    /*************************************************************************/
    /*!
      \brief
        clamps the players velocity to the max value
    */
    /*************************************************************************/
    void MovementUpdate()
    {
        CheckGround();

        Vector3 movement = new Vector3(0, 0, 0);

        var LeftStickPosition = InputManager.GetSingleton.GetLeftStickValues();

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

        UpdateVelocity(movement);


        if (Grounded)
        {
          // check interact / jump first
          if (InteractPressed)
          {
              RBody.AddForce(0, this.JumpPower, 0);
              Jumping = true;
              Grounded = false;
            }
        }

        // we have to check if we are colliding against a wall in the air, if so cancel our x velocity, i.e. don't stick to wall
        else 
        {
          SnapToGround();
          var hitColliders = Physics.OverlapSphere(transform.position, CCollider.radius * 1.01f, CastFilter);

          RBody.AddForce(0.05f * -WorldUp, ForceMode.Impulse);

          if(hitColliders.Length == 0)
          {
            if(PenetratingCollisionCheck(movement))
            {
              print("penetrated something.  ^_^");
            }
          }

          foreach (var test in hitColliders)
          {
            //print(test.name);
            Vector3 Normal = transform.position - test.transform.position;
            Normal.Normalize();
          }
        }  

        // clamp velocity
        ClampVelocity();
    }

    /*************************************************************************/
    /*!
      \brief
        clamps the players velocity to the max value
    */
    /*************************************************************************/
    void OnCollisionEnter(Collision collision)
    {
      if (!Grounded)
        foreach (var contact in collision.contacts)
        {
          bool ground = IsGroundSurface(contact.normal);
          bool ceiling = IsCeilingSurface(contact.normal);

          // we are colliding against a wall
          if (!ground && !ceiling)
          {
            RBody.velocity = new Vector3(0, 0, 0);
            RBody.AddForce(contact.normal, ForceMode.Impulse);
          }
        }

    }

    /*************************************************************************/
    /*!
      \brief
        clamps the players velocity to the max value
    */
    /*************************************************************************/
    bool PenetratingCollisionCheck(Vector3 movement)
    {
        Ray DepthTest = new Ray();

        DepthTest.origin = transform.position;
        DepthTest.direction = movement.normalized;
        RaycastHit RayInfo;

        var DepthCheck = Physics.Raycast(DepthTest, out RayInfo, movement.magnitude * MoveSpeed * Time.fixedDeltaTime + CCollider.radius, CastFilter);

        if (DepthCheck)
        {
          //print(Mathf.Abs(RayInfo.distance - CCollider.radius));
        }

        return DepthCheck;
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

      bool GroundCheck = Physics.SphereCast(GroundRay, CCollider.radius, out Hitinfo, GroundContactDistance + CCollider.height * 0.49f);

      if (GroundCheck)
      {
        GroundNormal = Hitinfo.normal;

        Grounded = true;
        Jumping = false;
      }
      else
      {
        GroundNormal = WorldUp;

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

    /****************************************************************************/
    /*!
      \brief
       Called each frame to get the updated input for the player
    */
    /****************************************************************************/
    private void SnapToGround()
    {
      RaycastHit hitInfo;

      if (Physics.SphereCast(transform.position, CCollider.radius, Vector3.down, out hitInfo, ((CCollider.height / 2f) - CCollider.radius) + 0.5f))
      {
        if (Mathf.Abs(Vector3.Angle(hitInfo.normal, Vector3.up)) < 85f)
        {
          RBody.velocity = Vector3.ProjectOnPlane(RBody.velocity, hitInfo.normal);
        }
      }
    }

    /****************************************************************************/
    /*!
      \brief
        Converts Radians to Degrees
    */
    /****************************************************************************/
    float ToDegrees(float Rads)
    {
      return (Rads * 180.0f) / Mathf.PI;
    }

    /****************************************************************************/
    /*!
      \brief
         Determines if a given normal is a ground surface relative to the player
      
      \param normal
         A vector3 representing the normal.
    */
    /****************************************************************************/
    bool IsGroundSurface(Vector3 normal)
    {
      float cosineOfAngle = Vector3.Dot(normal, WorldUp);
      cosineOfAngle = Mathf.Clamp(cosineOfAngle, -1.0f, 1.0f);
      float angle = Mathf.Acos(cosineOfAngle);

      return ToDegrees(angle - Epsilon) <= MaxGroundSlopeDegreeAngle;
    }

    /****************************************************************************/
    /*!
      \brief
       Determines if a given normal is a ceiling surface relative to the player
    */
    /****************************************************************************/
    bool IsCeilingSurface(Vector3 normal)
    {
      float cosineOfAngle = Vector3.Dot(normal, -WorldUp);
      cosineOfAngle = Mathf.Clamp(cosineOfAngle, -1.0f, 1.0f);
      float angle = Mathf.Acos(cosineOfAngle);
      return ToDegrees(angle - this.Epsilon) <= this.MaxCeilingSlope;
    }
}
