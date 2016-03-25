using UnityEngine;
using System.Collections;

namespace Assets.Scripts.ConversationSystem
{
    public class PlaceItemAction : ConversationAction
    {
        private BoolEvent BoolEventData = new BoolEvent();

        public ConversationAction NextIfTrue;
        public ConversationAction NextIfFalse;
        public ConversationAction NextIfNone;

        public string TargetItem = "";

        public override void Start()
        {
            base.Start();
        }

        public override void StartAction()
        {
            // Request an item from the player
            RequestItemEvent ItemTest = new RequestItemEvent();
            ItemTest.ItemName = TargetItem;
            ItemTest.Requestor = gameObject;

            EventSystem.GlobalHandler.DispatchEvent(Events.RequestItem, ItemTest);
            gameObject.Connect(Events.RecievedItem, OnRecievedItem);
        }

        void OnRecievedItem(EventData eventData)
        {
            //If we recieved the proper item.
            var data = eventData as RecievedItemEvent;

            //if the item is null
            if (data == null || data.Info.ItemName == "")
            {
                BoolEventData.IsTrue = false;
                Next = NextIfNone;
            }

            else
            {
                if (data.Info.ItemName == TargetItem)
                {
                    BoolEventData.IsTrue = true;
                    Next = NextIfTrue;

                    // create the item here
                    Instantiate(data.Info.ItemPrefab, transform.position, Quaternion.identity);
                }
                else
                {
                    BoolEventData.IsTrue = false;
                    Next = NextIfFalse;
                }
            }

            //Dispatching whether or not the correct item was recieved.
            EventSystem.GlobalHandler.DispatchEvent(Events.RecievedProperItem, BoolEventData);

            this.DispatchEvent(Events.NextAction);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
