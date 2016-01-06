using UnityEngine;
using System.Collections;

public class EditColorOnEvent : OnEvent
{
    public Color TargetColor = new Color(1,1,1,1);
	// Use this for initialization
	void Start ()
    {
        
	}
	
    public override void OnEventFunc(EventData data)
    {
        gameObject.GetComponent<SpriteRenderer>().color = TargetColor;
    }
	// Update is called once per frame
	void Update ()
    {
	
	}


}

