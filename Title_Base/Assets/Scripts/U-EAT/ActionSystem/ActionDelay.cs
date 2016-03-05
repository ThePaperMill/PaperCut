/****************************************************************************/
/*!
    \author Joshua Biggs  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using ActionSystem.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ActionSystem
{
    class ActionDelay : ActionBase
    {
        public ActionDelay(double duration = 0) : base()
        {
            EndTime = duration;
        }

        public override void Update(double dt)
        {
            if (IsCompleted() || IsPaused())
            {
                return;
            }

            CurrentTime += dt;
            if (CurrentTime >= EndTime)
            {
                Completed = true;
            }
        }

        public override void Restart()
        {
            Completed = false;
            CurrentTime = 0;
        }

        void Restart(double duration) 
        {
            EndTime = duration;
            Restart();
        }

        public double CurrentTime { get; private set; }
        public double EndTime { get; set; }
    }
}
