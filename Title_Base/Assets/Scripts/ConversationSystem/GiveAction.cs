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
            print("Sending item " + ItemToGive.ItemPrefab.name);

            RecievedItemEvent test = new RecievedItemEvent(ItemToGive);
            EventSystem.GlobalHandler.DispatchEvent(Events.RecievedItem, test);
        }

        

        // Update is called once per frame
        void Update()
        {
            
        }

        

    }

    
}
