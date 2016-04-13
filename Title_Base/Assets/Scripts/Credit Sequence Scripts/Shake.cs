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

    public float turnrate = 10;

    Vector3 StartPos = Vector3.zero;
    bool Circular = false;
    
    // Use this for initialization
    void Start () {
        StartPos = transform.localPosition;
	}


    void Update()
    {

            //I need to apply the shake
            Vector2 v2 = Random.insideUnitCircle * speed / turnrate;
            transform.localPosition = StartPos + new Vector3(v2.x, v2.y, 0);

            speed -= Time.deltaTime / 5;
            if (speed <= 0)
            {
                speed = 0;
                Circular = true;
            }
       // }



    }
}
