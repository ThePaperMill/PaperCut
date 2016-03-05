using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Reflection;

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
    [HideInInspector]
    public int InputIndex = 0;

    static List<List<InputCodes>> InputLists = new List<List<InputCodes>> ();

    static DispatchEventOnInput()
    {
        var type = typeof(GlobalControls);
        var lists = type.GetFields(BindingFlags.Static | BindingFlags.Public);
        
        foreach (var i in lists)
        {
            if (i.FieldType == typeof(List<InputCodes>))
            {
                InputLists.Add((List<InputCodes>)i.GetValue(null));
            }
        }
    }
    // Use this for initialization
    void Awake ()
    {
	    if(TargetObject.Equals(null))
        {
            TargetObject = gameObject;
        }
        if(InputLists.Count > 0)
        {
            Input = InputLists[InputIndex];
        }
        
	}

    void Start()
    {
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (!Active)
        {
            return;
        }
        
     //   System.Func<List<InputCodes>, bool> inputFunc = InputManager.IsInputTriggered;
	    //switch(InputType)
     //   {
     //       case InputState.Triggered:
     //           {
     //               inputFunc = InputManager.IsInputTriggered;
     //           }
     //           break;
     //       case InputState.Down:
     //           {
     //               inputFunc = InputManager.IsInputDown;
     //           }
     //           break;
     //       case InputState.Released:
     //           {
     //               inputFunc = InputManager.IsInputReleased;
     //           }
     //           break;
     //   }
        
        //if (inputFunc(Input))
        //{
        //    TargetObject.DispatchEvent(DispatchEventName);
        //}
	}
}

#if UNITY_EDITOR
namespace CustomInspector
{
    using UnityEditor;
    using System.Reflection;
    [CanEditMultipleObjects]
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
            if(obj.TargetObject == null)
            {
                obj.TargetObject = obj.gameObject;
            }
            
            
            var rect = EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("");
            obj.Input = InputLists[EditorGUI.Popup(rect, "Input", obj.InputIndex, InputNames)];
            obj.InputIndex = InputLists.IndexOf(obj.Input);
            EditorGUILayout.EndHorizontal();
            //Debug.Log(InputLists.IndexOf(obj.Input));
        }
    }
}
#endif