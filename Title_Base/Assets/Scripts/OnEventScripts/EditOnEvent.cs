using UnityEngine;
using System.Collections;
using ActionSystem;

[System.Serializable]
public class EditOnEvent : OnEvent
{
    public bool DispatchOnFinish = true;
    public bool DeactivateUntilFinished = true;
    
    //public Curve EasingCurve = Ease.Linear;
    protected ActionGroup Actions;
    // Use this for initialization
    public override void Awake ()
    {
        base.Awake();
        Actions = this.GetActions();
        DelayedDispatch = DispatchOnFinish;
        Actions.IsEmpty(); //To avoid warning
	}
	
    public void EditChecks(ActionSequence Seq)
    {
        if (DeactivateUntilFinished)
        {
            Active = false;
            Action.Call(Seq, SetActive, true);
        }
        if (DispatchOnFinish && DispatchEvents)
        {
            Action.Call(Seq, DispatchEvent);
        }
    }
}


#if UNITY_EDITOR
namespace CustomInspector
{
    using UnityEditor;
    using System.Reflection;

    [CanEditMultipleObjects]
    [CustomEditor(typeof(EditOnEvent), true)]
    public class EditOnEventEditor : OnEventEditor
    {

        //SerializedProperty EasingCurve;
        //SerializedProperty DeactivateUntilFinishedProp;

        public override void OnEnable()
        {
            base.OnEnable();

            //EasingCurve = serializedObject.FindProperty("EasingCurve");
            //DeactivateUntilFinishedProp = serializedObject.FindProperty("DeactivateUntilFinished");

        }

        public override void OnInspectorGUI()
        {
            base.Draw();
            
            Draw();
            serializedObject.ApplyModifiedProperties();
            this.DrawBaseDefaultInspector();
            serializedObject.ApplyModifiedProperties();
            //(target as OnEvent).Connect();
        }

        new public void Draw()
        {
            var comp = target as EditOnEvent;
            
            var optionsRect = EditorGUILayout.BeginHorizontal();

            if (comp.DispatchEvents)
            {
                EditorGUILayout.PrefixLabel("DispatchOnFinish");
                optionsRect.width = optionsRect.width / 2;
                comp.DispatchOnFinish = EditorGUI.Toggle(optionsRect, " ", comp.DispatchOnFinish);
                optionsRect.x += optionsRect.width;
                comp.DeactivateUntilFinished = EditorGUI.Toggle(optionsRect, "DeactivateUntilFinish", comp.DeactivateUntilFinished);
            }
            else
            {
                EditorGUILayout.PrefixLabel("DeactivateUntilFinish");
                comp.DeactivateUntilFinished = EditorGUI.Toggle(optionsRect, " ", comp.DeactivateUntilFinished);
            }
            EditorGUILayout.EndHorizontal();

        }
    }
}
#endif