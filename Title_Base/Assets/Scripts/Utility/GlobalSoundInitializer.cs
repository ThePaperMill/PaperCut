using UnityEngine;
using System.Collections;

public class GlobalSoundInitializer : Singleton<GlobalSoundInitializer>
{
    public bool FmodSoundInitialzied = false;

	// Use this for initialization
	void awake ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    public void Initialize()
    {
        GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("UndyingMusic"));
        FmodSoundInitialzied = true;
    }
}
