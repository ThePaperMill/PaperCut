using UnityEngine;
using System.Collections;
using ActionSystem;
public class DestroyOnEvent : OnEvent
{
	void Start()
    {
        
	}

    public override void OnEventFunc(EventData data)
    {
        Destroy(gameObject);
    }

}
