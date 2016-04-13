/****************************************************************************/
/*!
\file   TalkActionEndingTrigger.cs
\author Steven Gallwas 
\brief  
 
  
    © 2016 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.ConversationSystem
{
    public class TalkActionEndingTrigger : ConversationAction
    {
        [Multiline]
        public String Text;
        private StringEvent StringEventData = new StringEvent();

        public ConversationAction NextAction;

        public override void Start()
        {
            base.Start();
        }

        public override void StartAction()
        {
            // Play a sound if there is one
            AudioSource sounde = gameObject.GetComponent<AudioSource>();

            // Note: FMOD will handle sound elsewhere
            if (sounde != null)
            {
                sounde.Play();
            }

            base.StartAction();
            Next = NextAction;
            StringEventData.Message = Text;
            EventSystem.GlobalHandler.DispatchEvent(Events.UpdateText, StringEventData);

            
            GameInfo.GetSingleton.FinaleReady = true;
        }
    }
}
