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
    

//unsafe public interface IProperty<T>
//{
//    T Get();
//    void Set(T a);
//}

//unsafe public class Int32Property : IProperty<Int32>
//{
//    private Int32* storedValue;

//    public Int32Property(Int32* value)
//    {
//        storedValue = value;
//    }

//    public Int32 Get()
//    {
//        return *storedValue;
//    }

//    public void Set(Int32 value)
//    {
//        *storedValue = value;
//    }
//}
//unsafe public class Int64Property : IProperty<Int64>
//{
//    private Int64* storedValue;

//    public Int64 Get()
//    {
//        return *storedValue;
//    }

//    public void Set(Int64 value)
//    {
//        *storedValue = value;
//    }
//}
//unsafe public class FloatProperty : IProperty<float>
//{
//    private float* storedValue;

//    public float Get()
//    {
//        return *storedValue;
//    }

//    public void Set(float value)
//    {
//        *storedValue = value;
//    }
//}
//unsafe public class DoubleProperty : IProperty<Double>
//{
//    private Double* storedValue;

//    public Double Get()
//    {
//        return *storedValue;
//    }

//    public void Set(Double value)
//    {
//        *storedValue = value;
//    }
//}
//unsafe public class Vector2Property : IProperty<Vector2>
//{
//    private Vector2* storedValue;

//    public Vector2 Get()
//    {
//        return *storedValue;
//    }

//    public void Set(Vector2 value)
//    {
//        *storedValue = value;
//    }
//}

//unsafe public class Vector3Property : IProperty<Vector3>
//{
//    private Vector3* storedValue;

//    public Vector3 Get()
//    {
//        return *storedValue;
//    }

//    public void Set(Vector3 value)
//    {
//        *storedValue = value;
//    }
//}

//unsafe public class Vector4Property : IProperty<Vector4>
//{
//    private Vector4* storedValue;

//    public Vector4 Get()
//    {
//        return *storedValue;
//    }

//    public void Set(Vector4 value)
//    {
//        *storedValue = value;
//    }
//}

}


public class Property<T>
{
    private Func<T> Getter;
    private Action<T> Setter;


    public Property(Func<T> getter, Action<T> setter)
    {
        Getter = getter;
        Setter = setter;
    }

    public T Get()
    {
        return (T)Getter.DynamicInvoke();
    }

    public void Set(T value)
    {
        Setter.DynamicInvoke(value);
    }
}

static public class Types
{
    static private Dictionary<String, Type> StoredTypes = new Dictionary<String, Type>();

    static Types()
    {
        foreach (Assembly a in AppDomain.CurrentDomain.GetAssemblies())
        {
            foreach (Type t in a.GetTypes())
            {
                if(!HasType(t.Name))
                {
                    StoredTypes.Add(t.Name, t);
                }
            }
        }
    }

    static public Type GetType(String typeName)
    {
        return StoredTypes[typeName];
    }

    static public bool HasType(String typeName)
    {
        return StoredTypes.ContainsKey(typeName);
    }
}