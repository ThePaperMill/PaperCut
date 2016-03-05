using UnityEngine;
using System.Collections;
using ActionSystem;

public class OrientToMouse2DOnEvent : OnEvent
{

    // Use this for initialization
    public float Offset = 0.0f;
    public float LerpSpeed = 100f;

	void Start ()
    {
	    
	}
	
	// Update is called once per frame
	public override void OnEventFunc (EventData data)
    {
        
        var aimVec = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
        aimVec = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, Mathf.Atan2(aimVec.y, aimVec.x) * 180 / Mathf.PI + Offset);

        aimVec.z = Mathf.LerpAngle(transform.eulerAngles.z, aimVec.z, LerpSpeed * Time.smoothDeltaTime);

        transform.eulerAngles = aimVec;
    }
}


