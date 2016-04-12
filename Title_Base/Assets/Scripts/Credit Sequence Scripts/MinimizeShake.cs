/****************************************************************************/
/*!
\file  MinimizeShake.cs
\author Ian Aemmer
\brief  
 
  
    © 2016 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;

public class MinimizeShake : MonoBehaviour {
   // Shake myShake = null;
    public float TurnRate = 10f;
    Vector3 StartPos = Vector3.zero;

    // Use this for initialization
    void Start () {
       // myShake = gameObject.GetComponent<Shake>();
        StartPos = transform.position;

    }

    // Update is called once per frame
    void Update () {
        //myShake.speed -= 0.005f;

        //if(myShake.speed < 0.02)
        //{
            //myShake.Circular = true;
            //I need to increment myshake.amount to be on a counter rather than a flat rate
            //myShake.amount += Time.deltaTime / 100;
            //myShake.speed = 0.005f;// 1f;
        //}
        //else
        //{
            //myShake.speed = Mathf.Lerp(myShake.speed, 0, Time.deltaTime / TurnRate);
        //}

        //I want to slowly rotate my object to -90 z

        Quaternion quada = transform.rotation;
        Quaternion quadb = Quaternion.Euler(0, 0, -90);
        //I need the position of an 
        transform.LookAt(transform.localPosition + transform.right);
        //quaternion quadb = transform.LookAt(new Vector3());
        transform.rotation = Quaternion.Slerp(quada, quadb, Time.deltaTime/TurnRate);

        //I want my position to move left and right
        transform.localPosition = new Vector3( 0, Mathf.PingPong(Time.time, 3) - 1.5f, StartPos.z);
        
        //what do I want to happen?
        //I want the circular motion throughout, and I want it to be based on my starting position. This means it needs to be constant, every frame
        //I want the shake to start off rough but interpolate to smooth/not at all
        //I want the direction to start going up but slowly turn tothe right
	}
}
