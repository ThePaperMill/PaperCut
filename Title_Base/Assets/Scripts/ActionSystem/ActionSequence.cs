/****************************************************************************/
/*!
    \author Joshua Biggs  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ActionSystem.Internal;
using UnityEngine;

namespace ActionSystem
{
    public class ActionSequence : ActionBase
    {
        public bool LoopingSequence { get; set; }
        private List<ActionBase> ActionQueue;
        private ActionBase CurrentAction;
        private int Index = 0;

        public ActionSequence(bool looping = false) : base()
        {
            LoopingSequence = looping;
            ActionQueue = new List<ActionBase>();
        }

        public ActionSequence(List<ActionBase> actionQueue, bool looping = false) : base()
        {
            LoopingSequence = looping;
            ActionQueue = actionQueue;
            CurrentAction = ActionQueue.First();
        }

        public override void Update(double dt)
        {
            
            if (IsPaused() || IsCompleted())
            {
                return;
            }
            if (IsEmpty())
            {
                Completed = true;
                return;
            }

            if (CurrentAction.IsCompleted())
            {
                if (!LoopingSequence)
                {

                    ActionQueue.RemoveAt(0);
                    if (ActionQueue.Count == 0)
                    {
                        Completed = true;
                        return;
                    }
                    CurrentAction = ActionQueue.First();
                }
                else
                {
                    ++Index;
                    if (Index == ActionQueue.Count)
                    {
                        CurrentAction = ActionQueue[0];
                        Index = 0;
                    }
                    CurrentAction = ActionQueue[Index];
                }
                //Restart the action to ensure that the starting value is 
                //equivelent to the last action's ending value.
                CurrentAction.Restart();
            }

            CurrentAction.Update(dt);
        }

        public bool IsEmpty()
        {
            return ActionQueue.Count == 0;
        }

        public void AddAction(ActionBase action)
        {
            Completed = false;
            ActionQueue.Add(action);
            CurrentAction = ActionQueue.First();
        }

        public override void Restart()
        {
            Completed = false;
            Index = 0;
            
            if (ActionQueue.Count > 0)
            {
              CurrentAction = ActionQueue.First();
              while (Index < ActionQueue.Count)
              {
                ActionQueue[Index].Restart();
                ++Index;
              }
            }
            Index = 0;
        }

        public void Clear()
        {
            ActionQueue.Clear();
            Restart();
        }

        ~ActionSequence()
        {
            Clear();
        }
    }
}
