using UnityEngine;
using System.Collections;
using ActionSystem;
public class EditScaleOnEvent : OnEvent
{
    public Vector3 TargetScale = new Vector3(1,1,1);
    public Vector3 ScaleDurations = new Vector3(1, 1, 1);
    public Curve[] Eases = { Ease.Linear, Ease.Linear, Ease.Linear };
    // Use this for initialization
    void Start ()
    {
	
	}

    public override void OnEventFunc(EventData data)
    {

    }

    // Update is called once per frame
    void Update ()
    {
	
	}
}
