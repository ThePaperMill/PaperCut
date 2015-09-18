using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ActionSystem
{

    public class ActionMath<T>
    {
        const double Pi = Mathf.PI;
        

        public static T Linear(Number<Double> currentTime, Number<T> startValue, Number<T> endValue, Number<Double> duration)
        {
            Number<T> change = (endValue - startValue);
            return (change * currentTime / duration + startValue);
        }

        //Quadratic

        public static T QuadIn(Number<Double> currentTime, Number<T> startValue, Number<T> endValue, Number<Double> duration)
        {
            Number<T> change = endValue - startValue;
            currentTime /= duration;

            return (change * currentTime * currentTime + startValue);
        }

        public static T QuadOut(Number<Double> currentTime, Number<T> startValue, Number<T> endValue, Number<Double> duration)
        {
            Number<T> change = endValue - startValue;

            currentTime /= duration;
            //I multiply by -1 on the right hand side so the user does not need to implement the
            //unary operator-.
            return ((change * -1) * currentTime * (currentTime - 2) + startValue);
        }

        public static T QuadInOut(Number<Double> currentTime, Number<T> startValue, Number<T> endValue, Number<Double> duration)
        {
            Number<T> change = endValue - startValue;

            currentTime /= (duration/2.0);
            ;
            if (currentTime < 1)
            {
                return (change / 2 * currentTime * currentTime + startValue);
            }

            currentTime -= 1;
            return ((change * -1) / 2 * (currentTime * (currentTime - 2) - 1) + startValue);
        }

        //Sinusoidal
        public static T SinIn(Number<Double> currentTime, Number<T> startValue, Number<T> endValue, Number<Double> duration)
        {
           Number<T> change = endValue - startValue;

            return ((change * -1) * Mathf.Cos((float)(currentTime / duration * (Pi / 2))) + change + startValue);
        }

        public static T SinOut(Number<Double> currentTime, Number<T> startValue, Number<T> endValue, Number<Double> duration)
        {
            Number<T> change = endValue - startValue;

            return (change * Mathf.Sin((float)(currentTime / duration * (Pi / 2))) + startValue);
        }

        public static T SinInOut(Number<Double> currentTime, Number<T> startValue, Number<T> endValue, Number<Double> duration)
        {
            Number<T> change = endValue - startValue;

            return ((change * -0.5) * (Math.Cos((Pi * currentTime / duration) - 1)) + startValue);
        }

        //Exponential
        
        public static T ExpoIn(Number<Double> currentTime, Number<T> startValue, Number<T> endValue, Number<Double> duration)
        {
            Number<T> change = endValue - startValue;

            return (change * Math.Pow(2,(10 * (currentTime / duration - 1))) + startValue);
        }
        
        public static T ExpoOut(Number<Double> currentTime, Number<T> startValue, Number<T> endValue, Number<Double> duration)
        {
            Number<T> change = endValue - startValue;

            return (change * (-Math.Pow(2, -10 * currentTime / duration) + 1) + startValue);
        }

        public static T ExpoInOut(Number<Double> currentTime, Number<T> startValue, Number<T> endValue, Number<Double> duration)
        {
            Number<T> change = endValue - startValue;
            currentTime /= (duration / 2.0);
            if (currentTime < 1)
            {
                return change / 2 * Math.Pow(2, 10 * (currentTime - 1)) + startValue;
            }
            --currentTime;

            return (change / 2 * (-Math.Pow(2, -10 * currentTime) + 2) + startValue);
        }

        ////Circular

        public static T CircIn(Number<Double> currentTime, Number<T> startValue, Number<T> endValue, Number<Double> duration)
        {
            Number<T> change = endValue - startValue;
            currentTime /= duration;

            return ((change * -1) * (Math.Sqrt(1 - currentTime * currentTime) - 1) + startValue);
        }

        public static T CircOut(Number<Double> currentTime, Number<T> startValue, Number<T> endValue, Number<Double> duration)
        {
            Number<T> change = endValue - startValue;
            currentTime /= duration;
            --currentTime;

            return (change * Math.Sqrt(1 - currentTime * currentTime) + startValue);
        }

        public static T CircInOut(Number<Double> currentTime, Number<T> startValue, Number<T> endValue, Number<Double> duration)
        {
            Number<T> change = endValue - startValue;
            currentTime /= (duration / 2.0);
            
            if (currentTime < 1)
            {
                
                return (change * -1) / 2 * (Math.Sqrt(1 - currentTime * currentTime) - 1) + startValue;
            }
            currentTime -= 2;
            
            return (change / 2 * (Math.Sqrt(1 - currentTime * currentTime) + 1) + startValue);
        }

        //Cubic

        public static T CubicIn(Number<Double> currentTime, Number<T> startValue, Number<T> endValue, Number<Double> duration)
        {
            Number<T> change = endValue - startValue;

            currentTime /= duration;
            return (change * currentTime * currentTime * currentTime + startValue);
        }

        public static T CubicOut(Number<Double> currentTime, Number<T> startValue, Number<T> endValue, Number<Double> duration)
        {
            Number<T> change = endValue - startValue;

            currentTime /= duration;
            currentTime -= 1;
            return (change * (currentTime * currentTime * currentTime + 1) + startValue);
        }

        public static T CubicInOut(Number<Double> currentTime, Number<T> startValue, Number<T> endValue, Number<Double> duration)
        {
            Number<T> change = endValue - startValue;

            currentTime /= (duration / 2.0);
            if (currentTime < 1)
            {
                return (change / 2 * currentTime * currentTime * currentTime + startValue);
            }

            currentTime -= 2;
            return (change / 2 * (currentTime * currentTime * currentTime + 2) + startValue);
        }

        //Quartic

        public static T QuarticIn(Number<Double> currentTime, Number<T> startValue, Number<T> endValue, Number<Double> duration)
        {
            Number<T> change = endValue - startValue;

            currentTime /= duration;
            return (change * currentTime * currentTime * currentTime * currentTime + startValue);
        }

        public static T QuarticOut(Number<Double> currentTime, Number<T> startValue, Number<T> endValue, Number<Double> duration)
        {
            Number<T> change = endValue - startValue;

            currentTime /= duration;
            currentTime -= 1;
            return ((change * -1) * (currentTime * currentTime * currentTime * currentTime - 1) + startValue);
        }

        public static T QuarticInOut(Number<Double> currentTime, Number<T> startValue, Number<T> endValue, Number<Double> duration)
        {
            Number<T> change = endValue - startValue;

            currentTime /= duration / 2;
            if (currentTime < 1)
            {
                return (change / 2 * currentTime * currentTime * currentTime * currentTime + startValue);
            }

            currentTime -= 2;
            return ((change * -1) / 2 * (currentTime * currentTime * currentTime * currentTime - 2) + startValue);
        }

        //Quintic
        public static T QuinticIn(Number<Double> currentTime, Number<T> startValue, Number<T> endValue, Number<Double> duration)
        {
            Number<T> change = endValue - startValue;

            currentTime /= duration;
            return (change * currentTime * currentTime * currentTime * currentTime * currentTime + startValue);
        }

        public static T QuinticOut(Number<Double> currentTime, Number<T> startValue, Number<T> endValue, Number<Double> duration)
        {
            Number<T> change = endValue - startValue;

            currentTime /= duration;
            currentTime -= 1;
            return (change * (currentTime * currentTime * currentTime * currentTime * currentTime + 1) + startValue);
        }

        public static T QuinticInOut(Number<Double> currentTime, Number<T> startValue, Number<T> endValue, Number<Double> duration)
        {
            Number<T> change = endValue - startValue;

            currentTime /= (duration / 2.0);
            if (currentTime < 1)
            {
                return (change / 2 * currentTime * currentTime * currentTime * currentTime * currentTime + startValue);
            }

            currentTime -= 2;
            return (change / 2 * (currentTime * currentTime * currentTime * currentTime * currentTime + 2) + startValue);
        }
    }//namespace Math

    //All the various ease types.
    public enum Ease
    {
        Linear,
        QuadIn,
        QuadInOut,
        QuadOut,
        SinIn,
        SinInOut,
        SinOut,
        ExpoIn,
        ExpoInOut,
        ExpoOut,
        CircIn,
        CircInOut,
        CircOut,
        CubicIn,
        CubicInOut,
        CubicOut,
        QuarticIn,
        QuarticInOut,
        QuarticOut,
        QntIn,
        QntInOut,
        QntOut
    };
}//namespace ActionSystem
