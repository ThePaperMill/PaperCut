using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace Assets.Scripts.ConversationSystem
{
    public class ConversationAction : EventHandler
    {
        
        protected bool Active = false;
        
        public ConversationAction Next {get; protected set; }

        static public bool MoveNextInputRecieved()
        {
            var input = InputManager.GetSingleton;

            return input.IsInputTriggered(GlobalControls.InteractKeys);
        }

        public virtual void Start()
        {
            //Convo = this.gameObject.transform.parent.GetComponent<Conversation>();
            //if(!Convo)
            //{
            //    Debug.Log("Conversation on object " + this.gameObject.transform.parent.name + " must have a 'Conversation' Component.");
            //    return;
            //}
            
        }

        public virtual void StartAction()
        {
            Active = true;
        }

        public virtual void StopAction()
        {
            Active = false;
        }
        // Update is called once per frame
        void Update()
        {
            
        }
    }

    
}
