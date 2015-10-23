/****************************************************************************/
/*!
\file   CustomDynamicController.cs
\author Steven Gallwas
\brief  
    This file contains the implementation of a physics based controller
    this was based on the dynamic character controller by the zero engine
    team.  Thanks to all those who died to make this possible.
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;

public enum PlayerState
{
    Idle,
    Walking,
    Falling,
    Jumping
};

// we have to have a rigid body and capsule collider 
[RequireComponent(typeof(Rigidbody),typeof(CapsuleCollider))]
public class CustomDynamicController : MonoBehaviour
{
    // basic move booleans 
    private bool MoveForward;
    private bool MoveBack;
    private bool MoveLeft;
    private bool MoveRight;
    private bool InteractPressed;
    private bool InteractReleased;
    private bool OpenInventory;


    public bool Active = true;

    public bool StickToSlope = true;

    //The up vector of the character
    Vector3 WorldUp = new Vector3(0.0f, 1.0f, 0.0f);

    public float moveSpeed = 5.0f;

    // The maximum speed the character can achieve on its own
    public float MaxSpeed = 1.5f;

    // the speed the player accelerates at.
    public float MovePower = 3.5f;
    
    // the amount of control the player has in the air.
    public float AirControl = 0.15f;
    
    // the angle the player can walk up
    public float WalkableSlopeAngle = 45.0F;

    // the starting jump value applied when jump is pressed
    public float InitialJumpVelocity = 4.0f;

    // Extra velocity upward that is applied after a jump is initiated, every frame
    // for a specified amount of time (AdditiveJumpTime)
    public float AdditiveJumpVelocity = 2.0f;

    // the ammount of time in seconds to apply additional jump power
    public float AdditiveJumpTime = 0.0f;

    // the amount of time, while the player is in the air, that they can still jump.
    public float JumpLagTimer = 0.23f;

    // how far the away grouned should still be considered
    public float GroundContactDistance = 0.01f;

    // additional gravity to be applied to the player.
    public float AdditionalGravity = 0.0f;

    // A scalar for the amount of force the player can apply to move
    // Should be between 0-1
    // 0: No control while on the ground
    // 1: Full control while on the ground
    float Traction = 1.0f;
    
    // booleans to determine if we are jumping
    bool Jumping       = false;
    bool InAirFromJump = false;
    
    // Whether or not we're considered to be on the ground
    bool OnGround = false;
    
    // The time since we were in last direct contact with the ground
    float TimeSinceLastDirectContact = 0.0f;
    
    // The velocity of the ground we're standing on
    // This is used for to maintain velocity when jumping off of moving ground
    Vector3 VelocityOfGround = new Vector3(0.0f, 0.0f, 0.0f);

    // the players state default to idle
    PlayerState State = PlayerState.Idle;

    // used for variable height jumping
    float JumpTimer = 0.0f;
    

    // store our rigid body and capsule collider and the camera
    Rigidbody RBody = null;

    CapsuleCollider CCollider = null;

    GameObject Cam = null;

    public Vector3 MoveDirection = new Vector3(0.0f, 0.0f, 0.0f);

    int CastFilter = 0;

    Vector3 GroundNormal = Vector3.up;

    /****************************************************************************/
    /*!
        \brief
            initalize the class
    */
    /****************************************************************************/
    void Start()
    {
        RBody     = (Rigidbody)GetComponent<Rigidbody>();
        CCollider = (CapsuleCollider)GetComponent<CapsuleCollider>();
        Cam = GameObject.FindGameObjectWithTag("MainCamera");

        // we want to ignore the player layer
        CastFilter = 1 << 9;
        CastFilter = ~CastFilter;
    }

    void Update()
    {
      UpdateInput();
    }

    /****************************************************************************/
    /*!
        \brief
            initalize the class
    */
    /****************************************************************************/
    void LateUpdate()
    {
        // if the character controller is not active, do nothing
        if (Active == false)
        {
            return;
        }

        if(OpenInventory)
        {
          bool InventoryStatus = InventorySystem.GetSingleton.isInventoryOpen();

          if (!InventoryStatus)
            InventorySystem.GetSingleton.OpenInventory(InventoryState.INVENTORY_VIEW);
          
          else
            InventorySystem.GetSingleton.CloseInventory();
        }


        // Check jump first 
        if (InteractPressed)
        {
          BeginJump();
        }
        else if (InteractReleased)
        {
          EndJump();
        }

        var LeftStickPosition = InputManager.GetSingleton.GetLeftStickValues();
        Vector3 movement = new Vector3();

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


        MoveDirection = movement;

        //print(MoveDirection);

        MoveDirection.Normalize();

        // Update whether or not we are on ground, as well as getting 
        // the traction (slipperyness) of the surface we're on
        UpdateGroundState(Time.fixedDeltaTime);

        // All logic for jumping is contained in this function
        UpdateJumpState(Time.fixedDeltaTime);

        // Get our current control (value between 0-1)
        var controlScalar = this.GetCurrentControlScalar();

        // We want to set the amount of force we can apply to reach our desired maximum speed
        // We're multiplying by the object's mass so that the character can easily be scaled
        // without having to re-adjust MovePower
        float moveForce = MovePower * RBody.mass;
        
        // Apply the control scalar (air control / traction / etc...)
        moveForce *= controlScalar;
        
        // Get our current max speed
        var maxSpeed = GetMaxSpeed();

        MoveDirection = Vector3.ProjectOnPlane(MoveDirection, GroundNormal).normalized;

        if(State == PlayerState.Idle)
        {
          RBody.AddForce(MoveDirection * maxSpeed * MovePower, ForceMode.Force);
        }
        else
        {
          // Move in the given direction with our current max speed
          RBody.AddForce(MoveDirection * maxSpeed * moveForce, ForceMode.Acceleration);
        }

        if(StickToSlope && !Jumping && State == PlayerState.Idle)
        {
          if (GroundNormal != WorldUp)
          {
            RBody.velocity = new Vector3(0, RBody.velocity.y, 0);
          }
        }

        ClampVelocity();

        UpdateCurrentState(MoveDirection);
    }

    /****************************************************************************/
    /*!
        \brief
           Updates the player state, 
    */
    /****************************************************************************/
    void UpdateCurrentState(Vector3 movement)
    {
        // if we're on the ground and...
        if (OnGround)
        {
            //jumping set state to jumping otherwise...
            if (Jumping)
            {
                State = PlayerState.Jumping;
            }

            // we are idle or walking 
            else
            {
                var speed = movement.magnitude;
                
                if (speed == 0.0f)
                {
                    State = PlayerState.Idle;
                }
                
                else
                {
                    State = PlayerState.Walking;
                }
            }
        }
        
        else
        {
            if (Jumping)
            {
                State = PlayerState.Jumping;
            }
            else
            {
                State = PlayerState.Falling;
            }
        }
    }

    /****************************************************************************/
    /*!
        \brief
          returns the traction scalar based on our current grouned state
    */
    /****************************************************************************/
    float GetCurrentControlScalar()
    {
        // Use our current traction if we're on the ground
        if(OnGround)
        {
            return Traction;
        }
        
        // Otherwise, use the air control
        return AirControl;
    }

    /****************************************************************************/
    /*!
        \brief
            initalize the class
    */
    /****************************************************************************/
    float GetMaxSpeed()
    {
        float speed = moveSpeed;
        
        // If we are grounded return the max speed
        if(OnGround)
        {
            return speed;
        }

        // otherwise calculate max speed

        // Get our current velocity
        Vector3 vel = RBody.velocity;

        // Project out the up vector 
        vel = vel - WorldUp * Vector3.Dot(vel, WorldUp);
        

        float currSpeed = vel.magnitude;
        
        // Return whichever is greater
        return Mathf.Max(speed, currSpeed);
    }

    /****************************************************************************/
    /*!
        \brief
            I use OnCollisionStay, because it is the only reliable way to
            determine everything we're in contact with.
    */
    /****************************************************************************/
    void OnCollisionStay(Collision collisionInfo)
    {
      foreach (ContactPoint contact in collisionInfo.contacts)
      {
        var surfaceNormal = contact.normal;

        // If the object is considered walkable
        if (IsGround(surfaceNormal))
        {
          // Contact is valid ground
          OnGround = true;
          
          if (Jumping == false)
          {
            InAirFromJump = false;
          }

          GroundNormal = surfaceNormal;

          TimeSinceLastDirectContact = 0.0f;

          Rigidbody test = (Rigidbody)contact.otherCollider.gameObject.GetComponent<Rigidbody>();
          
          // We want to store the object's velocity so that we can
          // jump with the object's velocity taken into account
          if (test != null)
          {
            VelocityOfGround = test.velocity;
          }
        }
      }
    }

    /****************************************************************************/
    /*!
        \brief
         updates the players grounded state.  Currently, this is not called it 
         needs to be updated for special cases where jumping grounded may 
         incorectly be false
    */
    /****************************************************************************/
    void UpdateGroundState(float dt)
    {
        // Update the timer for late jumps
        TimeSinceLastDirectContact += dt;
        
        // We want to iterate through all objects we're in contact with in order
        // to determine whether or not we are in contact with the ground
        Ray GroundRay = new Ray();

        // cast downward
        GroundRay.direction = -WorldUp;

        //start at the center of the collider
        GroundRay.origin = CCollider.bounds.center;

        RaycastHit Hitinfo = new RaycastHit();

        // use bounds to get the correct height of the collider.
        float RayDistance = GroundContactDistance + CCollider.bounds.extents.y - (CCollider.bounds.extents.x);

        // use sphere cast to 
        var GroundCheck = Physics.SphereCast(GroundRay, 0.95f * CCollider.bounds.extents.x, out Hitinfo, RayDistance, CastFilter);

        Debug.DrawLine(GroundRay.origin, GroundRay.origin + new Vector3(0, RayDistance, 0));

        if(GroundCheck)
        {
            var contactHolder = Hitinfo;

            // Ignore trigger colliders
            if (contactHolder.collider.isTrigger)
            {
                return;
            }

            // Get the object we're in contact with
            var objectHit = contactHolder.collider.gameObject;

            // We need the normal of the surface (the normal that points from
            // the object hit to us) to determine whether or not it's walkable
            var surfaceNormal = contactHolder.normal;

            // If the object is considered walkable
            if (IsGround(surfaceNormal))
            {
                // Contact is valid ground
                OnGround = true;

                if (Jumping == false)
                {
                    InAirFromJump = false;
                }

                GroundNormal = surfaceNormal;

                TimeSinceLastDirectContact = 0.0f;

                Rigidbody test = (Rigidbody)objectHit.GetComponent<Rigidbody>();
                // We want to store the object's velocity so that we can
                // jump with the object's velocity taken into account
                if (test != null)
                {
                    VelocityOfGround = test.velocity;
                }
            }
        }

        if (TimeSinceLastDirectContact > JumpLagTimer)
        {
            // Reset all values
            OnGround = false;
            VelocityOfGround = new Vector3(0.0f, 0.0f, 0.0f);
            GroundNormal = WorldUp;
        }
    }   

    /****************************************************************************/
    /*!
        \brief
            initalize the class
    */
    /****************************************************************************/
    float GetDegreeDifference(Vector3 surfaceNormal)
    { 
        // Returns the angle between the surface normal and the up vector of the character
        float cosTheta = Vector3.Dot(surfaceNormal, WorldUp);

        float radians = Mathf.Acos(cosTheta);

        float degrees = radians * Mathf.Rad2Deg;

        return degrees;
    }

    /****************************************************************************/
    /*!
        \brief
            initalize the class
    */
    /****************************************************************************/
    bool IsGround(Vector3 surfaceNormal)
    {
        // If the angle of the surface's normal is less than the specified value,
        // we're considered to be on ground
        var degrees    = GetDegreeDifference(surfaceNormal);
        return degrees < WalkableSlopeAngle;
    }

    /****************************************************************************/
    /*!
        \brief
            initalize the class
    */
    /****************************************************************************/
    void BeginJump()
    {
        // Start jumping if we can
      if (OnGround)
      {
        Jump();
      }
    }

    /****************************************************************************/
    /*!
        \brief
            call this when the player releases the jump button used for variable 
            jumping
    */
    /****************************************************************************/
    void EndJump()
    {
        Jumping = false;
    }

    /****************************************************************************/
    /*!
        \brief
            initalize the class
    */
    /****************************************************************************/
    void UpdateJumpState(float dt)
    {
        // If we're currently jumping, we want to continue adding an upward velocity while
        // jump is active
        if (Jumping)
        {
            // Keep adding the additive jump velocity while Jump is still held and until we
            // have reached the additive jump timer
            if (JumpTimer < AdditiveJumpTime)
            {
                // Increment the timer
                JumpTimer += dt;

                // Add to our velocity
                RBody.velocity += WorldUp * AdditiveJumpVelocity * dt;
            }
            // Otherwise end the jump
            else
            {
                // If the player has released the jump button or we've reached the
                // end of the timer, we're no longer jumping
                Jumping = false;
            }
        }
    }

    /****************************************************************************/
    /*!
        \brief
          returns the players grounded status
    */
    /****************************************************************************/
    public bool IsGrounded()
    {
        return OnGround;
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
      InteractReleased = InputManager.GetSingleton.IsButtonReleased(XINPUT_BUTTONS.BUTTON_A) || InputManager.GetSingleton.IsKeyReleased(KeyCode.Space);
      OpenInventory = InputManager.GetSingleton.IsButtonTriggered(XINPUT_BUTTONS.BUTTON_BACK) || InputManager.GetSingleton.IsKeyTriggered(KeyCode.E);
    }

    /****************************************************************************/
    /*!
        \brief
          the main jump function, to be called by begin jump.
    */
    /****************************************************************************/
    private void Jump()
    {
        //// Get only horizontal element of our velocity (none in the direction of our Up vector)
        //var currVelocity = RBody.velocity;
        //var newVelocity = currVelocity - WorldUp * Vector3.Dot(currVelocity, WorldUp);

        //// Add velocity upward by the initial jump strength
        //newVelocity += WorldUp * InitialJumpVelocity;

        //// We want to add the velocity of the surface we're currently on
        //// This allows us to get an extra boost from jumping off moving objects (e.g. platforms moving upwards)
        //newVelocity += new Vector3(WorldUp.x * VelocityOfGround.x, WorldUp.y * VelocityOfGround.y, WorldUp.z * VelocityOfGround.z);

        //// Set the velocity
        //RBody.velocity = newVelocity;

        RBody.AddForce(WorldUp * InitialJumpVelocity,ForceMode.Impulse);

        // We're no longer on the ground
        OnGround = false;

        // We're now jumping (used for the additive jump)
        Jumping       = true;
        InAirFromJump = true;

        // Set the additive jump timer to 0
        JumpTimer = 0.0f;
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

      if (RBody.velocity.x > MaxSpeed)
      {
        ClampSpeed.x = MaxSpeed;
      }
      //if (RBody.velocity.y > MaxSpeed)
      //{
      //  ClampSpeed.y = MaxSpeed;
      //}
      if (RBody.velocity.z > MaxSpeed)
      {
        ClampSpeed.z = MaxSpeed;
      }

      if (RBody.velocity.x < -MaxSpeed)
      {
          ClampSpeed.x = -MaxSpeed;
      }
      //if (RBody.velocity.y < -MaxSpeed)
      //{
      //    ClampSpeed.y = -MaxSpeed;
      //}
      if (RBody.velocity.z < -MaxSpeed)
      {
          ClampSpeed.z = -MaxSpeed;
      }

      RBody.velocity = ClampSpeed;
    }
}

