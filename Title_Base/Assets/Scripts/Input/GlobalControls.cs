using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

class GlobalControls
{
    public static List<InputCodes> InteractKeys = new List<InputCodes>{ KeyCode.E, XINPUT_BUTTONS.BUTTON_A };
    public static List<InputCodes> OpenInventoryKeys = new List<InputCodes> { KeyCode.I, XINPUT_BUTTONS.BUTTON_BACK };
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

    public InputCodes(XINPUT_BUTTONS key)
    {
        InputType = InputTypes.Controller;
        Value = (int)key;
    }

    public static implicit operator InputCodes(KeyCode a)
    {
        return new InputCodes(a);
    }

    public static implicit operator InputCodes(XINPUT_BUTTONS a)
    {
        return new InputCodes(a);
    }
}

