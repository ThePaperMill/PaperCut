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

using UnityEngine;


namespace Assets.Scripts.ConversationSystem
{
    public class SplitActionParam : ConversationAction
    {
        public String FunctionName = "DefaultFunctionTrue";
        public double Param1 = 0;
        public double Param2 = 0;
        public double Param3 = 0;

        public ConversationAction NextIfTrue;
        public ConversationAction NextIfFalse;

        public override void Start()
        {
            base.Start();
            
            
            
        }

        public override void StartAction()
        {
            var type = typeof(ConditionalFunctions);
            MethodInfo methodInfo = null;
            var infoArray = type.GetMethods();

            foreach (var i in infoArray)
            {
                if (i.Name == FunctionName)
                {
                    methodInfo = i;
                    break;
                }
            }
            
            if(methodInfo == null)
            {
                Debug.Log("Could not find method: " + FunctionName);
                Debug.Log("Ensure that it is a member of the 'ConditionalFunctions' class.");
                return;
            }
            if(!methodInfo.IsStatic || methodInfo.ReturnType != typeof(bool))
            {
                Debug.Log("The method '" + FunctionName + "' must be public, static, take nothing, and return a bool.");
                return;
            }
            var del = (Func<double, double, double, bool>) Delegate.CreateDelegate(typeof(Func<double, double, double, bool>), methodInfo);
            if(del(Param1, Param2, Param3))
            {
                Next = NextIfTrue;
            }
            else
            {
                Next = NextIfFalse;
            }
            
            this.DispatchEvent(Events.NextAction);
        }

        

        // Update is called once per frame
        void Update()
        {
            
        }

        

    }

    
}
