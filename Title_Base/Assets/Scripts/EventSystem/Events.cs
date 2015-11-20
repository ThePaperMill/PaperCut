using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


public static class Events
{
    public static readonly String DefaultEvent = "DefaultEvent";
    public static readonly String UpdateEvent = "UpdateEvent";
    public static readonly String KeyEvent = "KeyEvent";
    public static readonly String MouseEvent = "MouseEvent";
    //EventData : Brings down the dialogue window.
    public static readonly String ActivateTextWindow = "ActivateTextWindowEvent";
    
    //EventData : Puts up the dialogue window.
    public static readonly String DeactivateTextWindow = "DeactivateTextWindowEvent";
    
    //StringEvent : Updates the text in the dialogue window.
    public static readonly String UpdateText = "UpdateTextEvent";
    
    //EventData : Engage in conversation with someone.
    public static readonly String EngageConversation = "EngageConversationEvent";
    
    //EventData : Disengage from conversation with someone.
    public static readonly String DisengageConversation = "DisengageConversationEvent";
    public static readonly String NextAction = "NextActionEvent";

    //Inventory Events
    public static readonly String RequestItem        = "RequestItemEvent";
    public static readonly String RecievedProperItem = "RecievedProperItemEvent";
    public static readonly String RecievedItem       = "RecievedItemEvent";
    public static readonly String MoveItem           = "MoveItem";
    public static readonly String ActivateSelector   = "ActivateSelector";
    public static readonly String DeactivateSelector = "DeactivateSelector";
    public static readonly String UpdateItemText     = "UpdateItemText";
    
    // PlayerSpawnerEvent
    public static readonly String AddSpawnPoint      = "AddSpawnPoint";

    public static readonly String Interact  = "InteractEvent";

    public static readonly String WindowActivated = "WindowActivatedEvent";
    public static readonly String ScientistReq    = "ScientistRequestEvent";
    public static readonly String TransformItem   = "TransformItemEvent";

    public static readonly String PauseGameEvent  = "PauseGameEvent";
    public static readonly String ResumeGameEvent = "ResumeGameEvent";

    public static readonly String TabUpdatedEvent = "TabUpdated";

    public static readonly String InitiateQuitEvent = "InitiateQuitEvent";
    public static readonly String CancelQuitEvent   = "CancelQuitEvent";
    public static readonly String OverlayActive     = "OverlayActiveEvent";
    public static readonly String CancelOverlay     = "CancelOverlayEvent";
}

public class IntegerEvent : EventData
{
    public int value;
    public IntegerEvent(int intValue = 0)
    {
        value = intValue;
    }
}

public class FloatEvent : EventData
{
    public float value;
    public FloatEvent(float floatValue = 0.0f)
    {
        value = floatValue;
    }
}