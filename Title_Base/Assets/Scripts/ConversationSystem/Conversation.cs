using ActionSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace Assets.Scripts.ConversationSystem
{

    public class Conversation : MonoBehaviour
    { 
        public List<ConversationAction> Actions = new List<ConversationAction>();
        private bool Engaged = false;
        private List<ConversationAction>.Enumerator CurrentNode;
        private ConversationAction CurrentAction;
        public System.Action Test;
        
        // Use this for initialization
        void Start()
        {
            this.Connect(Events.EngageConversation, OnEngageConversation);
            this.Connect(Events.DisengageConversation, OnDisengageConversation);
        }

        public void OnEngageConversation(EventData eventData)
        {
            Engage();
        }
        public void OnDisengageConversation(EventData eventData)
        {
            Disengage();
        }

        public void Engage()
        {
            if(Actions.Count == 0)
            {
                return;
            }

            CurrentNode = Actions.GetEnumerator();
            CurrentNode.MoveNext();
            EventSystem.GlobalHandler.DispatchEvent(Events.ActivateTextWindow);
            Engaged = true;
            CurrentAction = CurrentNode.Current;
            CurrentAction.Connect(Events.NextAction, OnNextAction);
            CurrentAction.StartAction();
        }

        void OnNextAction(EventData data)
        {
            NextAction();
        }

        public void NextAction()
        {
            if (Actions.Count == 0)
            {
                return;
            }
            if (CurrentAction != null)
            {
                CurrentAction.StopAction();
                CurrentAction.Disconnect(Events.NextAction, OnNextAction);
                if (CurrentAction.Next != null)
                {
                    CurrentAction = CurrentAction.Next;
                }
                else
                {
                    if (!CurrentNode.MoveNext())
                    {
                        Disengage();
                        return;
                    }
                    CurrentAction = CurrentNode.Current;
                }
            }
            else
            {
                if (!CurrentNode.MoveNext())
                {
                    Disengage();
                    return;
                }
                CurrentAction = CurrentNode.Current;
            }

            CurrentAction.Connect(Events.NextAction, OnNextAction);
            CurrentAction.StartAction();

        }

        public void Disengage()
        {
            if (Actions.Count == 0)
            {
                return;
            }
            EventSystem.GlobalHandler.DispatchEvent(Events.DeactivateTextWindow);
            Engaged = false;
            CurrentNode = Actions.GetEnumerator();
            CurrentNode.MoveNext();
            CurrentAction = CurrentNode.Current;
        }

        public void PreviousAction()
        {
            if (Actions.Count == 0)
            {
                return;
            }
        }


        // Update is called once per frame
        void Update()
        {
            if(InputManager.GetSingleton.IsKeyTriggered(KeyCode.P) && !Engaged)
            {
                Engage();

            }
            else if (ConversationAction.MoveNextInputRecieved() && Engaged)
            {
                this.NextAction();
            }

        }
    }
}
