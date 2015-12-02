/****************************************************************************/
/*!
    \author Joshua Biggs  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ActionSystem
{
    namespace Internal
    {
        public class ActionBase
        {
            //Booleans for different states of the current action.
            protected bool Paused = false;
            protected bool Completed = false;

            public void Pause() { Paused = true; }
            public void Resume() { Paused = false; }
            public bool IsPaused() { return Paused; }
            public bool IsCompleted() { return Completed; }

            //I have Update as a virtual function in order to make adding new action types
            //incredibly simple. I believe the added flexibility heavily outweighs the extra overhead.
            public virtual void Update(double dt) { }
            //Restart is virtual in the case that the user wants to ake their own custom 
            //derived Action class with different restart functionality.
            public virtual void Restart() { Completed = false; }

            
    
            

            //Protected so that this class is always a base class.
            protected ActionBase() { }


};
    }//namespace Internal
} //namespace ActionSystem
