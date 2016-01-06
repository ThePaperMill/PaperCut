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

namespace ActionSystem
{
    //public delegate TResult Function<TResult>();
    //public delegate TResult Function<T1, TResult>(T1 arg1);
    //public delegate TResult Function<T1, T2, TResult>(T1 arg1, T2 arg2);
    //public delegate TResult Function<T1, T2, T3, TResult>(T1 arg1, T2 arg2, T3 arg3);
    //public delegate TResult Function<T1, T2, T3, T4, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4);
    //public delegate TResult Function<T1, T2, T3, T4, T5, TResult>(T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5);

    static class Action
    {

        public static ActionProperty<T> Property<T>(ActionSequence seq, Property<T> startVal, T endVal, double duration, Curve ease)
        {
            var property = new ActionProperty<T>(startVal, endVal, duration, ease);
            seq.AddAction(property);
            return property;
        }

        public static ActionProperty<T> Property<T>(ActionGroup grp, Property<T> startVal, T endVal, double duration, Curve ease)
        {
            var property = new ActionProperty<T>(startVal, endVal, duration, ease);
            grp.AddAction(property);
            return property;
        }

        public static ActionSequence Sequence(ActionSequence seq, bool looping = false)
        {
            var sequence = new ActionSequence(looping);
            seq.AddAction(sequence);
            return sequence;
        }

        public static ActionSequence Sequence(ActionGroup grp, bool looping = false)
        {

            var sequence = new ActionSequence(looping);
            grp.AddAction(sequence);
            return sequence;
        }

        public static ActionGroup Group(ActionSequence seq, bool looping = false)
        {
            var group = new ActionGroup(looping);
            seq.AddAction(group);
            return group;
        }

        public static ActionGroup Group(ActionGroup grp, bool looping = false)
        {
            var group = new ActionGroup(looping);
            grp.AddAction(group);
            return group;
        }

        public static ActionDelay Delay(ActionSequence seq, double delayTime = 0)
        {
            var delay = new ActionDelay(delayTime);
            seq.AddAction(delay);
            return delay;
        }

        public static ActionDelay Delay(ActionGroup grp, double delayTime = 0)
        {
            var delay = new ActionDelay(delayTime);
            grp.AddAction(delay);
            return delay;
        }

        public static ActionDynamicCall Call(ActionSequence seq, Delegate function)
        {
            var call = new ActionDynamicCall(function);
            seq.AddAction(call);
            return call;
        }
        public static void Call(ActionSequence seq, System.Action function)
        {
            var call = new ActionCall(function);
            seq.AddAction(call);
        }
        public static void Call<T1>(ActionSequence seq, System.Action<T1> function, T1 arg1)
        {
            var call = new ActionCall<T1>(function, arg1);
            seq.AddAction(call);
        }
        public static void Call<T1, T2>(ActionSequence seq, System.Action<T1, T2> function, T1 arg1, T2 arg2)
        {
            var call = new ActionCall<T1, T2>(function, arg1, arg2);
            seq.AddAction(call);
        }
        public static void Call<T1, T2, T3>(ActionSequence seq, System.Action<T1, T2, T3> function, T1 arg1, T2 arg2, T3 arg3)
        {
            var call = new ActionCall<T1, T2, T3>(function, arg1, arg2, arg3);
            seq.AddAction(call);
        }
        public static void Call<T1, T2, T3, T4>(ActionSequence seq, System.Action<T1, T2, T3, T4> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            var call = new ActionCall<T1, T2, T3, T4>(function, arg1, arg2, arg3, arg4);
            seq.AddAction(call);
        }

        public static ActionReturnCall<TReturn> ReturnCall<TReturn>(ActionSequence seq, Func<TReturn> function)
        {
            var call = new ActionReturnCall<TReturn>(function);
            seq.AddAction(call);
            return call;
        }
        public static ActionReturnCall<T1, TReturn> ReturnCall<T1, TReturn>(ActionSequence seq, Func<T1, TReturn> function, T1 arg1)
        {
            var call = new ActionReturnCall<T1, TReturn>(function, arg1);
            seq.AddAction(call);
            return call;
        }
        public static ActionReturnCall<T1, T2, TReturn> ReturnCall<T1, T2, TReturn>(ActionSequence seq, Func<T1, T2, TReturn> function, T1 arg1, T2 arg2)
        {
            var call = new ActionReturnCall<T1, T2, TReturn>(function, arg1, arg2);
            seq.AddAction(call);
            return call;
        }
        public static ActionReturnCall<T1, T2, T3, TReturn> ReturnCall<T1, T2, T3, TReturn>(ActionSequence seq, Func<T1, T2, T3, TReturn> function, T1 arg1, T2 arg2, T3 arg3)
        {
            var call = new ActionReturnCall<T1, T2, T3, TReturn>(function, arg1, arg2, arg3);
            seq.AddAction(call);
            return call;
        }
        public static ActionReturnCall<T1, T2, T3, T4, TReturn> ReturnCall<T1, T2, T3, T4, TReturn>(ActionSequence seq, Func<T1, T2, T3, T4, TReturn> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            var call = new ActionReturnCall<T1, T2, T3, T4, TReturn>(function, arg1, arg2, arg3, arg4);
            seq.AddAction(call);
            return call;
        }

        public static ActionDynamicCall Call(ActionGroup grp, Delegate function)
        {
            var call = new ActionDynamicCall(function);
            grp.AddAction(call);
            return call;
        }
        public static void Call(ActionGroup grp, System.Action function)
        {
            var call = new ActionCall(function);
            grp.AddAction(call);
        }
        public static void Call<T1>(ActionGroup grp, System.Action<T1> function, T1 arg1)
        {
            var call = new ActionCall<T1>(function, arg1);
            grp.AddAction(call);
        }
        public static void Call<T1, T2>(ActionGroup grp, System.Action<T1, T2> function, T1 arg1, T2 arg2)
        {
            var call = new ActionCall<T1, T2>(function, arg1, arg2);
            grp.AddAction(call);
        }
        public static void Call<T1, T2, T3>(ActionGroup grp, System.Action<T1, T2, T3> function, T1 arg1, T2 arg2, T3 arg3)
        {
            var call = new ActionCall<T1, T2, T3>(function, arg1, arg2, arg3);
            grp.AddAction(call);
        }
        public static void Call<T1, T2, T3, T4>(ActionGroup grp, System.Action<T1, T2, T3, T4> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            var call = new ActionCall<T1, T2, T3, T4>(function, arg1, arg2, arg3, arg4);
            grp.AddAction(call);
        }

        public static ActionReturnCall<TReturn> ReturnCall<TReturn>(ActionGroup grp, Func<TReturn> function)
        {
            var call = new ActionReturnCall<TReturn>(function);
            grp.AddAction(call);
            return call;
        }
        public static ActionReturnCall<T1, TReturn> ReturnCall<T1, TReturn>(ActionGroup grp, Func<T1, TReturn> function, T1 arg1)
        {
            var call = new ActionReturnCall<T1, TReturn>(function, arg1);
            grp.AddAction(call);
            return call;
        }
        public static ActionReturnCall<T1, T2, TReturn> ReturnCall<T1, T2, TReturn>(ActionGroup grp, Func<T1, T2, TReturn> function, T1 arg1, T2 arg2)
        {
            var call = new ActionReturnCall<T1, T2, TReturn>(function, arg1, arg2);
            grp.AddAction(call);
            return call;
        }
        public static ActionReturnCall<T1, T2, T3, TReturn> ReturnCall<T1, T2, T3, TReturn>(ActionGroup grp, Func<T1, T2, T3, TReturn> function, T1 arg1, T2 arg2, T3 arg3)
        {
            var call = new ActionReturnCall<T1, T2, T3, TReturn>(function, arg1, arg2, arg3);
            grp.AddAction(call);
            return call;
        }
        public static ActionReturnCall<T1, T2, T3, T4, TReturn> ReturnCall<T1, T2, T3, T4, TReturn>(ActionGroup grp, Func<T1, T2, T3, T4, TReturn> function, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            var call = new ActionReturnCall<T1, T2, T3, T4, TReturn>(function, arg1, arg2, arg3, arg4);
            grp.AddAction(call);
            return call;
        }




    }
}
