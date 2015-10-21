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
    public static readonly String RequestItem = "RequestItemEvent";
    public static readonly String RecievedProperItem = "RecievedProperItemEvent";
    public static readonly String RecievedItem = "RecievedItemEvent";
}

