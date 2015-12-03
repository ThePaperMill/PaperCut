﻿/****************************************************************************/
/*!
\file   Global Controls
\author Joshua Biggs
\brief  
	the thing I keep telling everyone to use.
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class GlobalControls
{
    public static List<InputCodes> InteractKeys = new List<InputCodes>{ KeyCode.E, XINPUT_BUTTONS.BUTTON_X, XINPUT_BUTTONS.BUTTON_Y , MOUSE.LEFT};
    public static List<InputCodes> OpenInventoryKeys = new List<InputCodes> { KeyCode.I, XINPUT_BUTTONS.BUTTON_BACK };
    public static List<InputCodes> JumpKeys = new List<InputCodes> { KeyCode.Space, XINPUT_BUTTONS.BUTTON_A};
    public static List<InputCodes> MoveLeft = new List<InputCodes> { KeyCode.LeftArrow, KeyCode.A};
    public static List<InputCodes> MoveRight = new List<InputCodes> { KeyCode.RightArrow, KeyCode.D };
    public static List<InputCodes> TabControls = new List<InputCodes> { XINPUT_BUTTONS.BUTTON_X, KeyCode.E };
    public static List<InputCodes> MoveInventoryLeft = new List<InputCodes> { KeyCode.LeftArrow, KeyCode.A , XINPUT_BUTTONS.BUTTON_DPAD_LEFT };
    public static List<InputCodes> MoveInventoryRight = new List<InputCodes> { KeyCode.RightArrow, KeyCode.D, XINPUT_BUTTONS.BUTTON_DPAD_RIGHT };
    public static List<InputCodes> InventorySelectItem = new List<InputCodes> { KeyCode.Space, XINPUT_BUTTONS.BUTTON_A, XINPUT_BUTTONS.BUTTON_X, KeyCode.E };
}

public enum InputTypes
{
    Mouse,
    Keyboard,
    Controller
}

public struct InputCodes
{
    public InputTypes InputType;
    public int Value;

    public InputCodes(KeyCode key)
    {
        InputType = InputTypes.Keyboard;
        Value = (int)key;
    }

    public InputCodes(XINPUT_BUTTONS button)
    {
        InputType = InputTypes.Controller;
        Value = (int)button;
    }

    public InputCodes(MOUSE button)
    {
        InputType = InputTypes.Mouse;
        Value = (int)button;
    }

    public static implicit operator InputCodes(KeyCode a)
    {
        return new InputCodes(a);
    }

    public static implicit operator InputCodes(XINPUT_BUTTONS a)
    {
        return new InputCodes(a);
    }

    public static implicit operator InputCodes(MOUSE a)
    {
        
        return new InputCodes(a);
    }
}


