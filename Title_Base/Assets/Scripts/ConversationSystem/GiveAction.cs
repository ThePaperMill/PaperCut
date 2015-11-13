﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.ConversationSystem
{
    public class GiveAction : ConversationAction
    {
        public new ConversationAction Next;

        public ItemInfo ItemToGive = new ItemInfo();

        private StringEvent StringEventData = new StringEvent();
        public override void Start()
        {
            base.Start();

            // we have to init the item we want to give because of unity 
            ItemToGive.InitializeItem();
        }

        public override void StartAction()
        {
            base.Next = Next;
            RecievedItemEvent test = new RecievedItemEvent(ItemToGive);
            EventSystem.GlobalHandler.DispatchEvent(Events.RecievedItem, test);
            
            this.DispatchEvent(Events.NextAction);
        }

        

        // Update is called once per frame
        void Update()
        {
            
        }

        

    }

    
}
