using UnityEngine;
using System.Collections;

public class MinimizeShake : MonoBehaviour {
    Shake myShake = null;
    public float TurnRate = 10f;
	// Use this for initialization
	void Start () {
        myShake = gameObject.GetComponent<Shake>();
	}
	
	// Update is called once per frame
	void Update () {
        //myShake.speed -= 0.005f;

        if(myShake.speed < 0.02)
        {
            myShake.Circular = true;
            myShake.amount += Time.deltaTime / 100;
            myShake.speed = 0.005f;// 1f;
        }
        else
        {
            myShake.speed = Mathf.Lerp(myShake.speed, 0, Time.deltaTime / TurnRate);
        }

        //I want to slowly rotate my object to -90 z

        Quaternion quada = transform.rotation;
        Quaternion quadb = Quaternion.Euler(0, 0, -90);
        //I need the position of an 
        transform.LookAt(transform.localPosition + transform.right);
        //quaternion quadb = transform.LookAt(new Vector3());
        transform.rotation = Quaternion.Slerp(quada, quadb, Time.deltaTime/TurnRate);
	}
}
