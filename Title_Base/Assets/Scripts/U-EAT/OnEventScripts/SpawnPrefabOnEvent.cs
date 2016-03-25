using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SpawnPrefabOnEvent : OnEvent
{
    public GameObject ObjectToSpawn = null;
    public bool Additive = false;
    public Vector3 SpawnPosition = new Vector3();
    public GameObject Parent = null;
	// Use this for initialization
	public override void Awake ()
    {
        base.Awake();
	}

    public override void OnEventFunc(EventData data)
    {
       if(!ObjectToSpawn)
       {
            throw new System.Exception(this.GetType().Name + " on object " + gameObject.name + " has no object to spawn.");
       }

        var obj = Instantiate(ObjectToSpawn);
        var pos = SpawnPosition;
        if (Additive)
        {
            pos += gameObject.transform.position;
        }
        if(Parent)
        {
            obj.transform.parent = Parent.transform;
        }
        obj.transform.position = pos;
        
    }
}
