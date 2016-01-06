using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public enum InputState
{
    Triggered,
    Down,
    Released
}
//[Serializable]
public class DispatchEventOnInput : MonoBehaviour
{
    public bool Active = true;
    public Events DispatchEventName;
    public GameObject TargetObject;
    public InputState InputType = InputState.Triggered;
    public List<InputCodes> Input;
	// Use this for initialization
	void Awake ()
    {
	    if(TargetObject.Equals(null))
        {
            TargetObject = gameObject;
        }
        
	}

    void Start()
    {
        Game.GameSession.Connect(Events.LogicUpdate, OnLogicUpdate);
    }
	
	// Update is called once per frame
	void OnLogicUpdate (EventData data)
    {
        if (!Active)
        {
            return;
        }
        
        System.Func<List<InputCodes>, bool> inputFunc = InputManager.GetSingleton.IsInputTriggered;
	    switch(InputType)
        {
            case InputState.Triggered:
                {
                    inputFunc = InputManager.GetSingleton.IsInputTriggered;
                }
                break;
            case InputState.Down:
                {
                    inputFunc = InputManager.GetSingleton.IsInputDown;
                }
                break;
            case InputState.Released:
                {
                    inputFunc = InputManager.GetSingleton.IsInputReleased;
                }
                break;
        }
        
        if (inputFunc(Input))
        {
            
            TargetObject.DispatchEvent(DispatchEventName);
        }
	}
}

#if UNITY_EDITOR
namespace CustomInspector
{
    using UnityEditor;
    using System.Reflection;
    [CustomEditor(typeof(DispatchEventOnInput))]
    public class DispatchEventOnInputDrawer : Editor
    {
        string[] InputNames = { };
        List<List<InputCodes>> InputLists = new List<List<InputCodes>>();
        void OnEnable()
        {
            var type = typeof(GlobalControls);
            var lists = type.GetFields(BindingFlags.Static | BindingFlags.Public);
            List<string> inputNames = new List<string>();
            foreach (var i in lists)
            {
                if(i.FieldType ==  typeof(List<InputCodes>))
                {
                    inputNames.Add(i.Name);
                    InputLists.Add((List<InputCodes>)i.GetValue(null));
                }
            }
            InputNames = inputNames.ToArray();
            //ChooseInputList();
            
        }
        
        public override void OnInspectorGUI()
        {
            DrawDefaultInspector();
            ChooseInputList();
            
        }

        void ChooseInputList()
        {
            if(InputNames.Length == 0)
            {
                return;
            }
            var obj = target as DispatchEventOnInput;
            var index = InputLists.IndexOf(obj.Input);
            //Debug.Log(obj.Input);
            if (index == -1)
            {
                index = 0;
            }
            
            var rect = EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("");
            obj.Input = InputLists[EditorGUI.Popup(rect, "Input", index, InputNames)];
            EditorGUILayout.EndHorizontal();
            //Debug.Log(InputLists.IndexOf(obj.Input));
        }
    }
}
#endif