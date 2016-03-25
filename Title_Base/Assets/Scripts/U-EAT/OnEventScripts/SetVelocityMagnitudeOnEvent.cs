using UnityEngine;
using System.Collections;

public class SetVelocityMagnitudeOnEvent : OnEvent
{
    public float VelocityMagnitude = 1.0f;
    // Use this for initialization
    Rigidbody2D Body2D;
	void Start ()
    {
        Body2D = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        
    }

    public override void OnEventFunc(EventData data)
    {
        
        Body2D.velocity = Body2D.velocity.normalized * VelocityMagnitude;
    }
}
