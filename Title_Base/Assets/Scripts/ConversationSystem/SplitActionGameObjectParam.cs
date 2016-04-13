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
    public class SplitActionGameObjectParam : ConversationAction
    {
        public string FunctionName = "DefaultFunctionTrue";
        public GameObject Param1;

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
                Debug.Log("The method '" + FunctionName + "' must be public, static, take a GameObject, and return a bool.");
                return;
            }
            var del = (Func<GameObject, bool>) Delegate.CreateDelegate(typeof(Func<GameObject, bool>), methodInfo);

            if(del(Param1))
            {
                Next = NextIfTrue;
            }
            else
            {
                Next = NextIfFalse;
            }
            
            this.DispatchEvent(Events.NextAction);
        }

        

    }

    
}
