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
using UnityEngine;
using System.Reflection;
using System.Linq.Expressions;

namespace ActionSystem
{

    public class ActionMath
    {
        const double Pi = Mathf.PI;

        public static T LinearTest<T>(Double currentTime, T startValue, T endValue, Double duration)
        {
            var tType = typeof(T);
            var dType = typeof(float);
            ParameterExpression startVal = Expression.Parameter(tType, "startValue");
            ParameterExpression endVal = Expression.Parameter(tType, "endValue");
            ParameterExpression currentTimeVal = Expression.Parameter(dType, "currentTime");

            BinaryExpression addExp = Expression.Add(endVal, startVal);
            BinaryExpression subExp = Expression.Subtract(endVal, startVal);
            BinaryExpression multExp = Expression.Multiply(startVal, currentTimeVal);
            BinaryExpression divExp = Expression.Divide(startVal, currentTimeVal);

            Func<T, T, T> sub = Expression.Lambda<Func<T, T, T>>(subExp, endVal, startVal).Compile();
            Func<T, T, T> add = Expression.Lambda<Func<T, T, T>>(addExp, endVal, startVal).Compile();
            Func<T, float, T> div = Expression.Lambda<Func<T, float, T>>(divExp, startVal, currentTimeVal).Compile();
            Func<T, float, T> mult = Expression.Lambda<Func<T, float, T>>(multExp, startVal, currentTimeVal).Compile();
            
            T change = sub(endValue, startValue);
            
            return (add(div(mult(change, (float)currentTime) , (float)duration), startValue));
        }

        public static T Linear<T>(Number<Double> currentTime, Number<T> startValue, Number<T> endValue, Number<Double> duration)
        {
            
            Number<T> change = (endValue - startValue);
            return (change * currentTime / duration + startValue);
        }

        //Quadratic

        public static T QuadIn<T>(Number<Double> currentTime, Number<T> startValue, Number<T> endValue, Number<Double> duration)
        {
            Number<T> change = endValue - startValue;
            currentTime /= duration;

            return (change * currentTime * currentTime + startValue);
        }

        public static T QuadOut<T>(Number<Double> currentTime, Number<T> startValue, Number<T> endValue, Number<Double> duration)
        {
            Number<T> change = endValue - startValue;

            currentTime /= duration;
            //I multiply by -1 on the right hand side so the user does not need to implement the
            //unary operator-.
            return ((change * -1) * currentTime * (currentTime - 2) + startValue);
        }

        public static T QuadInOut<T>(Number<Double> currentTime, Number<T> startValue, Number<T> endValue, Number<Double> duration)
        {
            Number<T> change = endValue - startValue;

            currentTime /= (duration/2.0);
            ;
            if (currentTime < 1)
            {
                return ((change / 2.0) * currentTime * currentTime + startValue);
            }

            currentTime -= 1;
            return (((change * -1) / 2.0) * (currentTime * (currentTime - 2) - 1) + startValue);
        }

        //Sinusoidal
        public static T SinIn<T>(Number<Double> currentTime, Number<T> startValue, Number<T> endValue, Number<Double> duration)
        {
           Number<T> change = endValue - startValue;

            return ((change * -1) * Mathf.Cos((float)(currentTime / duration * (Pi / 2))) + change + startValue);
        }

        public static T SinOut<T>(Number<Double> currentTime, Number<T> startValue, Number<T> endValue, Number<Double> duration)
        {
            Number<T> change = endValue - startValue;

            return (change * Mathf.Sin((float)(currentTime / duration * (Pi / 2))) + startValue);
        }

        public static T SinInOut<T>(Number<Double> currentTime, Number<T> startValue, Number<T> endValue, Number<Double> duration)
        {
            Number<T> change = endValue - startValue;
            change *= -0.5;
            return ( change * (Math.Cos(((Pi * currentTime) / duration)) - 1) + startValue);
        }

        //Exponential
        
        public static T ExpoIn<T>(Number<Double> currentTime, Number<T> startValue, Number<T> endValue, Number<Double> duration)
        {
            Number<T> change = endValue - startValue;

            return (change * Math.Pow(2,(10 * (currentTime / duration - 1))) + startValue);
        }
        
        public static T ExpoOut<T>(Number<Double> currentTime, Number<T> startValue, Number<T> endValue, Number<Double> duration)
        {
            Number<T> change = endValue - startValue;

            return (change * (-Math.Pow(2, -10 * currentTime / duration) + 1) + startValue);
        }

        public static T ExpoInOut<T>(Number<Double> currentTime, Number<T> startValue, Number<T> endValue, Number<Double> duration)
        {
            Number<T> change = endValue - startValue;
            currentTime /= (duration / 2.0);
            if (currentTime < 1)
            {
                return (change / 2.0) * Math.Pow(2, 10 * (currentTime - 1)) + startValue;
            }
            --currentTime;

            return ((change / 2.0) * (-Math.Pow(2, -10 * currentTime) + 2) + startValue);
        }

        ////Circular

        public static T CircIn<T>(Number<Double> currentTime, Number<T> startValue, Number<T> endValue, Number<Double> duration)
        {
            Number<T> change = endValue - startValue;
            currentTime /= duration;

            return ((change * -1) * (Math.Sqrt(1 - currentTime * currentTime) - 1) + startValue);
        }

        public static T CircOut<T>(Number<Double> currentTime, Number<T> startValue, Number<T> endValue, Number<Double> duration)
        {
            Number<T> change = endValue - startValue;
            currentTime /= duration;
            --currentTime;

            return (change * Math.Sqrt(1 - currentTime * currentTime) + startValue);
        }

        public static T CircInOut<T>(Number<Double> currentTime, Number<T> startValue, Number<T> endValue, Number<Double> duration)
        {
            Number<T> change = endValue - startValue;
            currentTime /= (duration / 2.0);
            
            if (currentTime < 1)
            {
                
                return ((change * -1) / 2.0) * (Math.Sqrt(1 - currentTime * currentTime) - 1) + startValue;
            }
            currentTime -= 2;
            
            return ((change / 2.0) * (Math.Sqrt(1 - currentTime * currentTime) + 1) + startValue);
        }

        //Cubic

        public static T CubicIn<T>(Number<Double> currentTime, Number<T> startValue, Number<T> endValue, Number<Double> duration)
        {
            Number<T> change = endValue - startValue;

            currentTime /= duration;
            return (change * currentTime * currentTime * currentTime + startValue);
        }

        public static T CubicOut<T>(Number<Double> currentTime, Number<T> startValue, Number<T> endValue, Number<Double> duration)
        {
            Number<T> change = endValue - startValue;

            currentTime /= duration;
            currentTime -= 1;
            return (change * (currentTime * currentTime * currentTime + 1) + startValue);
        }

        public static T CubicInOut<T>(Number<Double> currentTime, Number<T> startValue, Number<T> endValue, Number<Double> duration)
        {
            Number<T> change = endValue - startValue;

            currentTime /= (duration / 2.0);
            if (currentTime < 1)
            {
                return ((change / 2.0) * currentTime * currentTime * currentTime + startValue);
            }

            currentTime -= 2;
            return ((change / 2.0) * (currentTime * currentTime * currentTime + 2) + startValue);
        }

        //Quartic

        public static T QuarticIn<T>(Number<Double> currentTime, Number<T> startValue, Number<T> endValue, Number<Double> duration)
        {
            Number<T> change = endValue - startValue;

            currentTime /= duration;
            return (change * currentTime * currentTime * currentTime * currentTime + startValue);
        }

        public static T QuarticOut<T>(Number<Double> currentTime, Number<T> startValue, Number<T> endValue, Number<Double> duration)
        {
            Number<T> change = endValue - startValue;

            currentTime /= duration;
            currentTime -= 1;
            return ((change * -1) * (currentTime * currentTime * currentTime * currentTime - 1) + startValue);
        }

        public static T QuarticInOut<T>(Number<Double> currentTime, Number<T> startValue, Number<T> endValue, Number<Double> duration)
        {
            Number<T> change = endValue - startValue;

            currentTime /= (duration / 2.0);
            if (currentTime < 1)
            {
                return ((change / 2) * (currentTime * currentTime * currentTime * currentTime) + startValue);
            }
            
            currentTime -= 2;
            return (((change * -1) / 2.0) * (currentTime * currentTime * currentTime * currentTime - 2) + startValue);
        }

        //Quintic
        public static T QuinticIn<T>(Number<Double> currentTime, Number<T> startValue, Number<T> endValue, Number<Double> duration)
        {
            Number<T> change = endValue - startValue;

            currentTime /= duration;
            return (change * currentTime * currentTime * currentTime * currentTime * currentTime + startValue);
        }

        public static T QuinticOut<T>(Number<Double> currentTime, Number<T> startValue, Number<T> endValue, Number<Double> duration)
        {
            Number<T> change = endValue - startValue;

            currentTime /= duration;
            currentTime -= 1;
            return (change * (currentTime * currentTime * currentTime * currentTime * currentTime + 1) + startValue);
        }

        public static T QuinticInOut<T>(Number<Double> currentTime, Number<T> startValue, Number<T> endValue, Number<Double> duration)
        {
            Number<T> change = endValue - startValue;

            currentTime /= (duration / 2.0);
            if (currentTime < 1)
            {
                return ((change / 2.0) * currentTime * currentTime * currentTime * currentTime * currentTime + startValue);
            }

            currentTime -= 2;
            return ((change / 2.0) * (currentTime * currentTime * currentTime * currentTime * currentTime + 2) + startValue);
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
