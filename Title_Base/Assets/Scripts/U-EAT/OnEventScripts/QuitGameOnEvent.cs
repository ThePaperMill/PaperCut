using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class QuitGameOnEvent : OnEvent
{
    
	// Use this for initialization
	public override void Awake ()
    {
        base.Awake();
        
	}

    public override void OnEventFunc(EventData data)
    {
        Application.Quit();
    }

    // Update is called once per frame
    void Update ()
    {
	    
	}
}
