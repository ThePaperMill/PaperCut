using UnityEngine;
using System.Collections;
using ActionSystem;

public class EditTextColorOnEvent : EditOnEvent
{
    public Color TargetColor = new Color(1, 1, 1, 1);
    public float Duration = 1;
    public Curve EasingCurve = Ease.Linear;
    TextMesh Mesh;
	// Use this for initialization
	public override void Awake ()
    {
        base.Awake();
        Mesh = GetComponent<TextMesh>();

	}

    public override void OnEventFunc(EventData data)
    {
        var Seq = Action.Sequence(Actions);
        
        Action.Property(Seq, Mesh.GetProperty(o => o.color), TargetColor, Duration, EasingCurve);
        EditChecks(Seq);
    }
    
    // Update is called once per frame
    void Update ()
    {
	
	}
}
