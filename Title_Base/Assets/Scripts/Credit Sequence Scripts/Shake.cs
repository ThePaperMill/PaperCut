using UnityEngine;
using System.Collections;

public class Shake : MonoBehaviour {

    public float speed = 1.0f; //how fast it shakes
    public float amount = 0.0f; //how much it shakes

    public bool Circular = false;

    Vector3 StartPos = Vector3.zero;
    
    // Use this for initialization
    void Start () {
        StartPos = transform.position;
	}


    void Update()
    {
        if(Circular)
        {
            transform.position = new Vector3 (StartPos.x +Mathf.Sin(Time.time * amount), StartPos.y+ Mathf.Cos(Time.time * amount), 0) * amount*2;
            //transform.position.x = Mathf.Sin(Time.time * speed);
        }
        else
        {
            Vector2 v2 = Random.insideUnitCircle * speed / 10;
            transform.position = StartPos + new Vector3(v2.x, v2.y, 0);
        }


    }
}
