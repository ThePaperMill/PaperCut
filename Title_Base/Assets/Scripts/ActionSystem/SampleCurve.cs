using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace ActionSystem
{
    public class SampleCurve<T>
    {
        
        //typedef std::function<T(long double currentTime, const T& startValue,
        //                        const T& endValue, long double duration)> SampleFunc;
        public SampleCurve(Func<Number<double>, Number<T>, Number<T>, Number<double>, T> function)
        {
            Sample = function;
        }
        public SampleCurve(Ease ease = Ease.Linear)
        {
            switch (ease)
            {
                case Ease.CircIn:
                {
                    Sample = ActionMath<T>.CircIn;
                    
                }break;
                case Ease.CircInOut:
                {
                    Sample = ActionMath<T>.CircInOut;
                }break;
                case Ease.CircOut:
                {
                    Sample = ActionMath<T>.CircOut;
                }break;
                case Ease.CubicIn:
                {
                    Sample = ActionMath<T>.CubicIn;
                }break;
                case Ease.CubicInOut:
                {
                    Sample = ActionMath<T>.CubicInOut;
                }break;
                case Ease.CubicOut:
                {
                    Sample = ActionMath<T>.CubicOut;
                }break;
                case Ease.ExpoIn:
                {
                    Sample = ActionMath<T>.ExpoIn;
                }break;
                case Ease.ExpoInOut:
                {
                    Sample = ActionMath<T>.ExpoInOut;
                }break;
                case Ease.ExpoOut:
                {
                    Sample = ActionMath<T>.ExpoOut;
                }break;
                case Ease.Linear:
                {
                    Sample = ActionMath<T>.Linear;
                }break;
                case Ease.QntIn:
                {
                    Sample = ActionMath<T>.QuinticIn;
                }break;
                case Ease.QntInOut:
                {
                    Sample = ActionMath<T>.QuinticInOut;
                }break;
                case Ease.QntOut:
                {
                    Sample = ActionMath<T>.QuinticOut;
                }break;
                case Ease.QuadIn:
                {
                    Sample = ActionMath<T>.QuadIn;
                }break;
                case Ease.QuadInOut:
                {
                    Sample = ActionMath<T>.QuadInOut;
                }break;
                case Ease.QuadOut:
                {
                    Sample = ActionMath<T>.QuadOut;
                }break;
                case Ease.QuarticIn:
                {
                    Sample = ActionMath<T>.QuarticIn;
                }break;
                case Ease.QuarticInOut:
                {
                    Sample = ActionMath<T>.QuarticInOut;
                }break;
                case Ease.QuarticOut:
                {
                    Sample = ActionMath<T>.QuarticOut;
                }break;
                case Ease.SinIn:
                {
                    Sample = ActionMath<T>.SinIn;
                }break;
                case Ease.SinInOut:
                {
                    Sample = ActionMath<T>.SinInOut;
                }break;
                case Ease.SinOut:
                {
                    Sample = ActionMath<T>.SinOut;
                }break;
                default:
                {
                    //No ease specified. Using linear.
                    UnityEngine.Debug.Log("This ease is not yet implemented. Using Linear as default.");
                    Sample = ActionMath<T>.Linear;
                }break;
            }
        }
        //This is the delegate to the easing equation.
        //It is public because it doesn't really matter if the user changes it.
        public Func<Number<double>, Number<T>, Number<T>, Number<double>, T> Sample;
    }

    public class CustomCurve<T> : SampleCurve<T>
    {

        public CustomCurve(AnimationCurve animationCurve) : base()
        {
            Curve = animationCurve;
            Sample = UpdateCurve;
        }

        private T UpdateCurve(Number<Double> currentTime, Number<T> startValue, Number<T> endValue, Number<Double> duration)
        {
            Number<T> change = (endValue - startValue);
            
            return change * Curve.Evaluate((float)(currentTime / duration)) + startValue;
        }

        public AnimationCurve Curve { get; private set;}
    }

}


