/****************************************************************************/
/*!
\file   Bounce.cs
\author Jerry Nacier
\brief  
    =]

    Makes the player bounce up and downupon input. The player's bounce will not freeze in midair. 

    Thanks to all those who died to make this possible.
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;
using ActionSystem;

public class Bounce : MonoBehaviour
{
    // the var needed to use actions
    ActionSequence seq = new ActionSequence();
    // The highest point in the object's bounce. Default in editor is y = 0.2
    public Vector3 BouncePeak = new Vector3(0, 0, 0);
    // Boolean used to check if a Forward-Input button is down; helps check if player is moving to the forward
    private bool MoveForward;
    // Boolean used to check if a Back-Input button is down; helps check if player is moving to the back
    private bool MoveBack;
    // Boolean used to check if a Left-Input button is down; helps check if player is moving to the left
    private bool MoveLeft;
    // Boolean used to check if a Right-Input button is down; helps check if player is moving to the right
    private bool MoveRight;
    // Boolean used to check if a Forward-Input button is down; helps check if player is moving to the forward
    bool moving = false;
    // Create a var of type CustomDynamicController. This allows us to access the CustomDynamicController's methods and properties
    CustomDynamicController ParentController = null;
    
    void Start ()
    {
        // Initialize the ParentController
        ParentController = this.transform.parent.gameObject.GetComponent<CustomDynamicController>();
        // Initialize seq. Set it's LoopingSequence property to true
        seq.LoopingSequence = true;
        // Call the seq Action and
        Action.Call(seq, ContinueBounce);
        /* Call Action.Property to create the action sequence. The target property to change is the object's localPosition, to BouncePeak, in 0.15 secs, by way of Easing QuadOut
        This takes the object upwards. */
        Action.Property(seq, this.transform.GetProperty(x => x.transform.localPosition), BouncePeak, 0.15f, Ease.QuadOut);
        /* Call Action.Property to create the action sequence. The target property to change is the object's localPosition, to (0,0,0), in 0.15 secs, by way of Easing QuadOut
        This takes the object downwards. */
        Action.Property(seq, this.transform.GetProperty(x => x.transform.localPosition), Vector3.zero, 0.15f, Ease.QuadOut);
    }
	
	void Update ()
    {
        // MoveForward boolean is equal to any down input that makes the player move forward: gamepad, arrow keys, WASD  
        MoveForward = InputManager.GetSingleton.IsButtonDown(XINPUT_BUTTONS.BUTTON_DPAD_UP) || InputManager.GetSingleton.IsKeyDown(KeyCode.UpArrow) || InputManager.GetSingleton.IsKeyDown(KeyCode.W);
        // MoveBack boolean is equal to any down input that makes the player move back: gamepad, arrow keys, WASD  
        MoveBack = InputManager.GetSingleton.IsButtonDown(XINPUT_BUTTONS.BUTTON_DPAD_DOWN) || InputManager.GetSingleton.IsKeyDown(KeyCode.DownArrow) || InputManager.GetSingleton.IsKeyDown(KeyCode.S);
        // MoveLeft boolean is equal to any down input that makes the player move left: gamepad, arrow keys, WASD 
        MoveLeft = InputManager.GetSingleton.IsButtonDown(XINPUT_BUTTONS.BUTTON_DPAD_LEFT) || InputManager.GetSingleton.IsKeyDown(KeyCode.LeftArrow) || InputManager.GetSingleton.IsKeyDown(KeyCode.A);
        // MoveLeft boolean is equal to any down input that makes the player move right: gamepad, arrow keys, WASD  
        MoveRight = InputManager.GetSingleton.IsButtonDown(XINPUT_BUTTONS.BUTTON_DPAD_RIGHT) || InputManager.GetSingleton.IsKeyDown(KeyCode.RightArrow) || InputManager.GetSingleton.IsKeyDown(KeyCode.D);
        // Struct with two floats is equal to the left stick's value. Used in how far the player tilts the left stick
        var LeftStickPosition = InputManager.GetSingleton.GetLeftStickValues();
        // if moving is equal to false/If not-moving is true...
        if(!moving)
        {
            // Call ContinueBounce
            ContinueBounce();
        }
        // Call update on the action sequence. This is done because Unity does not have a built-in action system
        seq.Update(Time.smoothDeltaTime);
    }
    /*
    Makes the player model bounce up and down, but only when the player is grounded. The player's bounce will not freeze in midair. 
    A full bounce will complete after input has stopped
    */
    void ContinueBounce()
    {
        // Struct with two floats is equal to the left stick's value. Used in how far the player tilts the left stick
        var LeftStickPosition = InputManager.GetSingleton.GetLeftStickValues();
        // Make the moving boolean false. explain.
        moving = false;
        // If the player is detects that the player is grounded
        if(ParentController.IsGrounded() == true)
        {
            // If the moving forward button is tilted or the left stick is tilted enough...
            if (MoveForward || LeftStickPosition.YPos > 0.2)
            {
                // Make the moving boolean true
                moving = true;
                // Call Resume on the action sequence
                seq.Resume();
            }
            // Or, if the moving back button is tilted or the left stick is tilted enough...
            else if (MoveBack || LeftStickPosition.YPos < -0.2)
            {
                // Make the moving boolean true
                moving = true;
                // Call Resume on the action sequence
                seq.Resume();
            }
            // Or, if the moving left button is tilted or the left stick is tilted enough...
            if (MoveLeft || LeftStickPosition.XPos < -0.2)
            {
                // Make the moving boolean true
                moving = true;
                // Call Resume on the action sequence
                seq.Resume();
            }
            // Or, if the moving right button is tilted or the left stick is tilted enough...
            else if (MoveRight || LeftStickPosition.XPos > 0.2)
            {
                // Make the moving boolean true
                moving = true;
                // Call Resume on the action sequence
                seq.Resume();
            }
            // If the moving boolean is false
            if(!moving)
            {
                // Call Pause on the sequence
                seq.Pause();
            }
        }
        // Else, pause the sequence
       else
        {
            seq.Pause();
        }
    }
}
