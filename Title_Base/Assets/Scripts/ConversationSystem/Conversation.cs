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
        
        // Use this for initialization
        void Start()
        {
            
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
            CurrentNode.Current.StartAction();
        }

        public void NextAction()
        {
            if (Actions.Count == 0)
            {
                return;
            }
            CurrentNode.Current.StopAction();
            if (!CurrentNode.MoveNext())
            {
                Disengage();
                return;
            }
            CurrentNode.Current.StartAction();
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
            if(InputManager.GetSingleton.IsKeyTriggered(KeyCode.P))
            {
                if(!Engaged)
                {
                    Engage();
                }
                else
                {
                    NextAction();
                }
                
            }

        }
    }
}
