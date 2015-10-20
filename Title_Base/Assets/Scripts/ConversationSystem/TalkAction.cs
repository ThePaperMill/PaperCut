using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


namespace Assets.Scripts.ConversationSystem
{
    public class TalkAction : ConversationAction
    {
        public String Text;
        private StringEvent StringEventData = new StringEvent();
        public override void Start()
        {
            base.Start();
            
            EventSystem.EventConnect(this, Events.DefaultEvent, SayHi);
            EventSystem.EventSend(this, Events.DefaultEvent, new StringEvent("No one saw it coming, Josh."));
            EventSystem.EventDisconnect(this, Events.DefaultEvent, SayHi);
            //this.DispatchEvent(Events.DefaultEvent);
            
            
        }

        public override void StartAction()
        {
            StringEventData.Message = Text;
            EventSystem.GlobalHandler.DispatchEvent(Events.UpdateText, StringEventData);

        }

        // Update is called once per frame
        void Update()
        {
            if (!Active)
            {
                return;
            }
            if (InputManager.GetSingleton.IsKeyTriggered(KeyCode.Space))
            {
                Convo.NextAction();

            }
        }


        void SayHi(EventData eventData)
        {
            Debug.Log((eventData as StringEvent).Message);
            this.gameObject.Destroy();
        }

    }

    public class StringEvent : EventData
    {
        public String Message;
        public StringEvent(String message = "")
        {
            Message = message;
        }
    }

    
}
