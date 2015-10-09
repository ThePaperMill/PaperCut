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

        private int CurrentIndex = 0;
        // Use this for initialization
        void Start()
        {
            
        }

        void Engage()
        {
            CurrentIndex = 0;
            Actions[CurrentIndex].StartAction();
        }

        void NextAction()
        {
            ++CurrentIndex;

        }

        void PreviousAction()
        {

        }


        // Update is called once per frame
        void Update()
        {
            
        }
    }
}
