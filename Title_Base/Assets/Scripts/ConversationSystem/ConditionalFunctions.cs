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

namespace Assets.Scripts.ConversationSystem
{
    static class ConditionalFunctions
    {
        static public bool DefaultFunctionTrue()
        {
            return true;
        }
        static public bool DefaultFunctionFalse()
        {
            return false;
        }

        static private bool Happened = false;
        static public bool HappenOnceFunc()
        {
            if(!Happened)
            {
                Happened = true;
                return true;
            }
            return false;
        }

        static public bool HappenRandomly(double Param1, double Param2, double Param3)
        {
            return UnityEngine.Random.Range(0f, 100f) >= Param1;
        }

        static private bool HasTalkedToDuck = false;
        static public bool SetHasTalkedToDuckFunc()
        {
            if (!HasTalkedToDuck)
            {
                HasTalkedToDuck = true;
                return true;
            }
            return false;
        }

        static public bool HasTalkedToDuckFunc()
        {
            return HasTalkedToDuck;
        }
    }
}
