﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace Assets.Scripts.ConversationSystem
{
    public class TalkAction : ConversationAction
    {
        [Multiline]
        public String Text;
        private StringEvent StringEventData = new StringEvent();

        public ConversationAction NextAction;

        public override void Start()
        {
            base.Start();           
        }

        public override void StartAction()
        {
            base.StartAction();
            Next = NextAction;
            StringEventData.Message = Text.ToString();
            EventSystem.GlobalHandler.DispatchEvent(Events.UpdateText, StringEventData);
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        

    }

    public class StringEvent : EventData
    {
        public String Message;
        public StringEvent(String message = "")
        {
            Message = message;
        }

        public static implicit operator String (StringEvent eventData)
        {
            return eventData.Message;
        }
    }
  public class TextAreaScript : MonoBehaviour
  {

    public string longString;
  }
}


