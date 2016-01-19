using UnityEngine;
using System.Collections;

public class Game : MonoBehaviour
{
    static public GameObject GameSession;
	// Use this for initialization
	void Awake ()
    {
        var obj = GameObject.FindGameObjectWithTag("GameSession");
        if(obj.Equals(null))
        {
            obj = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Prefabs/GameSession"));
        }
        GameSession = obj;
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
