/****************************************************************************/
/*!
\file   CharacterController3d.cs
\author Steven Gallwas
\brief  
    This file contains the immplementation of the character controller.
    this was designed to be used with the mysweptcontroller class.
 
  © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class CharacterController3d : MonoBehaviour
{
    // basic move booleans 
    private bool MoveForward;
    private bool MoveBack;
    private bool MoveLeft;
    private bool MoveRight;
    
    // special buttons for triggering specific actions.
    private bool InteractPressed;
    private bool StartPressed;
    
    // 
    bool MenuActive;

    // boolean for checking if we are grounded or not
    bool Grounded;  
    
    // how strong of a jump do we want.
    public float JumpPower = 200.0f;
    //int MaxJumps = 1;

    public float MoveSpeed = 2.5f;

    //Rigidbody RBody;

    MySweptController SController;
    //CustomDynamicController DController;  

    GameObject Cam = null;

    /****************************************************************************/
    /*!
    \brief
      Initializes the class, stores the sweptcontroller, rigidbody, and camera.

    */
    /****************************************************************************/
    void Start () 
	  {
        // initialize booleans to false
        MoveForward = false;
        MoveBack    = false;
        MoveLeft    = false;
        MoveRight   = false;
        MenuActive  = false;

        
        SController = (MySweptController)GetComponent<MySweptController>();
        //RBody       = (Rigidbody)GetComponent<Rigidbody>();
        Cam         = (GameObject)GameObject.FindGameObjectWithTag("MainCamera");
    }

    /****************************************************************************/
    /*!
    \brief
      Update the class, I'm using fixed update to try and avoid the tunneling
      issues.  
    */
    /****************************************************************************/
	void LateUpdate ()
    {
      // this checks what input has been given this frame
      UpdateInput();
    
      // depending on state call a different update
      if (!MenuActive)
      {
        MovementUpdate();
      }

      else
      {
        MenuUpdate();
      }
    }

    /****************************************************************************/
    /*!
    \brief
      Update function to be called in the main fixedupdate loop.  update the 
      players movement and calls the sweptcontroller update.
    */
    /****************************************************************************/
    void MovementUpdate()
    {
      Vector3 movement = new Vector3(0, 0, 0);

      var LeftStickPosition = InputManager.GetSingleton.GetLeftStickValues();
      
      // check interact / jump first
      if (InteractPressed)
      {
        // first, we'll need to check if an object is in our interact region @Troy's interact system here
        /*
          if(interactable)
          {
            send interact message here, 
            
            // use this to disable movement
            MenuActive = true;
          }
          else
          */

          if(SController != null)
          {
            SController.Jump();
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

      if (SController != null)
      {
        SController.SweepUpdate(movement, Time.fixedDeltaTime);
      }
      
    }

  /****************************************************************************/
  /*!
  \brief
    Called when the menu is open, can be used to control the menu, or just 
    stop the player from updating when the menu is open.

  */
  /****************************************************************************/
  void MenuUpdate()
  {
    // currently nothing, when we have a conversation system, the code could go here
  }

  /****************************************************************************/
  /*!
  \brief
      Checks input and updates the booleans tracking how the character is moving
  */
  /****************************************************************************/
  void UpdateInput()
  {
      MoveForward     = InputManager.GetSingleton.IsButtonDown(XINPUT_BUTTONS.BUTTON_DPAD_UP)    || InputManager.GetSingleton.IsKeyDown(KeyCode.UpArrow);

      MoveBack        = InputManager.GetSingleton.IsButtonDown(XINPUT_BUTTONS.BUTTON_DPAD_DOWN)  || InputManager.GetSingleton.IsKeyDown(KeyCode.DownArrow);

      MoveLeft        = InputManager.GetSingleton.IsButtonDown(XINPUT_BUTTONS.BUTTON_DPAD_LEFT)  || InputManager.GetSingleton.IsKeyDown(KeyCode.LeftArrow);

      MoveRight       = InputManager.GetSingleton.IsButtonDown(XINPUT_BUTTONS.BUTTON_DPAD_RIGHT) || InputManager.GetSingleton.IsKeyDown(KeyCode.RightArrow);

      InteractPressed = (InputManager.GetSingleton.IsButtonTriggered(XINPUT_BUTTONS.BUTTON_A)    || InputManager.GetSingleton.IsKeyTriggered(KeyCode.E));
  }
}
