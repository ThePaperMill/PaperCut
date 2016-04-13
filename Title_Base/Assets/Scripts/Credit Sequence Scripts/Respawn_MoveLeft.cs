/****************************************************************************/
/*!
\file  Respawn_MoveLeft.cs
\author Ian Aemmer 
\brief  
 
  
    © 2016 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;

public class Respawn_MoveLeft : MonoBehaviour {


    public float DSpeed = 1f;
    public float DLifetime = 15f;

    float timer = 0f;

    public Vector3 MoveVector = Vector3.zero;
    bool isnew = true;

    float lifeRemaining = 100f;
    float randspeed = 0f;

    Vector3 StartPos = Vector3.zero;

	// Use this for initialization
	void Start () {
        StartPos = transform.position;
	}
	
	// Update is called once per frame
	void Update () {


        if(isnew)
        {
            //I need to alter the speed by x
            //I need to alter the lifetime by x
            transform.position = StartPos;

            //I need to randomize the speed
            randspeed = DSpeed + Random.Range(-0.9f, 2);

            isnew = false;
            timer = 0;
        }


        //I need to move right at speed
        transform.position += MoveVector * randspeed * Time.deltaTime;

        timer += Time.deltaTime;

        //if (timer >= DLifetime)
        if(transform.position.x <= -15)
        {

            isnew = true;
        }

	}
}
