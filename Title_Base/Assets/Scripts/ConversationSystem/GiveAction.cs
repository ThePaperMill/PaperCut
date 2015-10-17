using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;


namespace Assets.Scripts.ConversationSystem
{
    public class GiveAction : ConversationAction
    {
        public String Text;
        private StringEvent StringEventData = new StringEvent();
        public override void Start()
        {
            base.Start();
            
            
            
        }

        public override void StartAction()
        {
            StringEventData.Message = Text;
            EventSystem.GlobalHandler.DispatchEvent(Events.RecievedItem, StringEventData);
            
            
        }

        

        // Update is called once per frame
        void Update()
        {
            
        }

        

    }

    
}
