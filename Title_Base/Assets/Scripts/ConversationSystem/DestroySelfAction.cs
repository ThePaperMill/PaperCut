using UnityEngine;
using System.Collections;

namespace Assets.Scripts.ConversationSystem
{
    public class DestroySelfAction : ConversationAction
    {
        public override void Start()
        {
            base.Start();
        }

        public override void StartAction()
        {
            gameObject.Destroy();
        }
    }
}
