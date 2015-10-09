using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
namespace Assets.Scripts.ConversationSystem
{
    public class ConversationAction : MonoBehaviour
    {
        Conversation Convo;
        void Start()
        {
            Convo = this.gameObject.GetComponent<Conversation>();
            if(!Convo)
            {
                Debug.Log("Conversation on object " + this.gameObject.name + "must have a 'Conversation' Component.");
                return;
            }
            Convo.Actions.Add(this);
        }

        public virtual void StartAction()
        {

        }

        // Update is called once per frame
        void Update()
        {
            
        }
    }

    public class TalkAction : ConversationAction
    {
        public String Text;
        void Start()
        {

        }

        public override void StartAction()
        {
            UITextManager.ConversationText.UpdateText(Text);
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
