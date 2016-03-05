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

// <summary>
/// This interface defines all of the operations that can be done in generic classes
/// These operations can be assigned to operators in class Number<T>
/// </summary>

/// <typeparam name="T">Type that
/// we will be doing arithmetic with</typeparam>

namespace ActionSystem
{
    public interface ICalculator<T>
    {
        T Sum(T a, T b);
        T Difference(T a, T b);
        bool Compare(T a, T b);
        T Multiply(T a, T b);
        T Multiply(T a, double b);
        T Divide(T a, T b);
        T Divide(T a, double b);
        T Divide(T a, int b);
        T Decrement(T a);
        //for doing integer division which is needed to do averages
    }

    /// <summary>
    /// ICalculator<T> implementation for Int32 type
    /// </summary>

    struct Int32Calculator : ICalculator<Int32>
    {
        public Int32 Decrement(Int32 a)
        {
            return --a;
        }

        public Int32 Sum(Int32 a, Int32 b)
        {
            return a + b;
        }

        public Int32 Difference(Int32 a, Int32 b)
        {
            return a - b;
        }

        public bool Compare(Int32 a, Int32 b)
        {
            return Difference(a, b) > 0;
        }

        public int Multiply(Int32 a, Int32 b)
        {
            return a * b;
        }

        public Int32 Multiply(Int32 a, Double b)
        {
            return a *(Int32)b;
        }

        public int Divide(Int32 a, Int32 b)
        {
            return a / b;
        }

        public int Divide(Int32 a, Double b)
        {
            return a / (Int32)b;
        }
    }

    struct DoubleCalculator : ICalculator<Double>
    {
        public Double Decrement(Double a)
        {
            return --a;
        }

        public Double Sum(Double a, Double b)
        {
            return a + b;
        }

        public Double Difference(Double a, Double b)
        {
            return a - b;
        }

        public bool Compare(Double a, Double b)
        {
            return Difference(a, b) > 0;
        }

        public bool Compare(Double a, int b)
        {
            return Difference(a, b) > 0;
        }

        public Double Multiply(Double a, Double b)
        {
            return a * b;
        }

        public Double Divide(Double a, Double b)
        {
            return a / b;
        }

        public Double Divide(Double a, int b)
        {
            return (int)a / b;
        }
    }

    struct Int64Calculator : ICalculator<Int64>
    {
        public Int64 Decrement(Int64 a)
        {
            return --a;
        }

        public Int64 Sum(Int64 a, Int64 b)
        {
            return a + b;
        }

        public Int64 Difference(Int64 a, Int64 b)
        {
            return a - b;
        }

        public bool Compare(Int64 a, Int64 b)
        {
            return Difference(a, b) > 0;
        }

        public Int64 Compare(Int64 a, int b)
        {
            return Difference(a, b);
        }

        public Int64 Multiply(Int64 a, Int64 b)
        {
            return a * b;
        }

        public Int64 Multiply(Int64 a, Double b)
        {
            return a * (Int64)b;
        }

        public Int64 Divide(Int64 a, Int64 b)
        {
            return a / b;
        }

        public Int64 Divide(Int64 a, int b)
        {
            return a / b;
        }

        public Int64 Divide(Int64 a, Double b)
        {
            return a / (Int64)b;
        }
    }

    struct SingleCalculator : ICalculator<float>
    {
        public float Decrement(float a)
        {
            return --a;
        }

        public float Sum(float a, float b)
        {
            return a + b;
        }

        public float Difference(float a, float b)
        {
            return a - b;
        }

        public bool Compare(float a, float b)
        {
            return Difference(a, b) > 0;
        }

        public bool Compare(float a, int b)
        {
            return Difference(a, b) > 0;
        }

        public float Multiply(float a, Double b)
        {
            return (float)(a * b);
        }

        public float Multiply(float a, float b)
        {
            return (float)(a * b);
        }

        public float Divide(float a, Double b)
        {
            return (float)(a / b);
        }

        public float Divide(float a, float b)
        {
            return a / b;
        }

        public float Divide(float a, int b)
        {
            return (int)(a / b);
        }
    }

    struct Vector2Calculator : ICalculator<Vector2>
    {
        public Vector2 Decrement(Vector2 a)
        {
            --a.x;
            --a.y;
            return a;
        }

        public Vector2 Sum(Vector2 a, Vector2 b)
        {
            return a + b;
        }

        public Vector2 Difference(Vector2 a, Vector2 b)
        {
            return a - b;
        }

        public bool Compare(Vector2 a, Vector2 b)
        {
            return (a.x + a.y) > (b.x + b.y);
        }

        public bool Compare(Vector2 a, int b)
        {
            return (a.x + a.y) > (b);
        }

        public Vector2 Multiply(Vector2 a, Double b)
        {
            a.x *= (float)b;
            a.y *= (float)b;
            return a;
        }

        public Vector2 Multiply(Vector2 a, Vector2 b)
        {
            a.x *= b.x;
            a.y *= b.y;
            return a;
        }

        public Vector2 Divide(Vector2 a, Double b)
        {
            a.x /= (float)b;
            a.y /= (float)b;
            return a;
        }

        public Vector2 Divide(Vector2 a, Vector2 b)
        {
            a.x /= b.x;
            a.y /= b.y;
            return a;
        }

        public Vector2 Divide(Vector2 a, int b)
        {
            a.x /= (float)b;
            a.y /= (float)b;
            return a;
        }
    }

    struct Vector3Calculator : ICalculator<Vector3>
    {
        public Vector3 Decrement(Vector3 a)
        {
            --a.x;
            --a.y;
            --a.z;
            return a;
        }

        public Vector3 Sum(Vector3 a, Vector3 b)
        {
            return a + b;
        }

        public Vector3 Difference(Vector3 a, Vector3 b)
        {
            return a - b;
        }

        public bool Compare(Vector3 a, Vector3 b)
        {
            return (a.x + a.y + a.z) > (b.x + b.y + b.z);
        }

        public bool Compare(Vector3 a, int b)
        {
            return (a.x + a.y + a.z) > (b);
        }

        public Vector3 Multiply(Vector3 a, Double b)
        {
            a.x *= (float)b;
            a.y *= (float)b;
            a.z *= (float)b;
            
            return a;
        }

        public Vector3 Multiply(Vector3 a, Vector3 b)
        {
            a.x *= b.x;
            a.y *= b.y;
            a.z *= b.z;
            return a;
        }

        public Vector3 Divide(Vector3 a, Double b)
        {
            a.x /= (float)b;
            a.y /= (float)b;
            a.z /= (float)b;
            return a;
        }

        public Vector3 Divide(Vector3 a, Vector3 b)
        {
            a.x /= b.x;
            a.y /= b.y;
            a.z /= b.z;
            return a;
        }

        public Vector3 Divide(Vector3 a, int b)
        {
            a.x /= (float)b;
            a.y /= (float)b;
            a.z /= (float)b;
            return a;
        }
    }

    struct Vector4Calculator : ICalculator<Vector4>
    {
        public Vector4 Decrement(Vector4 a)
        {
            --a.x;
            --a.y;
            --a.z;
            --a.w;
            return a;
        }

        public Vector4 Sum(Vector4 a, Vector4 b)
        {
            return a + b;
        }

        public Vector4 Difference(Vector4 a, Vector4 b)
        {
            return a - b;
        }

        public bool Compare(Vector4 a, Vector4 b)
        {
            return (a.x + a.y + a.z + a.w) > (b.x + b.y + b.z + b.w);
        }

        public bool Compare(Vector4 a, int b)
        {
            return (a.x + a.y + a.z + a.w) > (b);
        }

        public Vector4 Multiply(Vector4 a, Double b)
        {
            a.x *= (float)b;
            a.y *= (float)b;
            a.z *= (float)b;
            a.w *= (float)b;
            return a;
        }

        public Vector4 Multiply(Vector4 a, Vector4 b)
        {
            a.x *= b.x;
            a.y *= b.y;
            a.z *= b.z;
            a.w *= b.z;
            return a;
        }

        public Vector4 Divide(Vector4 a, Double b)
        {
            a.x /= (float)b;
            a.y /= (float)b;
            a.z /= (float)b;
            a.w /= (float)b;
            return a;
        }

        public Vector4 Divide(Vector4 a, Vector4 b)
        {
            a.x /= b.x;
            a.y /= b.y;
            a.z /= b.z;
            a.w /= b.z;
            return a;
        }

        public Vector4 Divide(Vector4 a, int b)
        {
            a.x /= (float)b;
            a.y /= (float)b;
            a.z /= (float)b;
            a.w /= (float)b;
            return a;
        }
    }

    struct QuaternionCalculator : ICalculator<Quaternion>
    {
        public Quaternion Decrement(Quaternion a)
        {
            --a.x;
            --a.y;
            --a.z;
            --a.w;
            return a;
        }

        public Quaternion Sum(Quaternion a, Quaternion b)
        {
            
            return new Quaternion(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
        }

        public Quaternion Difference(Quaternion a, Quaternion b)
        {
            return new Quaternion(a.x - b.x, a.y - b.y, a.z - b.z, a.w - b.w);
        }

        public bool Compare(Quaternion a, Quaternion b)
        {
            return (a.x + a.y + a.z + a.w) > (b.x + b.y + b.z + b.w);
        }

        public bool Compare(Quaternion a, int b)
        {
            return (a.x + a.y + a.z + a.w) > (b);
        }

        public Quaternion Multiply(Quaternion a, Double b)
        {
            a.x *= (float)b;
            a.y *= (float)b;
            a.z *= (float)b;
            a.w *= (float)b;
            return a;
        }

        public Quaternion Multiply(Quaternion a, Quaternion b)
        {
            a.x *= b.x;
            a.y *= b.y;
            a.z *= b.z;
            a.w *= b.z;
            return a;
        }

        public Quaternion Divide(Quaternion a, Double b)
        {
            a.x /= (float)b;
            a.y /= (float)b;
            a.z /= (float)b;
            a.w /= (float)b;
            return a;
        }

        public Quaternion Divide(Quaternion a, Quaternion b)
        {
            a.x /= b.x;
            a.y /= b.y;
            a.z /= b.z;
            a.w /= b.z;
            return a;
        }

        public Quaternion Divide(Quaternion a, int b)
        {
            a.x /= (float)b;
            a.y /= (float)b;
            a.z /= (float)b;
            a.w /= (float)b;
            return a;
        }
    }

    struct ColorCalculator : ICalculator<Color>
    {
        public Color Decrement(Color a)
        {
            --a.r;
            --a.g;
            --a.b;
            --a.a;
            return a;
        }

        public Color Sum(Color a, Color b)
        {

            return new Color(a.r + b.r, a.g + b.g, a.b + b.b, a.a + b.a);
        }

        public Color Difference(Color a, Color b)
        {
            return new Color(a.r - b.r, a.g - b.g, a.b - b.b, a.a - b.a);
        }

        public bool Compare(Color a, Color b)
        {
            return (a.r + a.g + a.b + a.a) > (b.r + b.g + b.b + b.a);
        }

        public bool Compare(Color a, int b)
        {
            return (a.r + a.g + a.b + a.a) > (b);
        }

        public Color Multiply(Color a, Double b)
        {
            a.r *= (float)b;
            a.g *= (float)b;
            a.b *= (float)b;
            a.a *= (float)b;
            return a;
        }

        public Color Multiply(Color a, Color b)
        {
            a.r *= b.r;
            a.g *= b.g;
            a.b *= b.b;
            a.a *= b.b;
            return a;
        }

        public Color Divide(Color a, Double b)
        {
            a.r /= (float)b;
            a.g /= (float)b;
            a.b /= (float)b;
            a.a /= (float)b;
            return a;
        }

        public Color Divide(Color a, Color b)
        {
            a.r /= b.r;
            a.g /= b.g;
            a.b /= b.b;
            a.a /= b.b;
            return a;
        }

        public Color Divide(Color a, int b)
        {
            a.r /= (float)b;
            a.g /= (float)b;
            a.b /= (float)b;
            a.a /= (float)b;
            return a;
        }
    }

    /// <summary>
    /// This class uses reflection to automatically create the correct 
    /// ICalculator<T> that is needed for any particular type T.
    /// </summary>
    /// <typeparam name="T">Type that we
    /// will be doing arithmetic with</typeparam>

    public class Number<T>
    {

        private T value;

        public Number(T value)
        {
            this.value = value;
        }

        /// <summary>
        /// Big IF chain to decide exactly which ICalculator needs to be created
        /// Since the ICalculator is cached, this if chain is executed only once per type
        /// </summary>
        /// <returns>The type of the calculator that needs to be created</returns>
        
        public static Type GetCalculatorType()
        {
            Type tType = typeof(T);

            String propName = String.Format(tType.Name + "Calculator");
            
            if (!Types.HasType(propName))
            {
                throw new InvalidCastException("Unsupported Type: " + tType.Name +
                      " does not have a partner implementation of interface " +
                      "ICalculator<T> and cannot be used in generic " +
                      "arithmetic using type Number<T>");
            }
            
            return Types.GetType(propName);
        }

        /// <summary>

        /// a static field to store the calculator after it is created
        /// this is the caching that is refered to above
        /// </summary>
        private static ICalculator<T> fCalculator = null;

        /// <summary>

        /// Singleton pattern- only one calculator created per type
        /// 
        /// </summary>
        public static ICalculator<T> Calculator
        {
            get
            {
                if (fCalculator == null)
                {
                    MakeCalculator();
                }
                return fCalculator;
            }
        }

        /// <summary>

        /// Here the actual calculator is created using the system activator
        /// </summary>

        public static void MakeCalculator()
        {
            Type calculatorType = GetCalculatorType();
            fCalculator = Activator.CreateInstance(calculatorType) as ICalculator<T>;
        }

        /// These methods can be called by the applications
        /// programmer if no operator overload is defined
        /// If an operator overload is defined these methods are not needed
        #region operation methods

        public static T Sum(T a, T b)
        {
            return Calculator.Sum(a, b);
        }

        public static T Difference(T a, T b)
        {
            return Calculator.Difference(a, b);
        }

        public static bool Compare(T a, T b)
        {
            return Calculator.Compare(a, b);
        }

        public static T Multiply(T a, T b)
        {
            return Calculator.Multiply(a, b);
        }

        public static T Divide(T a, T b)
        {
            return Calculator.Divide(a, b);
        }

        public static T Divide(T a, int b)
        {
            return Calculator.Divide(a, b);
        }

        #endregion

        /// These operator overloads make doing the arithmetic easy.
        /// For custom operations, an operation method
        /// may be the only way to perform the operation
        #region Operators

        //IMPORTANT:  The implicit operators
        //allows an object of type Number<T> to be
        //easily and seamlessly wrap an object of type T. 
        public static implicit operator Number<T>(T a)
        {
            return new Number<T>(a);
        }

        //IMPORTANT:  The implicit operators allows 
        //an object of type Number<T> to be
        //easily and seamlessly wrap an object of type T. 
        public static implicit operator T(Number<T> a)
        {
            return a.value;
        }

        public static Number<T>
               operator +(Number<T> a, Number<T> b)
        {
            return Calculator.Sum(a.value, b.value);
        }

        public static Number<T>
               operator -(Number<T> a, Number<T> b)
        {
            return Calculator.Difference(a, b);
        }

        public static bool operator >(Number<T> a, Number<T> b)
        {
            return Calculator.Compare(a, b);
        }

        public static bool operator <(Number<T> a, Number<T> b)
        {
            return !Calculator.Compare(a, b);
        }

        public static Number<T>
               operator *(Number<T> a, Number<T> b)
        {
            return Calculator.Multiply(a, b);
        }

        public static Number<T>
               operator *(Number<T> a, Number<Double> b)
        {
            return Calculator.Multiply(a, b);
        }

        public static Number<T>
               operator /(Number<T> a, Number<T> b)
        {
            return Calculator.Divide(a, b);
        }

        public static Number<T>
               operator /(Number<T> a, int b)
        {
            return Calculator.Divide(a, b);
        }

        public static Number<T>
               operator /(Number<T> a, Number<Double> b)
        {
            return Calculator.Divide(a, b);
        }
        public static Number<T>
               operator --(Number<T> a)
        {
            return Calculator.Decrement(a);
        }
        
        #endregion
    }



}