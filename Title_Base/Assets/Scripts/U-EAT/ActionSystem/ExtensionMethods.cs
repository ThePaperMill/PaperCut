/****************************************************************************/
/*!
    \author Joshua Biggs  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using ActionSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;

public static class ExtensionMethods
{
    public static int WordCount(this String str)
    {
        return str.Split(new char[] { ' ', '.', '?' },
                            StringSplitOptions.RemoveEmptyEntries).Length;
    }

    //public static T Get<T>(this T me)
    //{
    //    foreach (PropertyInfo i in typeof(T).GetProperties())
    //    {
    //        Debug.Log(i);
    //    }
    //    var Getter = typeof(T).GetProperties().First().GetGetMethod();

    //    var a = Delegate.CreateDelegate(typeof(Action<T>), me, Getter).DynamicInvoke();
    //    Debug.Log(a);
    //    return me;
    //}

    public static Action<T> GetPropertySetter<TObject, T>(this TObject instance, Expression<Func<TObject, T>> propAccessExpression)
    {
        var memberExpression = propAccessExpression.Body as MemberExpression;
        if (memberExpression == null) throw new ArgumentException("Lambda must be a simple property access", "propAccessExpression");

        var accessedMember = memberExpression.Member as PropertyInfo;
        if (accessedMember == null) throw new ArgumentException("Lambda must be a simple property access", "propAccessExpression");


        var setter = accessedMember.GetSetMethod();

        return (Action<T>)Delegate.CreateDelegate(typeof(Action<T>), instance, setter);
    }

    public static Func<T> GetPropertyGetter<TObject, T>(this TObject instance, Expression<Func<TObject, T>> propAccessExpression)
    {
        var memberExpression = propAccessExpression.Body as MemberExpression;
        if (memberExpression == null) throw new ArgumentException("Lambda must be a simple property access", "propAccessExpression");

        var accessedMember = memberExpression.Member as PropertyInfo;
        if (accessedMember == null) throw new ArgumentException("Lambda must be a simple property access", "propAccessExpression");

        var getter = accessedMember.GetGetMethod();

        return (Func < T > )Delegate.CreateDelegate(typeof(Func<T>), instance, getter);
    }

    public static Property<T> GetProperty<TObject, T>(this TObject instance, Expression<Func<TObject, T>> propAccessExpression)
    {
        var memberExpression = propAccessExpression.Body as MemberExpression;
        if (memberExpression == null) throw new ArgumentException("Lambda must be a simple property access", "propAccessExpression");

        var accessedMember = memberExpression.Member as PropertyInfo;
        if (accessedMember == null) throw new ArgumentException("Lambda must be a simple property access", "propAccessExpression");

        var getter = (Func<T>)Delegate.CreateDelegate(typeof(Func<T>), instance, accessedMember.GetGetMethod());
        var setter = (Action<T>)Delegate.CreateDelegate(typeof(Action<T>), instance, accessedMember.GetSetMethod());
        
        return new Property<T>(getter, setter);
    }

    public static List<MethodInfo> GetMethods(this Type me, String methodName)
    {
        List<MethodInfo> info = new List<MethodInfo>();
        var methods = me.GetMethods();
        foreach(MethodInfo i in methods)
        {
            if(methodName == i.Name)
            {
                info.Add(i);
            }
        }

        return info;
    }

    public static MethodInfo GetMethod(this Type me, String methodName, Type returnType, params Type[] argumentTypes)
    {
        var methods = me.GetMethods();
        foreach (MethodInfo i in methods)
        {
            if (methodName == i.Name)
            {
                if (i.ReturnType == returnType)
                {

                    var args = i.GetParameters();
                    for (int j = 0; j < args.Count(); ++j)
                    {
                        if (j >= argumentTypes.Count())
                        {
                            break;
                        }
                        if (args[j].ParameterType != argumentTypes[j])
                        {
                            break;
                        }
                        if (j == (args.Count() - 1))
                        {
                            return i;
                        }
                    }

                }
            }

        }

        return null;
    }

    public static MethodInfo FindFunctionByName<T>(this T me, String name)
    {
        var infoArray = typeof(T).GetMethods();
        foreach(var i in infoArray)
        {
            if(i.Name == name)
            {
                return i;
            }
            
        }
        return null;
    }

    public static ActionGroup GetActions(this MonoBehaviour me)
    {
        var Actions = me.GetComponent<ObjectActions>();
        if (Actions == null)
        {
            Actions = me.gameObject.AddComponent<ObjectActions>();
        }
        return Actions.Actions;
    }

    public static AudioSource GetAudioSource(this MonoBehaviour me)
    {
        var Audio = me.GetComponent<AudioSource>();
        if (Audio == null)
        {
            Audio = me.gameObject.AddComponent<AudioSource>();
        }
        return Audio;
    }

#if UNITY_EDITOR
    public static void DrawBaseDefaultInspector(this Editor me)
    {
        var type = me.target.GetType();
        //Can be optimiezed using the iterator.
        var fields = type.GetFields(BindingFlags.Instance | BindingFlags.DeclaredOnly | BindingFlags.Public);
        foreach (var i in fields)
        {
            var prop = me.serializedObject.FindProperty(i.Name);
            
            if (prop != null)
            {
                EditorGUILayout.PropertyField(prop);
                if (prop.isArray && prop.isExpanded)
                {
                    //prop.arraySize = EditorGUILayout.IntField("   Elements",prop.arraySize);
                    //Type objType = prop.type.GetType().GetGenericArguments()[0];
                    while (prop.NextVisible(true))
                    {
                        if(prop.depth == 0)
                        {
                            break;
                        }
                        EditorGUILayout.PropertyField(prop);
                        
                    }

                }
            }
        }
    }
#endif

    //public static Delegate GetDelegate<This>(this This me, System.Action function)
    //{
    //    return Delegate.CreateDelegate(typeof(System.Action), function.Method);
    //}
    //public static Delegate GetDelegate<This, T1>(this This me, String functionName)
    //{
    //    Type[] args = { typeof(T1) };
    //    return Delegate.CreateDelegate(typeof(System.Action<T1>), typeof(This).GetMethod(functionName, args));
    //}
    //public static Delegate GetDelegate<This, T1, T2>(this This me, Action<T1, T2> function)
    //{
    //    Type[] args = { typeof(T1), typeof(T2) };
    //    Debug.Log("ASDASD");
    //    MethodInfo info = function.Method;
    //    Debug.Log(info.GetParameters().Count());

    //    return Delegate.CreateDelegate(typeof(Action<T1, T2>), info);
    //}
    //public static Delegate GetDelegate<This, T1, T2, T3>(this This me, String functionName)
    //{
    //    Type[] args = { typeof(T1), typeof(T2), typeof(T3) };
    //    return Delegate.CreateDelegate(typeof(System.Action<T1, T2, T3>), typeof(This).GetMethod(functionName, args));
    //}

    //public static Delegate GetDelegate<This, T1, T2, T3, T4>(this This me, String functionName)
    //{
    //    Type[] args = { typeof(T1), typeof(T2), typeof(T3), typeof(T4) };
    //    return Delegate.CreateDelegate(typeof(System.Action<T1, T2, T3, T4>), typeof(This).GetMethod(functionName, args));
    //}


    //public static Delegate GetReturnDelegate<This, ReturnType>(this This me, String functionName)
    //{
    //    return Delegate.CreateDelegate(typeof(Function<ReturnType>), typeof(This).GetMethod(functionName));
    //}

    static public void Destroy(this GameObject obj)
    {
        GameObject.Destroy(obj);
    }

    public static void Trace<T>(this T instance, object param)
    {
        MonoBehaviour.print(param);
    }

    public static GameObject GetSpace(this MonoBehaviour instance)
    {
        var obj = GameObject.FindGameObjectWithTag("Space");
        if (obj.Equals(null))
        {
            obj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/Space"));
        }
        return obj;
    }

    public static int IndexOf<T>(this T[] instance, T param)
    {
        for (int i = 0; i < instance.Length; ++i)
        {
            if (instance[i].Equals(param))
            {
                return i;
            }
        }

        return -1;
    }

}

