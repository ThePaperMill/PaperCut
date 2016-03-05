using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SetTimeScaleOnEvent : OnEvent
{
    [Range(0, 100)]
    public float TimeScale = 0;
	// Use this for initialization
	public override void Awake ()
    {
        base.Awake();
        
	}

    public override void OnEventFunc(EventData data)
    {
        Time.timeScale = TimeScale;
    }

    // Update is called once per frame
    void Update ()
    {
	    
	}
}
