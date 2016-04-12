/****************************************************************************/
/*!
\file  Shake.cs
\author Ian Aemmer 
\brief  
 
  
    © 2016 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;

public class Shake : MonoBehaviour {

    public float speed = 1.0f; //how fast it shakes
    public float amount = 0.0f; //how much it shakes
    float circstart = 0.0f;

    public bool decelerating = false;

    Vector3 StartPos = Vector3.zero;
    bool Circular = false;
    
    // Use this for initialization
    void Start () {
        StartPos = transform.position;
	}


    void Update()
    {


        //Slowly increment my amount (distance for circular motion) to a max level
        //amount += 0.01f;



        /*
        if(Circular)
        {


            if (circstart >= 1)
            {
                decelerating = true;

            }
            if(decelerating)
            {
                circstart -= Time.deltaTime / 5;
                if (circstart <= 0)
                {
                    circstart = 0;
                }
            }
            else
            {
                circstart += Time.deltaTime / 5;
            }

            amount = Mathf.Lerp(0, 1f, circstart);

            //Apply circular rotation 
            transform.position = new Vector3(StartPos.x + Mathf.Sin(Time.time * amount), StartPos.y + Mathf.Cos(Time.time * amount), 0) * amount * 2;
            transform.position += new Vector3(0, 0, StartPos.z);

            //I need to apply the shake
            Vector2 v2 = Random.insideUnitCircle * speed / 10;
            transform.position = transform.localPosition + new Vector3(v2.x, v2.y, 0);
            //I want to space myself. my space from 
        }*///
        //else
        //{
            //I need to apply the shake
            Vector2 v2 = Random.insideUnitCircle * speed / 10;
            transform.position = StartPos + new Vector3(v2.x, v2.y, StartPos.z);

            speed -= Time.deltaTime / 5;
            if (speed <= 0)
            {
                speed = 0;
                Circular = true;
            }
       // }



    }
}
