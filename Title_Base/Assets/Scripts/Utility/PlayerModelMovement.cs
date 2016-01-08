/****************************************************************************/
/*!
\file   PlayerModelMovement.cs
\author Jerry Nacier
\brief  
    =]

    Deals with the Player model flipping on turning.

    Thanks to all those who died to make this possible.
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;
using ActionSystem;
using Assets.Scripts.ConversationSystem;

public class PlayerModelMovement : MonoBehaviour
{
    // the Player's model
	public GameObject PlayerModel = null;  
    // the var needed to use actions
    ActionGroup grp               = new ActionGroup();
    // The vector3 that is going to be the model facing right
    Vector3 FacingRight           = new Vector3();
    // The vector3 that is going to be the model facing left
    Vector3 FacingLeft            = new Vector3();
    // Boolean used to check whether an action sequence is in the middle of taking place
    bool bInAction = false;
    // Boolean used to check if a Left-Input button is down; helps check if player is moving to the left
    private bool MoveLeft;
    // Boolean used to check if a Right-Input button is down; helps check if player is moving to the right
    private bool MoveRight;
    //var that will be used to put Its grandparent's Prone var into
    public Vector3 AlsoProne = new Vector3();
    // Vec3 var that will access the PlayerModel's euler angles
    Vector3 ModelRotation = new Vector3();
    // Vec3 var that will be used to rotate the model
    Vector3 RotationAngle = new Vector3();
    ActionGroup grp2 = new ActionGroup();

    void Start () 
	{
        // Initialize FacingRight vec3. It holds onto default
        FacingRight = new Vector3(0, 0, 0);
        // Initialize FacingLeft vec3 at 180 on the y-axis(the back of the model). The model will turn 180 degrees
        FacingLeft = new Vector3(0, 180.0f, 0);

       // AlsoProne = new Vector3(0, 0, 0);
        
        //
        ModelRotation = new Vector3 (0, 0, 0);

        RotationAngle = new Vector3(0, Mathf.Rad2Deg * 360, 0);
        
    }

    void Update () 
	{
        /*Added by steven to stop during certain events.*/
        if (GamestateManager.GetSingleton.IsPaused == true || GamestateManager.GetSingleton.CurState == GAME_STATE.GS_CINEMATIC)
        {
            return;
        }

        bool InventoryStatus = InventorySystem.GetSingleton.isInventoryOpen();

        // if the inventory is open and we press the inventory button, close it ignore other input 
        if (InventoryStatus)
        {
            return;
        }

        if(UITextManager.ConversationText && UITextManager.ConversationText.WindowActive)
        {
            return;
        }

        AlsoProne = transform.parent.transform.parent.GetComponent<HoverSpin>().Prone;
        this.transform.localEulerAngles = new Vector3(AlsoProne.x, this.transform.localEulerAngles.y, this.transform.localEulerAngles.z);
        if (AlsoProne.x == 90.0f)
        {
            Spin();
        }
        else
        {
            // MoveLeft boolean is equal to any down input that makes the player move left: gamepad, arrow keys, WASD 
            MoveLeft = InputManager.GetSingleton.IsButtonDown(XINPUT_BUTTONS.BUTTON_DPAD_LEFT) || InputManager.GetSingleton.IsKeyDown(KeyCode.LeftArrow) || InputManager.GetSingleton.IsKeyDown(KeyCode.A);
            // MoveLeft boolean is equal to any down input that makes the player move right: gamepad, arrow keys, WASD  
            MoveRight = InputManager.GetSingleton.IsButtonDown(XINPUT_BUTTONS.BUTTON_DPAD_RIGHT) || InputManager.GetSingleton.IsKeyDown(KeyCode.RightArrow) || InputManager.GetSingleton.IsKeyDown(KeyCode.D);
            // If there isn't an action already currently taking place...
            if (bInAction == false)
            {
                // Struct with two floats is equal to the left stick's value. Used in how far the player tilts the left stick
                var LeftStickPosition = InputManager.GetSingleton.GetLeftStickValues();
                // If the MoveLeft boolean is true or the left stick is tilted enough...
                if (MoveLeft || LeftStickPosition.XPos < -0.2)
                {
                    // bInAction is now true
                    bInAction = true;
                    // Create an Action Sequence var
                    var seq = ActionSystem.Action.Sequence(grp);
                    // Interpolate the object (that this script is attached to)'s current localEulerAngles to the FacingRight vec3, over the coure of 0.25 secs, by way of QuadOut
                    Action.Property(seq, this.transform.GetProperty(x => x.localEulerAngles), FacingRight, 0.25f, Ease.QuadOut);
                    // Call the action sequence. Pass in the sequence, the SetInAction function with a parameter of false
                    Action.Call(seq, SetInAction, false);
                }
                // Or If the MoveRight boolean is true or the left stick is tilted enough...
                else if (MoveRight || LeftStickPosition.XPos > 0.2)
                {
                    // bInAction is now true
                    bInAction = true;
                    // Create an Action Sequence var
                    var seq = ActionSystem.Action.Sequence(grp);
                    // Interpolate the object (that this script is attached to)'s current localEulerAngles to the FacingLeft vec3, over the coure of 0.25 secs, by way of QuadOut
                    Action.Property(seq, this.transform.GetProperty(x => x.localEulerAngles), FacingLeft, 0.25f, Ease.QuadOut);
                    Action.Call(seq, SetInAction, false);
                }
            }
        }
        // Call update on the action group. This is done because Unity does not have a built-in action system
        grp.Update(Time.smoothDeltaTime);

    }

    // This function makes bInAction equal to the boolean var passed in (bInAction_)
    void SetInAction(bool bInAction_)
    {
        bInAction = bInAction_;
    }

    void Spin()
    {
        this.transform.localEulerAngles += new Vector3(0, 1800, 0) * Time.smoothDeltaTime;
        /*print(PlayerModel.transform.localEulerAngles);
        ModelRotation = PlayerModel.transform.localEulerAngles;

        print(ModelRotation);
        ModelRotation += RotationAngle;

        print(ModelRotation += PlayerModel.transform.localEulerAngles);
        PlayerModel.transform.rotation = Quaternion.Euler(ModelRotation);*/
        //var seq = ActionSystem.Action.Sequence(grp2);

        //grp2.Update(Time.smoothDeltaTime);
    }
}
