using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.ConversationSystem
{
    public class TalkAction : ConversationAction
    {
        public String Text;

        public override void Start()
        {
            base.Start();
        }

        public override void StartAction()
        {
            UITextManager.ConversationText.UpdateText(Text);

        }

        // Update is called once per frame
        void Update()
        {
            if (!Active)
            {
                return;
            }
            if (InputManager.GetSingleton.IsKeyTriggered(KeyCode.Space))
            {
                Convo.NextAction();

            }
        }
    }
}
