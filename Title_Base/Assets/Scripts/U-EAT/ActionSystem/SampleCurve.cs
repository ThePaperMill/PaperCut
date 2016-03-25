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

namespace ActionSystem
{
    [Serializable]
    public class Curve
    {
        public bool UseCustomCurve = false;
        public Ease CurveType = Ease.Linear;
        public AnimationCurve CustomCurveType = new AnimationCurve();
        BaseCurve StoredCurve;

        public Curve(Ease ease = Ease.Linear)
        {
            CurveType = ease;
        }

        public Curve(AnimationCurve curve)
        {
            UseCustomCurve = true;
            CustomCurveType = curve;
        }

        public T Sample<T>(Number<Double> currentTime, Number<T> startValue, Number<T> endValue, Number<Double> duration)
        {
            if(!UseCustomCurve)
            {
                if (StoredCurve == null || StoredCurve.GetType() != typeof(SampleCurve<T>))
                {
                    StoredCurve = new SampleCurve<T>(CurveType);
                }
                var curve = ((SampleCurve<T>)StoredCurve);
                curve.Sample = BaseCurve.GetEaseFromEnum<T>(CurveType);
                return curve.Sample(currentTime, startValue, endValue, duration);
            }
            else
            {
                if (StoredCurve == null || StoredCurve.GetType() != typeof(CustomCurve<T>))
                {
                    StoredCurve = new CustomCurve<T>(CustomCurveType);
                }
                return ((CustomCurve<T>)StoredCurve).Sample(currentTime, startValue, endValue, duration);
            }
        }

        public static implicit operator Ease(Curve value)
        {
            return value.CurveType;
        }

        public static implicit operator Curve(Ease value)
        {
            return new Curve(value);
        }

        public static implicit operator AnimationCurve(Curve value)
        {
            return value.CustomCurveType;
        }

        public static implicit operator Curve(AnimationCurve value)
        {
            return new Curve(value);
        }
    }

    [Serializable]
    public class BaseCurve
    {
        static public Func<Number<double>, Number<T>, Number<T>, Number<double>, T> GetEaseFromEnum<T>(Ease ease)
        {
            switch (ease)
            {
                case Ease.CircIn:
                    {
                        return ActionMath.CircIn;
                    }                    
                case Ease.CircInOut:
                    {
                        return ActionMath.CircInOut;
                    }                    
                case Ease.CircOut:
                    {
                        return ActionMath.CircOut;
                    }                    
                case Ease.CubicIn:
                    {
                        return ActionMath.CubicIn;
                    }                   
                case Ease.CubicInOut:
                    {
                        return ActionMath.CubicInOut;
                    }                    
                case Ease.CubicOut:
                    {
                        return ActionMath.CubicOut;
                    }                   
                case Ease.ExpoIn:
                    {
                        return ActionMath.ExpoIn;
                    }              
                case Ease.ExpoInOut:
                    {
                        return ActionMath.ExpoInOut;
                    }                
                case Ease.ExpoOut:
                    {
                        return ActionMath.ExpoOut;
                    }                
                case Ease.Linear:
                    {
                        return ActionMath.Linear;
                    }              
                case Ease.QntIn:
                    {
                        return ActionMath.QuinticIn;
                    }                 
                case Ease.QntInOut:
                    {
                        return ActionMath.QuinticInOut;
                    }               
                case Ease.QntOut:
                    {
                        return ActionMath.QuinticOut;
                    }          
                case Ease.QuadIn:
                    {
                        return ActionMath.QuadIn;
                    }         
                case Ease.QuadInOut:
                    {
                        return ActionMath.QuadInOut;
                    }            
                case Ease.QuadOut:
                    {
                        return ActionMath.QuadOut;
                    }             
                case Ease.QuarticIn:
                    {
                        return ActionMath.QuarticIn;
                    }          
                case Ease.QuarticInOut:
                    {
                        return ActionMath.QuarticInOut;
                    }        
                case Ease.QuarticOut:
                    {
                        return ActionMath.QuarticOut;
                    }        
                case Ease.SinIn:
                    {
                        return ActionMath.SinIn;
                    }  
                case Ease.SinInOut:
                    {
                        return ActionMath.SinInOut;
                    } 
                case Ease.SinOut:
                    {
                        return ActionMath.SinOut;
                    }          
                default:
                    {
                        //No ease specified. Using linear.
                        UnityEngine.Debug.Log("This ease is not yet implemented. Using Linear as default.");
                        return  ActionMath.Linear;
                    }
                    
            }
        }
    }
    [Serializable]
    public class SampleCurve<T> : BaseCurve
    {
        //typedef std::function<T(long double currentTime, const T& startValue,
        //                        const T& endValue, long double duration)> SampleFunc;
        public SampleCurve(Func<Number<double>, Number<T>, Number<T>, Number<double>, T> function)
        {
            Sample = function;
        }
        public SampleCurve(Ease ease = Ease.Linear)
        {
            Sample = GetEaseFromEnum<T>(ease);
        }
        //This is the delegate to the easing equation.
        //It is public because it doesn't really matter if the user changes it.
        public Func<Number<double>, Number<T>, Number<T>, Number<double>, T> Sample;
    }
    [Serializable]
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
#if UNITY_EDITOR
namespace CustomInspector
{
    using ActionSystem;
    using UnityEditor;
    [CanEditMultipleObjects]
    [CustomPropertyDrawer(typeof(Curve), true)]
    public class SampleCurveDrawer : PropertyDrawer
    {
        
        float ToggleWidth = 70;
        void OnEnable()
        {
            

        }

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            var enumProp = property.FindPropertyRelative("CurveType");
            var curveProp = property.FindPropertyRelative("CustomCurveType");
            var customCurveProp = property.FindPropertyRelative("UseCustomCurve");
            Ease enumVal = (Ease)enumProp.enumValueIndex;
            AnimationCurve customCurve = curveProp.animationCurveValue;

            var labelRect = new Rect(position.x, position.y, InspectorValues.LabelWidth, position.height);
            EditorGUI.LabelField(labelRect, property.name);
            var propStartPos = labelRect.position.x + labelRect.width;
            if (position.width > InspectorValues.MinRectWidth)
            {
                propStartPos += (position.width - InspectorValues.MinRectWidth) / InspectorValues.WidthScaler;
            }

            var toggleRect = new Rect(propStartPos, position.y, ToggleWidth, position.height);
            var enumRect = new Rect(toggleRect.position.x + toggleRect.width, position.y, position.width - (toggleRect.position.x + toggleRect.width) + 14, position.height);

            customCurveProp.boolValue = EditorGUI.ToggleLeft(toggleRect, "Custom", customCurveProp.boolValue);
            if(!customCurveProp.boolValue)
            {
                enumProp.enumValueIndex = (int)(Ease)EditorGUI.EnumPopup(enumRect, enumVal);
            }
            else
            {
                curveProp.animationCurveValue = EditorGUI.CurveField(enumRect, customCurve);
            }
            
            
            //curveProp.enumValueIndex = (int)Ease.CircInOut;
            //Debug.Log(property.type.GetPropert);
            //Debug.Log(property.);
            //property = property.;
            //EditorGUI.EnumPopup(position, label, Ease.Linear);
        }

    }
}
#endif