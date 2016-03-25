using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadSceneOnEvent : OnEvent
{
    public string SceneName;
    
	// Use this for initialization
	public override void Awake ()
    {
        base.Awake();
        
	}

    public override void OnEventFunc(EventData data)
    {
        if(SceneName == "")
        {
            return;
        }
        //LoadSceneMode.)
        SceneManager.LoadScene(SceneName);
    }

    // Update is called once per frame
    void Update ()
    {
	
	}
}
