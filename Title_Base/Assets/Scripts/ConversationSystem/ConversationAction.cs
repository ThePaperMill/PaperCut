using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace Assets.Scripts.ConversationSystem
{
    public class ConversationAction : EventHandler
    {
        protected Conversation Convo;
        protected bool Active = false;

        public GameObject Next;
        public virtual void Start()
        {
            Convo = this.gameObject.transform.parent.GetComponent<Conversation>();
            if(!Convo)
            {
                Debug.Log("Conversation on object " + this.gameObject.transform.parent.name + " must have a 'Conversation' Component.");
                return;
            }
            
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
