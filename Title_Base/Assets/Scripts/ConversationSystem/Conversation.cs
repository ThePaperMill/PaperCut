/****************************************************************************/
/*!
\file   Conversation.cs
\author Joshua Biggs
\brief  
       The base conversation class.
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using ActionSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace Assets.Scripts.ConversationSystem
{

    public class Conversation : EventHandler
    { 
        public List<ConversationAction> Actions = new List<ConversationAction>();
        private bool Engaged = false;
        private List<ConversationAction>.Enumerator CurrentNode;
        private ConversationAction CurrentAction;
        public System.Action Test;

        private GameObject ConversationWindow;
        //public double WindowEaseDuration = 1.5;
        //public Vector3 WindowOffsetInitial = new Vector3(0, 5, 0);
        //public Vector3 WindowOffsetFinal = new Vector3(0, 1, 0);
        //public Vector3 WindowRotation = new Vector3(0, 0, 0);
		private FMOD_StudioEventEmitter SoundClip = null;
		//private FMOD_StudioEventEmitter DialogueSFX = null;
        public float AudioDelay = 0.5f;

        private ActionSequence seq = new ActionSequence();
        private ActionSequence Testseq = new ActionSequence();

        private Vector3 StartScale = new Vector3();

        public bool UseSquidgy = true;
        public int SquigyCount = 2;


        // Use this for initialization
        void Start()
        {
            this.gameObject.Connect(Events.EngageConversation, OnEngageConversation);
            this.gameObject.Connect(Events.DisengageConversation, OnDisengageConversation);
            
            if (CurrentAction)
            {
              CurrentAction.Disconnect(Events.NextAction, OnNextAction);
            }
            
			SoundClip = (FMOD_StudioEventEmitter)GetComponent<FMOD_StudioEventEmitter>();
			//DialogueSFX = gameObject.transform.Find("ExtraSFX").gameObject.GetComponent<FMOD_StudioEventEmitter>();

            EventSystem.GlobalHandler.Connect(Events.NextAction, OnNextAction);

            StartScale = transform.localScale;
        }

        void OnDestroy()
        {
          this.gameObject.Disconnect(Events.EngageConversation, OnEngageConversation);
          this.gameObject.Disconnect(Events.DisengageConversation, OnDisengageConversation);

          EventSystem.GlobalHandler.Disconnect(Events.NextAction, OnNextAction);
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
            if(Actions.Count == 0 || Engaged)
            {
                return;
            }

            if (UseSquidgy)
            {
                Squidgy();
            }

            if (ConversationWindow == null)
            {
                ConversationWindow = UITextManager.ConversationText.gameObject;
                //    ConversationWindow = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("TextBackgroundTest"));
                //    ConversationWindow.transform.position = gameObject.transform.position + WindowOffsetInitial;
                //    ConversationWindow.transform.Rotate(WindowRotation);
                //    var comp = ConversationWindow.GetComponent<UITextManager>();
                //    comp.FinalPos = gameObject.transform.position + WindowOffsetFinal;
                //    comp.Connect();
                //    comp.EaseTime = WindowEaseDuration;
            }
            
            if (!CurrentAction)
            {
                CurrentNode = Actions.GetEnumerator();
                CurrentNode.MoveNext();
                CurrentAction = CurrentNode.Current;
            }
            EventSystem.GlobalHandler.DispatchEvent(Events.ActivateTextWindow);
            Engaged = true;
            CurrentAction = CurrentNode.Current;
            if(CurrentAction == null)
            {
                Debug.Log("Please set a start to the conversation on object: " + gameObject.name);
                return;
            }
            CurrentAction.Connect(Events.NextAction, OnNextAction);
            //CurrentAction.StartAction();

            seq.Clear();
            ActionSystem.Action.Delay(seq, AudioDelay);
            ActionSystem.Action.Call(seq, StartFirstAction);
            ActionSystem.Action.Call(seq, PlaySound);
        }

        

        void OnNextAction(EventData data)
        {
            if (Engaged)
			{
                if(ConversationWindow)
                {
                    var test = ConversationWindow.GetComponent<ScrollingText>();

                    if(test && test.TextStatus == TEXT_STATUS.TEXT_IN_PROGRESS)
                    {
                        test.SkipToEnd();
                        return;
                    }
                }

                if (UseSquidgy)
                {
                    Squidgy();
                }

                NextAction();
            }
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

					//ActionSystem.Action.Call(seq, PlaySound);
                }
                else
                {
                    Disengage();
                    if (!CurrentNode.MoveNext())
                    {
                        CurrentAction = null;
                    }
                                     
                    return;
                }
            }
            else
            {
                Disengage();
                if (!CurrentNode.MoveNext())
                {
                    CurrentAction = null;
                }
            }

            CurrentAction.Connect(Events.NextAction, OnNextAction);
            CurrentAction.StartAction();
        }

        public void Disengage()
        {
            //Debug.Log("DSADA");
            if (Actions.Count == 0)
            {
                return;
            }
            EventSystem.GlobalHandler.DispatchEvent(Events.DeactivateTextWindow);
            Engaged = false;
            //CurrentNode = Actions.GetEnumerator();
            
            CurrentAction = CurrentNode.Current;
            ConversationWindow = null;
        }

        public void PreviousAction()
        {
            if (Actions.Count == 0)
            {
                return;
            }
        }

        public void StartFirstAction()
        {
            if(CurrentAction)
                CurrentAction.StartAction();
        }

        public void PlaySound()
        {
			if(SoundClip != null && GlobalSoundInitializer.GetSingleton.FmodSoundInitialzied == true)
			{
				SoundClip.StartEvent();
			}
		}

        /*public void PlayDialogueSFX()
		{
			DialogueSFX.StartEvent();
		}*/

        public void Squidgy()
        {
           Vector3 ModifiedScale = new Vector3(1.25f * StartScale.x, 0.9f * StartScale.y, StartScale.z);

            for (int i = 0; i < SquigyCount; ++i)
            {
                ActionSystem.Action.Property(Testseq, gameObject.transform.GetProperty(o => o.localScale), ModifiedScale, 0.05f, Ease.Linear);
                ActionSystem.Action.Delay(Testseq, 0.05f);
                ActionSystem.Action.Property(Testseq, gameObject.transform.GetProperty(o => o.localScale), StartScale, 0.05f, Ease.Linear);
            }
        }

        // Update is called once per frame
        void Update()
        {
            seq.Update(Time.deltaTime);
            Testseq.Update(Time.deltaTime);
            //if (ConversationAction.MoveNextInputRecieved() && Engaged)
            //{
            //    this.NextAction();
            //}
        }
    }
}