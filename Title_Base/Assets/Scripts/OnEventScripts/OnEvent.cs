using UnityEngine;
using System.Collections;
using ActionSystem;

using System.Collections.Generic;

using System;

public class OnEvent : MonoBehaviour
{
    public EventData Data = new EventData();
    public System.Action<EventData> OnEventFunction;
    public bool Active = true;
    public Events ListenEventName = Events.DefaultEvent;
    public GameObject ListenTarget;

    public bool DispatchEvents = false;
    public Events DispatchEventName = Events.DefaultEvent;
    public GameObject DispatchTarget;

    protected bool DelayedDispatch = false;
    //ActionSequence Seq = new ActionSequence();

    public void SetActive(bool setter)
    {
        Active = setter;
    }

    public virtual void Awake()
    {
        if (ListenTarget == null)
        {
            ListenTarget = this.gameObject;
        }
        if(OnEventFunction == null)
        {
            OnEventFunction = this.OnEventFunc;
        }
        Connect();

    }

    public void Connect()
    {
        ListenTarget.Connect(ListenEventName, OnEventInternal);
    }

    public void Disconnect()
    {
        ListenTarget.Disconnect(ListenEventName, OnEventInternal);
        
    }

    public void Reconnect(GameObject listenTarget = null, string eventName = null, System.Action<EventData> onEventFunc = null)
    {
        ListenTarget.Disconnect(ListenEventName, OnEventInternal);
        if(listenTarget != null)
        {
            ListenTarget = listenTarget;
        }
        if(eventName != null)
        {
            ListenEventName = eventName;
        }
        if(onEventFunc != null)
        {
            OnEventFunction = onEventFunc;
        }
        ListenTarget.Connect(ListenEventName, OnEventInternal);
    }

	// Use this for initialization
	void Start ()
    {
        
    }

    void OnEventInternal(EventData data)
    {
        if (!Active)
        {
            return;
        }

        OnEventFunction(data);

        if(DispatchEvents && !DelayedDispatch)
        {
            DispatchEvent();
        }
    }

    virtual public void OnEventFunc(EventData data)
    {

    }

    public void DispatchEvent()
    {
        if(DispatchTarget == null)
        {
            return;
        }
        DispatchTarget.DispatchEvent(DispatchEventName, Data);
    }

    void OnDestroy()
    {
        ListenTarget.Disconnect(ListenEventName, OnEventInternal);
    }
}

#if UNITY_EDITOR
namespace CustomInspector
{
    using UnityEditor;
    using System.Reflection;

    [CanEditMultipleObjects]
    [CustomEditor(typeof(OnEvent), true)]
    public class OnEventEditor : Editor
    {
        
        SerializedProperty ListenEventProp;
        SerializedProperty ListenTargetProp;
        SerializedProperty DispatchEventProp;
        SerializedProperty DispatchTargetProp;

        string[] FunctionNames;
        List<Action<EventData>> FunctionList;

        public virtual void OnEnable()
        {
            OnEvent comp = target as OnEvent;
            if (comp.ListenTarget == null)
            {
                comp.ListenTarget = comp.gameObject;
            }
            if (comp.DispatchTarget == null)
            {
                comp.DispatchTarget = comp.gameObject;
            }
            //

            ListenEventProp = serializedObject.FindProperty("ListenEventName");
            ListenTargetProp = serializedObject.FindProperty("ListenTarget");
            DispatchEventProp = serializedObject.FindProperty("DispatchEventName");
            DispatchTargetProp = serializedObject.FindProperty("DispatchTarget");

            if (comp.OnEventFunction == null)
            {
                comp.OnEventFunction = comp.OnEventFunc;
            }

            var functions = target.GetType().GetMethods(BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static);
            List<string> functionNames = new List<string>();
            
            FunctionList = new List<System.Action<EventData>>();
            foreach(var i in functions)
            {
                if(i.ReturnType == typeof(void) && !i.IsConstructor)
                {
                    var param = i.GetParameters();
                    if(param.Length == 1 && param[0].ParameterType == typeof(EventData))
                    {
                        functionNames.Add(i.Name);
                        Action<EventData> func;
                        if (i.IsStatic)
                        {
                            func = (Action<EventData>)Delegate.CreateDelegate(typeof(Action<EventData>), i);
                        }
                        else
                        {
                            func = (Action<EventData>)Delegate.CreateDelegate(typeof(Action<EventData>),comp, i);
                        }
                        FunctionList.Add(func);
                    }
                }
            }

            FunctionNames = functionNames.ToArray();

        }

        public override void OnInspectorGUI()
        {
            //(target as OnEvent).Disconnect();
            Draw();
            if(target.GetType() != typeof(OnEvent))
            {
                this.DrawBaseDefaultInspector();
            }
            serializedObject.ApplyModifiedProperties();
            //(target as OnEvent).Connect();
        }

        public void Draw()
        {
            OnEvent comp = target as OnEvent;


            var optionsRect = EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("Active");
            optionsRect.width = optionsRect.width / 2;
            comp.Active = EditorGUI.Toggle(optionsRect, " ", comp.Active);
            optionsRect.x += optionsRect.width;
            comp.DispatchEvents = EditorGUI.Toggle(optionsRect, "Dispatch Events", comp.DispatchEvents);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.PropertyField(ListenEventProp);
            EditorGUILayout.PropertyField(ListenTargetProp);
            
            var functionRect = EditorGUILayout.BeginHorizontal();
            EditorGUILayout.PrefixLabel("");
            var index = FunctionList.IndexOf(comp.OnEventFunction);

            if (index == -1)
            {
                index = 0;
            }

            comp.OnEventFunction = FunctionList[EditorGUI.Popup(functionRect, "Target Function", index, FunctionNames)];
            EditorGUILayout.EndHorizontal();

            if (comp.DispatchEvents)
            {
                EditorGUILayout.PropertyField(DispatchEventProp);
                EditorGUILayout.PropertyField(DispatchTargetProp);
            }

            
        }
    }
}
#endif