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
using UnityEngine;

namespace ActionSystem
{
    class ActionDynamicCall: ActionBase
    {
        public ActionDynamicCall(Delegate function) : base()
        {
            Function = function;
        }
        //For member functions
        public ActionDynamicCall(Delegate function, params object[] args) : base()
        {
            if(args.Count() != function.Method.GetParameters().Count())
            {
                throw new Exception("Arguments given do not match the method's arguments");
            }
            Function = function;
            Arguments = args;
        }

        public override void Update(double dt)
        {
            if (IsCompleted() || IsPaused())
            {
                return;
            }

            Return = Function.DynamicInvoke(Arguments);
            Completed = true;
        }
        
        private Delegate Function;
        private object[] Arguments;
        public object Return { get; private set; }
        
    }

    class ActionCall : ActionBase
    {
        public ActionCall(System.Action function) : base()
        {
            Function = function;
        }

        public override void Update(double dt)
        {
            if (IsCompleted() || IsPaused())
            {
                return;
            }
            Function.Invoke();
            Completed = true;
        }

        private System.Action Function;
    }
    class ActionCall<T1> : ActionBase
    {
        public ActionCall(System.Action<T1> function, T1 arg1) : base()
        {
            Function = function;
            Arg1 = arg1;
        }

        public override void Update(double dt)
        {
            if (IsCompleted() || IsPaused())
            {
                return;
            }
            Function.Invoke(Arg1);
            Completed = true;
        }

        private System.Action<T1> Function;
        T1 Arg1;
    }
    class ActionCall<T1, T2> : ActionBase
    {
        public ActionCall(System.Action<T1, T2> function, T1 arg1, T2 arg2) : base()
        {
            Function = function;
            Arg1 = arg1;
            Arg2 = arg2;
        }

        public override void Update(double dt)
        {
            if (IsCompleted() || IsPaused())
            {
                return;
            }
            Function.Invoke(Arg1, Arg2);
            Completed = true;
        }

        private System.Action<T1, T2> Function;
        private T1 Arg1;
        private T2 Arg2;
    }
    class ActionCall<T1, T2, T3> : ActionBase
    {
        public ActionCall(System.Action<T1, T2, T3> function, T1 arg1, T2 arg2, T3 arg3) : base()
        {
            Function = function;
            Arg1 = arg1;
            Arg2 = arg2;
            Arg3 = arg3;
        }

        public override void Update(double dt)
        {
            if (IsCompleted() || IsPaused())
            {
                return;
            }
            Function.Invoke(Arg1, Arg2, Arg3);
            Completed = true;
        }

        private System.Action<T1, T2, T3> Function;
        private T1 Arg1;
        private T2 Arg2;
        private T3 Arg3;
    }
    class ActionCall<T1, T2, T3, T4> : ActionBase
    {
        public ActionCall(System.Action<T1, T2, T3, T4> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4) : base()
        {
            Function = function;
            Arg1 = arg1;
            Arg2 = arg2;
            Arg3 = arg3;
            Arg4 = arg4;
        }

        public override void Update(double dt)
        {
            if (IsCompleted() || IsPaused())
            {
                return;
            }
            Function.Invoke(Arg1, Arg2, Arg3, Arg4);
            Completed = true;
        }

        private System.Action<T1, T2, T3, T4> Function;
        private T1 Arg1;
        private T2 Arg2;
        private T3 Arg3;
        private T4 Arg4;
    }

    class ActionReturnCall<TReturn> : ActionBase
    {
        public ActionReturnCall(Func<TReturn> function) : base()
        {
            Function = function;
        }

        public override void Update(double dt)
        {
            if (IsCompleted() || IsPaused())
            {
                return;
            }
            Return = Function.Invoke();
            Completed = true;
        }

        private Func<TReturn> Function;
        public TReturn Return { get; private set; }
    }
    class ActionReturnCall<T1, TReturn> : ActionBase
    {
        public ActionReturnCall(Func<T1, TReturn> function, T1 arg1) : base()
        {
            Function = function;
            Arg1 = arg1;
        }

        public override void Update(double dt)
        {
            if (IsCompleted() || IsPaused())
            {
                return;
            }
            Return = Function.Invoke(Arg1);
            Completed = true;
        }

        public TReturn Return { get; private set; }
        private Func<T1, TReturn> Function;
        T1 Arg1;
    }
    class ActionReturnCall<T1, T2, TReturn> : ActionBase
    {
        public ActionReturnCall(Func<T1, T2, TReturn> function, T1 arg1, T2 arg2) : base()
        {
            Function = function;
            Arg1 = arg1;
            Arg2 = arg2;
        }

        public override void Update(double dt)
        {
            if (IsCompleted() || IsPaused())
            {
                return;
            }
            Return = Function.Invoke(Arg1, Arg2);
            Completed = true;
        }

        public TReturn Return { get; private set; }
        private Func<T1, T2, TReturn> Function;
        private T1 Arg1;
        private T2 Arg2;
    }
    class ActionReturnCall<T1, T2, T3, TReturn> : ActionBase
    {
        public ActionReturnCall(System.Func<T1, T2, T3, TReturn> function, T1 arg1, T2 arg2, T3 arg3) : base()
        {
            Function = function;
            Arg1 = arg1;
            Arg2 = arg2;
            Arg3 = arg3;
        }

        public override void Update(double dt)
        {
            if (IsCompleted() || IsPaused())
            {
                return;
            }
            Return = Function.Invoke(Arg1, Arg2, Arg3);
            Completed = true;
        }

        private Func<T1, T2, T3, TReturn> Function;
        public TReturn Return { get; private set; }
        private T1 Arg1;
        private T2 Arg2;
        private T3 Arg3;
    }
    class ActionReturnCall<T1, T2, T3, T4, TReturn> : ActionBase
    {
        public ActionReturnCall(System.Func<T1, T2, T3, T4, TReturn> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4) : base()
        {
            Function = function;
            Arg1 = arg1;
            Arg2 = arg2;
            Arg3 = arg3;
            Arg4 = arg4;
        }

        public override void Update(double dt)
        {
            if (IsCompleted() || IsPaused())
            {
                return;
            }
            Return = Function.Invoke(Arg1, Arg2, Arg3, Arg4);
            Completed = true;
        }

        public TReturn Return { get; private set; }
        private Func<T1, T2, T3, T4, TReturn> Function;
        private T1 Arg1;
        private T2 Arg2;
        private T3 Arg3;
        private T4 Arg4;
    }
}
