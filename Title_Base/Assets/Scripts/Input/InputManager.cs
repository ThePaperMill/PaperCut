﻿/****************************************************************************/
/*!
\file   InputManager.cs
\author Steven Gallwas
\brief  
    This file contains the implementation of the input manager class
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;
using XInputDotNetPure; // Required in C#
using System.Collections.Generic;

public enum XINPUT_BUTTONS
{
    BUTTON_X,
    BUTTON_Y,
    BUTTON_A,
    BUTTON_B,
    BUTTON_DPAD_UP,
    BUTTON_DPAD_DOWN,
    BUTTON_DPAD_LEFT,
    BUTTON_DPAD_RIGHT,
    BUTTON_LEFT_SHOULDER,
    BUTTON_RIGHT_SHOULDER,
    BUTTON_START,
    BUTTON_BACK,
    BUTTON_LEFT_STICK,
    BUTTON_RIGHT_STICK,
    BUTTON_TOTAL
};

public struct Gamepad_Stick_Values
{
    public 
    float XPos;

    public
    float YPos;
}

public struct Gamepad_Trigger_Values
{
    public
    float LeftTrigger;

    public
    float RightTrigger;
}


public class InputManager : Singleton<InputManager> //MonoBehaviour
{
    // list of buttonstates so we can determine different states.
    List<ButtonState> ButtonsPast;
    List<ButtonState> ButtonsPresent;

    // we use this to determine when triggers are far enough pressed to consider triggered
    private float TriggerValue = 0.3f;

    bool playerIndexSet = false;
  
    PlayerIndex playerIndex;
  
    GamePadState state;
  
    GamePadState prevState;
    
    public
    int ButtonTotal;

    InputManager()
    {
      ButtonTotal = (int)XINPUT_BUTTONS.BUTTON_TOTAL;

      ButtonsPast = new List<ButtonState>();
      ButtonsPresent = new List<ButtonState>();

      for (int i = 0; i < ButtonTotal; ++i)
      {
        ButtonsPast.Add(new ButtonState());
        ButtonsPresent.Add(new ButtonState());
      }
    }

    /*************************************************************************/
    /*!
      \brief
        Initializes the class nothing to initialize 
    */
    /*************************************************************************/
    void Start ()
    {
        InventorySystem.GetSingleton.ClearInventory();
    }

    /*************************************************************************/
    /*!
      \brief
        Updates the class getting updated input each frame.
    */
    /*************************************************************************/
    void Update ()
    {
        // Find a PlayerIndex, for a single player game we can expand this later if we need additional input for some reason
        if (!playerIndexSet || !prevState.IsConnected)
        {
            for (int i = 0; i < 4; ++i)
            {
                PlayerIndex testPlayerIndex = (PlayerIndex)i;
                GamePadState testState = GamePad.GetState(testPlayerIndex);

                if (testState.IsConnected)
                {
                    playerIndex = testPlayerIndex;
                    playerIndexSet = true;
                }
            }
        }

        // main update function 
        UpdateCurrentInput();
    }

    /*************************************************************************/
    /*!
      \brief
        Called each frame, gets the current input.
    */
    /*************************************************************************/
    void UpdateCurrentInput()
    {
        // copy the current buttons into the past buttons
        for (int i = 0; i < ButtonTotal; ++i)
        {
            ButtonsPast[i] = ButtonsPresent[i];
        }
        
        // store the previous state
        prevState = state;

        // get the new state
        state = GamePad.GetState(playerIndex);

        // update the current buttons with the new input. there is probably a better way to do this.
        ButtonsPresent[(int)XINPUT_BUTTONS.BUTTON_A] = state.Buttons.A;
        ButtonsPresent[(int)XINPUT_BUTTONS.BUTTON_B] = state.Buttons.B;
        ButtonsPresent[(int)XINPUT_BUTTONS.BUTTON_X] = state.Buttons.X;
        ButtonsPresent[(int)XINPUT_BUTTONS.BUTTON_Y] = state.Buttons.Y;

        ButtonsPresent[(int)XINPUT_BUTTONS.BUTTON_DPAD_UP]    = state.DPad.Up;
        ButtonsPresent[(int)XINPUT_BUTTONS.BUTTON_DPAD_DOWN]  = state.DPad.Down;
        ButtonsPresent[(int)XINPUT_BUTTONS.BUTTON_DPAD_LEFT]  = state.DPad.Left;
        ButtonsPresent[(int)XINPUT_BUTTONS.BUTTON_DPAD_RIGHT] = state.DPad.Right;

        ButtonsPresent[(int)XINPUT_BUTTONS.BUTTON_LEFT_SHOULDER]  = state.Buttons.LeftShoulder;
        ButtonsPresent[(int)XINPUT_BUTTONS.BUTTON_RIGHT_SHOULDER] = state.Buttons.RightShoulder;

        ButtonsPresent[(int)XINPUT_BUTTONS.BUTTON_START] = state.Buttons.Start;
        ButtonsPresent[(int)XINPUT_BUTTONS.BUTTON_BACK] = state.Buttons.Back;

        ButtonsPresent[(int)XINPUT_BUTTONS.BUTTON_LEFT_STICK] = state.Buttons.LeftStick;
        ButtonsPresent[(int)XINPUT_BUTTONS.BUTTON_RIGHT_STICK] = state.Buttons.RightStick;
    }

    /*************************************************************************/
    /*!
      \brief
        returns true if the given button was triggered this frame only
      
      \param Button
        The button that is being checked.
     
      \Return
       Returns either true or false if the button was pressed this frame
    */
    /*************************************************************************/
    public bool IsButtonTriggered(XINPUT_BUTTONS Button)
    {
        int tempButton = (int)Button;

        return (ButtonsPast[tempButton] == ButtonState.Released && ButtonsPresent[tempButton] == ButtonState.Pressed);
    }

    /*************************************************************************/
    /*!
      \brief
        returns true if the given button is held down true each frame the button
        is down
    */
    /*************************************************************************/
    public bool IsButtonDown(XINPUT_BUTTONS Button)
    {
        int tempButton = (int)Button;

        return ButtonsPresent[tempButton] == ButtonState.Pressed;
    }

    /*************************************************************************/
    /*!
      \brief
        returns true if the given button was released this frame
    */
    /*************************************************************************/
    public bool IsButtonReleased(XINPUT_BUTTONS Button)
    {
        int tempButton = (int)Button;

        return (ButtonsPast[tempButton] == ButtonState.Pressed && ButtonsPresent[tempButton] == ButtonState.Released);
    }

    /*************************************************************************/
    /*!
      \brief
        returns true if the given button is up, true each frame
    */
    /*************************************************************************/
    public bool IsButtonUp(XINPUT_BUTTONS Button)
    {
        int tempButton = (int)Button;

        return (ButtonsPresent[tempButton] == ButtonState.Released);
    }

    /*************************************************************************/
    /*!
      \brief
        returns the x and y values for the left stick
    */
    /*************************************************************************/
    public Gamepad_Stick_Values GetLeftStickValues()
    {
        Gamepad_Stick_Values temp = new Gamepad_Stick_Values();

        temp.XPos = state.ThumbSticks.Left.X;
        temp.YPos = state.ThumbSticks.Left.Y;

        return temp;
    }

    /*************************************************************************/
    /*!
      \brief
        returns the x and y values for the right stick
    */
    /*************************************************************************/
    public Gamepad_Stick_Values GetRightStickValues()
    {
        Gamepad_Stick_Values temp = new Gamepad_Stick_Values();

        temp.XPos = state.ThumbSticks.Right.X;
        temp.YPos = state.ThumbSticks.Right.Y;

        return temp;
    }

    /*************************************************************************/
    /*!
      \brief
        returns the values for the left and right triggers
    */
    /*************************************************************************/
    public Gamepad_Trigger_Values GetTriggerValues()
    {
      Gamepad_Trigger_Values temp = new Gamepad_Trigger_Values();

      temp.LeftTrigger = state.Triggers.Left;
      temp.RightTrigger = state.Triggers.Right;

      return temp;
    }

    /*************************************************************************/
    /*!
      \brief
        returns true if the left trigger is pressed this frame
    */
    /*************************************************************************/
    public bool IsLeftTriggerTriggered()
    {
      return (prevState.Triggers.Left < TriggerValue && state.Triggers.Left > TriggerValue);
    }

    /*************************************************************************/
    /*!
      \brief
        returns true if the right trigger is pressed this frame
    */
    /*************************************************************************/
    public bool IsRightTriggerTriggered()
    {
      return (prevState.Triggers.Right < TriggerValue && state.Triggers.Right > TriggerValue);
    }

    /*************************************************************************/
    /*!
      \brief
        returns true if the left trigger is down true each frame the trigger
        is down
    */
    /*************************************************************************/
    public bool IsLeftTriggerDown()
    {
      return state.Triggers.Left > TriggerValue;
    }

    /*************************************************************************/
    /*!
      \brief
        returns true if the right trigger down, true each frame the trigger
        is down
    */
    /*************************************************************************/
    public bool IsRightTriggerDown()
    {
      return state.Triggers.Right > TriggerValue;
    }

    /*********************************************************************
     Keyboard wrappers 
     *********************************************************************/

    
    public bool IsKeyTriggered(KeyCode Key)
    {
      return Input.GetKeyDown(Key);
    }

    public bool IsKeyDown(KeyCode Key)
    {
      return Input.GetKey(Key);
    }

    public bool IskeyReleased(KeyCode Key)
    {
      return Input.GetKeyUp(Key);
    }
}
