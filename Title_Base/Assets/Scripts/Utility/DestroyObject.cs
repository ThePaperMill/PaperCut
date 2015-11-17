using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DestroyObject : MonoBehaviour 
{
  public bool DestroyOnTime                     = false;
  public bool DestroyOnCollide                  = false;
  public bool DestroyOnSelectedComponentCollide = false;
  public bool DestroyOnCollideWithNamedObject   = false;
  public bool DestroyOffCamera                  = false;
  public bool DestroyOnEvent                    = false;

  public List<string> CheckComponents = new List<string>();
  public List<string> OtherObjectNames   = new List<string>();
  public float DestroyTime        = 1.0f;
  public string DestroyEvent = Events.DefaultEvent;

  float DestroyTimer = 0.0f;

	// Use this for initialization
	void Start () 
  {
    if(DestroyEvent != Events.DefaultEvent)
      EventSystem.GlobalHandler.Connect(DestroyEvent, OnDestroyEvent);
	}
	
  void OnDestroyEvent(EventData data)
  {
    DestroySelf();
  }

  void DestroySelf()
  {
    GameObject.Destroy(gameObject);
  }

	// Update is called once per frame
	void Update () 
  {
	  if(DestroyOnTime)
    {
      DestroyTimer += Time.deltaTime;

      if (DestroyTimer > DestroyTime)
      {
        DestroySelf();
        return;
      }
    }
  }

  void OnBecameInvisible()
  {
    if (DestroyOffCamera)
      DestroySelf();
  }

  void OnCollisionEnter(Collision collision) 
  {
    if(DestroyOnCollide)
    {
      DestroySelf();
      return;
    }

    if(DestroyOnCollideWithNamedObject)
    {
      foreach (var i in OtherObjectNames)
        if(collision.collider.gameObject.name == i)
        {
          DestroySelf();
          return;
        }
    }

    if(DestroyOnSelectedComponentCollide)
    {
      foreach (var i in OtherObjectNames)
        if (collision.collider.gameObject.GetComponent(i) != null)
        {
          DestroySelf();
          return;
        }
    }
  }

	
}
