/****************************************************************************/
/*!
\file   Take Action 
\author Joshua Biggs and Steven Gallwas
\brief  
       The request conversation for the scientist.
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;


namespace Assets.Scripts.ConversationSystem
{
    public class TakeAction : ConversationAction
    {
        public String ItemName = "DefaultCube";
        private StringEvent StringEventData = new StringEvent();
        private BoolEvent BoolEventData = new BoolEvent();

        public ConversationAction NextIfTrue;
        public ConversationAction NextIfFalse;
        public ConversationAction NextIfNone;

        public override void Start()
        {
            base.Start();
            if (NextIfTrue == null)
            {
                var next = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("TalkAction")).AddComponent<TalkAction>();
                //next.Text = "Thanks for the \n<b>" + ItemName + "</b>!";
                NextIfTrue = next;
            }
            if (NextIfFalse == null)
            {
                var next = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("TalkAction")).AddComponent<TalkAction>();
                //next.Text = "This isn't an \n<b>" + ItemName + "</b>...";
                NextIfFalse = next;
            }
            if (NextIfNone == null)
            {
                var next = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("TalkAction")).AddComponent<TalkAction>();
                //next.Text = "Come back when you have \na <b>" + ItemName + "</b>.";
                NextIfNone = next;
            }
            
        }

        public override void StartAction()
        {
            StringEventData.Message = ItemName;
            //Ask the inventory to give me an item.
            //this.Connect(Events.RecievedItem, OnRecievedItem);
            EventSystem.GlobalHandler.Connect(Events.RecievedItem, OnRecievedItem);
            //If they have the item
            if (!false)
            {
                EventSystem.GlobalHandler.DispatchEvent(Events.RequestItem, StringEventData);
            }
            //else
            //{
            //    Next = NextIfNone;
            //}
            
            //this.DispatchEvent(Events.RecievedItem);
        }

        void OnRecievedItem(EventData eventData)
        {
            
            //If we recieved the proper item.
            var data = eventData as RecievedItemEvent;
            //if the item is null
            if (data.Info.ItemName == "")
            {
                Next = NextIfNone;
            }
            else
            {
                
                if (data.Info.ItemName == this.ItemName)
                {
                    BoolEventData.IsTrue = true;
                    Next = NextIfTrue;
                }
                else
                {
                    BoolEventData.IsTrue = false;
                    Next = NextIfFalse;
                }
            }
            

            this.Disconnect(Events.RecievedItem, OnRecievedItem);
            //Dispatching whether or not the correct item was recieved.
            EventSystem.GlobalHandler.DispatchEvent(Events.RecievedProperItem, BoolEventData);
            
            this.DispatchEvent(Events.NextAction);
        }

        // Update is called once per frame
        void Update()
        {
            
        }

        

    }

    public class BoolEvent : EventData
    {
        public bool IsTrue;
        public BoolEvent(bool isTrue = true)
        {
            IsTrue = isTrue;
        }

        public static implicit operator bool(BoolEvent eventData)
        {
            return eventData.IsTrue;
        }

    }


}
