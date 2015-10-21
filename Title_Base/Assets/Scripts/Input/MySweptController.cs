/****************************************************************************/
/*!
\file   MySweptController.cs
\author Zero Dev Team and Steven Gallwas
\brief  
    This file contains the implementation of sweptcontroller.  I have
    rewritten some of the logic, but this is largely a port of the 
    zero sweptcontroller.
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

/*
  My Goal for this controller is to duplicate the swept controller as 
  implemented by zero dev team.
 
 Usage:
 The user will need to make their own script for input.
 Each Update, the input script should determine the direction of
 desired movement (does not need to be normalized) and pass it to the
 Update function of this script, along with the Dt from the UpdateEvent.

 Modifying the RigidBody velocity will have no affect on the controller.

Character controller is independent of physics update.
The RigidBody is expected to be Kinematic when using this controller.
*/

[RequireComponent(typeof(Rigidbody),typeof(Collider))]
public class MySweptController : MonoBehaviour 
{
/*******************************************************************************************************************/
/***********************************************Public Variables****************************************************/
/*******************************************************************************************************************/       
    // Tell other object we collided with them.
    public bool ForwardEvents = false;
    
    // You should know what gravity is.
    public float Gravity = 10.0f;

    public float PushForce = 2.0f;

    // Instantanious velocity in the WorldUp direction when jump is activated.
    public float JumpPower = 5.0f;

    // this value is used to allow us to jump for brief moment after we start falling
    public float JumpDelayTime = 0.15f;
    
    // The percentage of upward velocity that should be removed
    // when a jump is cancelled.
    // Value expected to be between 0 to 1.
    public float JumpCancelFactor = 0.5f;
    
    // Maximum speed you can accelerate to in the horizontal direction.
    public float MaxMoveSpeed = 10.0f;
    
    // Maximum speed you can fall at.
    public float MaxFallSpeed = 50.0f;
    
    // Increase in movement velocity per second on ground or in air.
    public float GroundAcceleration = 50.0f;
    
    public float AirAcceleration = 10.0f;
    
    // Decrease in movement velocity per second on ground or in air.
    // Prevents side-slipping when changing directions.
    // Adds to acceleration when moving against current velocity.
    public float GroundDeceleration = 50.0f;
    
    public float AirDeceleration = 10.0f;
    
    // Maximum angle, in degrees, that a surface can be to be considered ground
    // based upon the WorldUp vector.
    // Ground can be walked on and jumped off of.
    // A value of 0 means only flat surfaces are walkable.
    // A value near 90 means almost all surface are walkable.
    public float MaxGroundSlopeDegreeAngle = 45.0f;
    
    // Maximum angle, in degrees, that a surface can be to be considered a ceiling
    // based upon the WorldUp vector.
    // Used to prevent collide and slide along a ceiling surface.
    // Stops upward velocity when jumping into the ceiling.
    public float MaxCeilingSlope = 45.0f;
    
    // Maximum distance that the character will be projected down
    // to maintain connection to the ground.
    // Only takes affect when grounded.
    // If moving over sloped surfaces fast enough, the character can sweep
    // far enough to exceed this distance from the ground in a single update,
    // causing the character to become ungrounded.
    public float GroundContactDistance = 0.1f;
    
    // Store the normal vector of the world
    // Used to decompose movement into horizontal and vertical pieces. Used to determine what surfaces are walkable.
    Vector3 WorldUp = new Vector3(0.0f, 1.0f, 0.0f);

    // Is the Character on the ground
    bool Grounded = true;
    
    // Is the Character Jumping
    bool Jumping = false;

/*******************************************************************************************************************/
/***********************************************Private Variables***************************************************/
/*******************************************************************************************************************/  
    // The velocity that reflects the characters intended movement.
    private Vector3 MoveDIR = new Vector3(0,0,0);

    // owners rigid body
    private Rigidbody RBody = null;

    private CapsuleCollider CCollider = null;

    // Scalars for changing the amount of acceleration/deceleration
    // at run time without losing the base values set in the properties.
    // These are reset to 1.0 every frame after computing acceleration/deceleration.
    // Expected modification should occur every frame.
    private float GroundTraction  = 1.0f;
    
    private float AirTraction = 1.0f;
    
    // Used to prevent floating point errors 
    private float Epsilon = 0.001f;
    
    // List of kinematic objects detected during the controller's update.
    // Copied to KinematicContacts at the end of the update.
    private List<GameObject> KinematicPending = new List<GameObject>();
    
    // List of kinematic objects that the character came into contact with.
    // Used to run sweep with the velocities of kinematic objects so that
    // the character can be moved by them.
    private List<GameObject> KinematicContacts = new List<GameObject>();
   
    private int RayLayer = 0;

    /****************************************************************************/
     /*
      * Initialize the class store the rigid body and set to kinematic just in 
      case.
      */ 
    /****************************************************************************/
    void Start()
    {
       // store our rigidbody and collider
        RBody = (Rigidbody) GetComponent<Rigidbody>();

        // Set ourselves to kinematic
        RBody.isKinematic = true;

        CCollider = (CapsuleCollider)GetComponent<CapsuleCollider>();

        RayLayer = 1 << 8;

        // This would cast rays only against colliders in layer 8, so we just inverse the mask.
        RayLayer = ~RayLayer;
    }


/*******************************************************************************************************************/
/***********************************************Jump Functions******************************************************/
/*******************************************************************************************************************/ 
    
    /****************************************************************************
     Jump actions should be called before calling Update for that frame.
     Will only cause the character to jump when grounded.
     User does not need to check for grounded or anything else before calling.
    ****************************************************************************/  
    public void Jump()
    {
        if (Grounded)
        {
            // add jump power in the up direction
            MoveDIR  += WorldUp * JumpPower;
            Grounded = false;
            Jumping  = true;
        }
    }
    
    /****************************************************************************
     jump regardless of grounded status.
    ****************************************************************************/
    public void JumpUnconditionally()
    {
        // Remove any velocity that's currently on the WorldUp axis first
        // and then add jump velocity.
        MoveDIR -= Vector3.Project(MoveDIR, WorldUp);
        MoveDIR += WorldUp * JumpPower;
        Grounded = false;
        Jumping = true;
    }
    
    /****************************************************************************
     Will cause the character to jump unconditionally. Overwrites any previous 
     velocity to the jump velocity plus the additional velocity passed in.
    ****************************************************************************/
    public void JumpDirectionally(Vector3 dir)
    {
        MoveDIR  = dir + WorldUp * JumpPower;
        Grounded = false;
        Jumping  = true;
    }
    
    /****************************************************************************
     Must be called to enable variable jump heights, Can be called whenever a 
     jump button is not down or when the jump should be cancelled.
     If a jump is released while still moving upward, reduce the remaining.
    ****************************************************************************/
    public void JumpCancel()
    {
        // velocity to make a shorter jump. Holding jump longer will make a higher jump.
        if (Jumping && Vector3.Dot(MoveDIR, WorldUp) > 0.0f)
        {
            MoveDIR -= Vector3.Project(MoveDIR, this.WorldUp) * this.JumpCancelFactor;
        }
        this.Jumping = false;
    }
    
/*******************************************************************************************************************/
/***********************************************Update Function*****************************************************/
/*******************************************************************************************************************/ 
    
    /****************************************************************************
     Must be called once per logic update to work correctly If the character controller 
     should not be active for any reason then this should not be called.
    ****************************************************************************/
    public void SweepUpdate(Vector3 dir, float dt)
    {
        // Removes any movement given along the WorldUp axis.
        dir = dir - Vector3.Project(dir, WorldUp);
        
        // User not required to pass a normalized direction.
        dir.Normalize();
        
        // Decompose velocity directions so that movement logic
        // can be independent of jumping/falling.
        Vector3 verticalVelocity   = Vector3.Project(MoveDIR, WorldUp);
        Vector3 horizontalVelocity = MoveDIR - verticalVelocity;
        
        // calculate our ground and air acceleration and deceleration 
        float acceleration = dt;
        float deceleration = dt;

        // depending on our grounded state decide which acceleration to use.
        if (Grounded)
        {
            acceleration *= (GroundAcceleration * GroundTraction);
            deceleration *= (GroundDeceleration * GroundTraction);
        }
        else
        {
            acceleration *= AirAcceleration * AirTraction;
            deceleration *= AirDeceleration * AirTraction;
        }
        
        // Reset traction scalars for next update.
        GroundTraction = 1.0f;
        AirTraction    = 1.0f;
        
        // Get velocity directions relative to input movement.
        // sideVelocity will be all of horizontalVelocity when movement is zero,
        // this will decelerate horizontalVelocity to zero when there is no input movement.
        Vector3 forwardVelocity = Vector3.Project(horizontalVelocity, dir);
        Vector3 sideVelocity    = horizontalVelocity - forwardVelocity;
        
        // Decelerate velocity that is not in the direction of movement.
        // Deceleration amount can only take velocity to zero, not backwards.
        float cappedSideDecel = Mathf.Min(Vector3.Magnitude(sideVelocity), deceleration);
        horizontalVelocity   -= sideVelocity.normalized * cappedSideDecel;
        
        // If movement is against current velocity, apply deceleration to assist movement.
        if (Vector3.Dot(forwardVelocity, dir) < 0.0f)
        {
            var cappedForwardDecel  = Mathf.Min(Vector3.Magnitude(forwardVelocity), deceleration);
            horizontalVelocity     -= forwardVelocity.normalized * cappedForwardDecel;
        }
        else if (Vector3.Magnitude(forwardVelocity) > MaxMoveSpeed)
        {
            //decelerate if we surpass our max speed.
            var cappedForwardDecel  = Mathf.Min(Vector3.Magnitude(forwardVelocity) - MaxMoveSpeed, deceleration);
            horizontalVelocity     -= forwardVelocity.normalized * cappedForwardDecel;
        }
        
        // Accelerate in the direction of movement, only up to max speed.
        // This check is only so that the characters movement cannot accelereate
        // beyond max speed, but other things could cause it to if desired.
        if (Vector3.Magnitude(horizontalVelocity) < MaxMoveSpeed)
        {
            float cappedAccel = Mathf.Min(MaxMoveSpeed - Vector3.Magnitude(horizontalVelocity), acceleration);
            horizontalVelocity += dir * cappedAccel;
        }
        
        if (Grounded)
        {
            // Do not want to accumulate vertical velocity when grounded.
            // Gravity is effectively turned off while grounded.
            verticalVelocity = new Vector3(0.0f,0.0f,0.0f);
        }
        else
        {
            // Apply gravity in opposite direction of WorldUp.
            verticalVelocity -= WorldUp * Gravity * dt;
           
            // This will cap velocity in the downward direction only.
            if (Vector3.Dot(verticalVelocity, WorldUp) < 0.0f)
            {
                float cappedFallSpeed = Mathf.Min(Vector3.Magnitude(verticalVelocity), this.MaxFallSpeed);
                verticalVelocity      = verticalVelocity.normalized * cappedFallSpeed;
            }
        }
        
        // Recompose velocity directions.
        MoveDIR = horizontalVelocity + verticalVelocity;
        
        // Makes sure jumping flag is removed when velocity is not upwards
        // so that an upward jump can be maintained while in contact with the ground.
        // i.e. Jumping into a slope.
        if (Vector3.Dot(MoveDIR, WorldUp) <= 0.0f)
        {
            Jumping = false;
        }

        // Does a "collide and slide" like behavior, starting with move direction
        SweptCollision(MoveDIR, dt, false);

        PenetratingCollisionCheck(dir, MoveDIR.magnitude * dt);

        // Do a sweep for every kinematic object the character is in contact with
        // using the velocity of that object. (for moving platforms and such)
        foreach (GameObject GB in this.KinematicContacts)
        {
            Rigidbody temp = (Rigidbody)GB.GetComponent<Rigidbody>();
            
            if(temp != null)
              this.SweptCollision(temp.velocity, dt, true);
        }
        
        // Done after the sweep to stay in contact with the ground when detected.
        SnapToGround();
        
        // Event and data management for the end of the update.
        SweptCompleted();
    }


    /****************************************************************************
     Each frame update, sweepVelocity starts out as the intended movement of the character (movedir).
     As contact with other geometry is detected during iteration, sweepVelocity is continually modified to
     represent the possible path of motion that is still within the initial direction of motion.
     The ControllerVelocity is only modified when velocity in a particular direction is not
     desired for the following frame updates.
    ****************************************************************************/
    private void SweptCollision(Vector3 sweepVelocity, float timeLeft, bool kinematic)
    {
        // Used to keep track of consecutive contacted surface normals
        // to detect unsolvable configurations that require special handling.
        List<Vector3> normals = new List<Vector3>();

        // Sentinel value, serves no purpose other than removing a conditional statement.
        normals.Add(new Vector3(0,0,0));
        
        // The number of iterations used is arbitrary.
        // Some geometrical configurations can take as much as 10-20 iterations to resolve.
        // Almost always resolves within a few iterations otherwise.
        // 20 iterations was found to behave well through lots of testing.
        for (var iterCount = 0; iterCount < 20; ++iterCount)
        {
            // Used to denote when a collision that can be resolved was found in the sweep.
            var collision = false;
            
            // sweep the rigid body along the the given direction
            var continuousResultRange = RBody.SweepTestAll(sweepVelocity, sweepVelocity.magnitude * timeLeft , QueryTriggerInteraction.Collide);
           
            //Vector3 Top    = transform.position + (WorldUp * (0.5f * CCollider.height));
            //Vector3 Bottom = transform.position - (WorldUp * (0.5f * CCollider.height));

            //var continuousResultRange = Physics.CapsuleCastAll(Bottom, Top, CCollider.radius, sweepVelocity, sweepVelocity.magnitude * timeLeft,RayLayer);

            // selection sort to sort the damn results
            for (int i = 0; i < continuousResultRange.Length; ++i)
            {
              int j = i;
              var current = continuousResultRange[i];

              while ((j > 0) && (continuousResultRange[j - 1].distance > current.distance))
              {
                continuousResultRange[j] = continuousResultRange[j - 1];
                j--;
              }

              continuousResultRange[j] = current; 
            }
            
            foreach (var result in continuousResultRange)
            {
              // Normal of the contacted surface.
              var normal = result.normal;

              // Get the velocity relative to the direction of the contacted surface.
              var relativeVel = -Vector3.Dot(normal, sweepVelocity);

              // Check for separating velocity.
              // Considering near zero relative velocities will waste iterations on numerical error
              // and lock up possible movement for the controller.
              if (relativeVel < this.Epsilon)
              {
                continue;
              }

              if (result.collider.isTrigger)
              {
                var test = result.rigidbody.gameObject.GetComponent<OnCollideGoToLevel>();

                if (test)
                {
                    test.ChangeLevel();
                }
             }
             else
            {
                    Rigidbody temp = result.collider.gameObject.GetComponent<Rigidbody>();

                    if(temp != null)
                    {
                        Vector3 force = sweepVelocity * PushForce;

                        //project out upward force 
                        force = force - Vector3.Project(force, WorldUp);

                        temp.AddForceAtPosition(force, result.point,ForceMode.Impulse);
                    }
            }


              // calculate time of collision
              float sweepTime = 0.0f;
            
              // if we are close to wall, stop moving into the wall 
              if(result.distance < 0.01f || result.distance < 0.0f)
              {
                    sweepTime = 0.0f;
              }
              else
              {
                sweepTime = result.distance / (sweepVelocity.magnitude / timeLeft);

                // Move forward to the first time of impact.
                // A time of 0 is valid, it just wont result in any translation.
                timeLeft -= sweepTime;
                transform.position += sweepVelocity * sweepTime;
              }


              // Determine what kind of surface was contacted.
              bool ground  = IsGroundSurface(normal);
              bool ceiling = IsCeilingSurface(normal);

              // Moving along the ground.
              // This case is for maintaining the controller's horizontal speed
              // while moving over sloped ground surfaces.
              // If that behavior is not desired, then add '&& kinematic'
              // to the condition because this is still needed for kinematic sweeps.
              if (Grounded && ground)
              {
                sweepVelocity = SkewProjection(sweepVelocity, WorldUp, normal);
              }

              // Moving into a wall while grounded.
              else if (Grounded && !ground && !ceiling)
              {
                // Kinematic sweep can have vertical velocity when grounded.
                // Have to project along the wall and maintain verticle speed.
                var verticalSweep = Vector3.Project(sweepVelocity, WorldUp);

                sweepVelocity -= verticalSweep;

                if (kinematic)
                {
                  verticalSweep = SkewProjection(verticalSweep, normal - Vector3.Project(normal, WorldUp), normal);
                }
                else
                {
                  verticalSweep = new Vector3(0, 0, 0);
                }

                // Project out the horizontal motion that's in the direction of the surface.
                Vector3 horizontalNormal = normal - Vector3.Project(normal, WorldUp);
                horizontalNormal = horizontalNormal.normalized;
                sweepVelocity -= Vector3.Project(sweepVelocity, horizontalNormal);
                sweepVelocity += verticalSweep;
             }

              // Jumping upward into the ceiling.
              else if (!Grounded && ceiling && Vector3.Dot(sweepVelocity, WorldUp) > 0.0)
              {
                // Remove vertical velocity for sweep.
                sweepVelocity -= Vector3.Project(sweepVelocity, WorldUp);

                // Remove vertical velocity when we hit the ceiling
                if (!kinematic && Vector3.Dot(MoveDIR, this.WorldUp) > 0.0)
                {
                  MoveDIR -= Vector3.Project(MoveDIR, this.WorldUp);
                }
              }

              // Falling onto the ground.
              // If moving up a slope fast enough and then jumping, contacting the ground can cancel a jump,
              // the check for not jumping is to prevent that and can be removed if desired.
              else if (!kinematic && !this.Grounded && !this.Jumping && ground)
              {
                // Remove vertical velocity only on first impact with ground,
                // the controller does not have vertical velocity when grounded.
                MoveDIR -= Vector3.Project(MoveDIR, this.WorldUp);

                // Continue sweep using the controller's horizontal velocity
                // so that landing on a slope while moving does not cause a large change in velicity.
                sweepVelocity = MoveDIR;

                // Need to set grounded as soon as it happens so that the following
                // iterations behave with the correct conditions,
                // and so that this case is not repeated in the same update.
                this.Grounded = true;
              }

              // All non specific behavior cases.
              else
              {
                // Project out all velocity that's in the direction of the contact surface.
                sweepVelocity -= Vector3.Project(sweepVelocity, normal);
              }

              // When contacting a wall, do not want any velocity into the wall to persist between updates.
              if (!kinematic && !ground && !ceiling)
              {
                // The horizontal component of the resulting sweepVelocity will have been
                // projected out of the contacted wall surface, this can be used for
                // determining if the controllerVelocity should persist in that direction.
                // The persisting vertical component is taken from the ControllerVelocity
                // so that it behaves the same on the ground and in the air.
                var horizontalSweep    = sweepVelocity - Vector3.Project(sweepVelocity, WorldUp);
                var verticalVelocity   = Vector3.Project(MoveDIR, WorldUp);
                var horizontalVelocity = MoveDIR - verticalVelocity;

                // Don't want to take the sweep velocity if it's not in the direction of the controller,
                // otherwise falling down a sloped wall onto the ground will cause it to
                // slide backwards instead of stopping on the ground.
                if (Vector3.Dot(horizontalSweep, horizontalVelocity) > 0.0 || horizontalSweep.magnitude < Epsilon)
                {
                  MoveDIR = horizontalSweep + verticalVelocity;
                }
              }

              // Add surface normal for checking edge cases.
              normals.Add(normal);

              // Get normals from the last two consecutively contacted surfaces.
              // When contacting the very first surface, the initial zero vector
              // that was added will cause the condition to intentionally fail.
              var normal1 = normals[normals.Count - 2];
              var normal2 = normals[normals.Count - 1];

              // Check for acute angle between surfaces.
              // If angle is acute (less than 90 degrees), then sweep will project back and forth
              // between surfaces forever without any progress.
              // Problem must be resolved on the axis created by both surfaces.
              if (Vector3.Dot(normal1, normal2) < -Epsilon)
              {
                // Get axis of plane intersection.
                // Must be normalized to maintain correct velocity magnitudes.
                var slopeAxis = Vector3.Cross(normal1, normal2);
                slopeAxis.Normalize();

                // Kinematic sweep should move along the axis in all cases.
                // Controller sweep, if not grounded, could get in other
                // unsolvable configurations for the character's motion.
                if (kinematic || this.Grounded)
                {
                  sweepVelocity = Vector3.Project(sweepVelocity, slopeAxis);
                }

                // Character is stuck sliding between two walls.
                else
                {
                  // Because the character is not grounded, if AirAcceleration is zero
                  // then the character will be unable to move, or fall, when the slope
                  // is perpendicular to the WorldUp axis.
                  MoveDIR = Vector3.Project(MoveDIR, slopeAxis);
                  sweepVelocity = MoveDIR;

                  // If slope is not perpendicular, setting AirTraction to zero
                  // will force the character to slide down the slope.
                  if (Mathf.Abs(Vector3.Dot(slopeAxis, WorldUp)) > Epsilon)
                  {
                    AirTraction = 0.0f;
                  }
                }
              }

              // Only resolve the first non-separating contact
              collision = true;
              break;
            }

            // If we aren't colliding, move unhindered
            if (!collision)
            {
                // Move by the remaining sweep amount.
                transform.position += sweepVelocity * timeLeft;

                // No more interations to do, sweep is completed.
                break;
            }
        }
    }
    
    private bool PenetratingCollisionCheck(Vector3 movement, float distance)
    {
        movement = movement - Vector3.Project(movement, WorldUp);

        float actualRadius = CCollider.bounds.extents.x;

        Ray DepthTest = new Ray();

        DepthTest.origin = transform.position;
        DepthTest.direction = movement.normalized;
        RaycastHit RayInfo;

        Debug.DrawRay(DepthTest.origin, DepthTest.direction);

        var DepthCheck = Physics.Raycast(DepthTest, out RayInfo, CCollider.radius, RayLayer);

        if (DepthCheck)
        {
            float ActualDistance = RayInfo.distance - actualRadius;

            if (ActualDistance < 0)
            {
                transform.position -= movement.normalized * Mathf.Abs(ActualDistance);
            }
        }

        return DepthCheck;
    }

    // A downward cast for snapping the character to the ground, only done when
    // grounded to stay in contact with the ground, unless moving too fast.
    private void SnapToGround()
    {
        if (Grounded)
        {
            // Assume not grounded anymore and reset flag only if ground is still detected below the character.
            Grounded = false;

            // calculate the Maximum distance allowed to snap in opposite direction of WorldUp.
            Vector3 maxDisplacement = WorldUp * -GroundContactDistance;
            
            // sweeptest down to check what is below us.   
            var continuousResultRange = RBody.SweepTestAll(-WorldUp, maxDisplacement.magnitude);

            // selection sort the results so they are in order of distance
            for (int i = 0; i < continuousResultRange.Length; ++i)
            {
                int j = i;
                var current = continuousResultRange[i];

                while ((j > 0) && (continuousResultRange[j - 1].distance > current.distance))
                {
                    continuousResultRange[j] = continuousResultRange[j - 1];
                    j--;
                }

                continuousResultRange[j] = current;
            }

            foreach (var result in continuousResultRange)
            {
                var normal = result.normal;
                var relativeVel = -Vector3.Dot(normal, maxDisplacement);
                
                // Ignore separating velocity for the same reasons as the regular sweep.
                if (relativeVel < this.Epsilon) 
                { 
                  continue; 
                }
                
                // Skip everything that's not ground.
                // Doesn't matter if something else is hit first because the controller
                // shouldn't unground when on the edge of the ground and a wall slope simultaniously.
                // The allowed distance from the ground is meant to be fairly small anyway.
                if (!this.IsGroundSurface(normal)) 
                { 
                  continue; 
                }

                // calculate the time of collision
                float Time = 0.0f; //result.distance / maxDisplacement.magnitude;

                transform.position += maxDisplacement * Time;
                
                // Reset flag since ground was detected.
                this.Grounded = true;
                
                // First detection with a ground surface is all that's needed.
                break;
            }
        }
    }
    
    // converts degrees to radians.
    float ToDegrees(float Rads)
    {
      return (Rads * 180.0f) / Mathf.PI;
    }

    // Measures angle between suface normal and WorldUp
    // to determine if the surface is ground.
    bool IsGroundSurface(Vector3 normal)
    {
        float cosineOfAngle = Vector3.Dot(normal, WorldUp);
        cosineOfAngle = Mathf.Clamp(cosineOfAngle, -1.0f, 1.0f);
        float angle = Mathf.Acos(cosineOfAngle);
        
       return ToDegrees(angle - Epsilon) <= MaxGroundSlopeDegreeAngle;
    }
    
    // Measures angle between suface normal and negative WorldUp
    // to determine if the surface is a ceiling.
    bool IsCeilingSurface(Vector3 normal)
    {
        float cosineOfAngle = Vector3.Dot(normal, -WorldUp);
        cosineOfAngle = Mathf.Clamp(cosineOfAngle, -1.0f, 1.0f);
        float angle = Mathf.Acos(cosineOfAngle);
        return ToDegrees(angle - this.Epsilon) <= this.MaxCeilingSlope;
    }
    
    // Projects velocity directionally on to the plane defined by the normal.
    // Used to maintain a velocity's length perpendicular to a given axis (direction).
    // Projection is effectively a ray to plane intersection with a plane through the origin.
    Vector3 SkewProjection(Vector3 velocity, Vector3 direction, Vector3 normal)
    {
        var vDotn = Vector3.Dot(velocity, normal);
        var dDotn = Vector3.Dot(direction, normal);
        
        // No intersection if direction and plane are parallel.
        // Will only happen if slope properties are set to meaningless values.
        if (Mathf.Abs(dDotn) < this.Epsilon)
        {
            return new Vector3(0.0f,0.0f,0.0f);
        }
        
        return velocity + direction * -(vDotn / dDotn);
    }

    /****************************************************************************/
    /*!
      \brief  
         Updates the list of kinematic contacts 
    */
    /****************************************************************************/
    void SweptCompleted()
    {      
        this.UpdateKinematicList();
    }

    /****************************************************************************/
    /*!
      \brief  
         Forwards object to kinematic check.
    */
    /****************************************************************************/
    void OnCollisionEnter(Collision collision)
    { 
        this.AddIfKinematic(collision.rigidbody.gameObject);
    }

    /****************************************************************************/
    /*!
      \brief  
        adds kinematic objects to the list that will be resolved next update
        does not allow duplicate entries.
    */
    /****************************************************************************/
    void AddIfKinematic(GameObject GO)
    {
        Rigidbody test = (Rigidbody)GO.GetComponent<Rigidbody>();

        if (test != null && test.isKinematic == true)
        {
            var has = false;
            foreach (var arraycog in this.KinematicPending)
            {
                if (arraycog == GO)
                {
                    has = true;
                    break;
                }
            }
            if (has == false)
            {
                this.KinematicPending.Add(GO);
            }
        }
    }

    /****************************************************************************/
    /*!
    \brief  
      Copies all entries over and clears old list for tracking next update.
    */
    /****************************************************************************/
    void UpdateKinematicList()
    {
        this.KinematicContacts = this.KinematicPending;
        this.KinematicPending.Clear();
    }
}
