using UnityEngine;
using System.Collections;

public class Shake : MonoBehaviour {

    public float speed = 1.0f; //how fast it shakes
    public float amount = 1.0f; //how much it shakes

    Vector3 StartPos = Vector3.zero;
    
    // Use this for initialization
    void Start () {
        StartPos = transform.position;
	}


    void Update()
    {
        Vector2 v2 = Random.insideUnitCircle *speed;
        transform.position = StartPos + new Vector3(v2.x, v2.y, 0);
        //transform.position = new Vector3 (StartPos.x +Mathf.Sin(Time.time * speed), StartPos.y+ Mathf.Cos(Time.time * speed), 0);
        //transform.position.x = Mathf.Sin(Time.time * speed);
    }
}
