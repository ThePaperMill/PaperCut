/****************************************************************************/
/*!
\file   OnCollidePush.cs
\author Ian Aemmer
\brief  
    on collide it pushes.
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/

using UnityEngine;
using System.Collections;

public class OnCollidePush : MonoBehaviour {

    bool hasTriggered = false;

    public GameObject ObjectToPush = null;
    bool IsPushing = false;
    public float PushTimer = 5.0f;

    public Vector3 Direction = new Vector3(1f, 1f, 1f);

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
        if(IsPushing == true && ObjectToPush != null)
        {
            PushTimer -= Time.deltaTime;
            //print(PushTimer);

            Rigidbody rigid = ObjectToPush.GetComponent<Rigidbody>();
            //rigid.AddForce(-14f, 2f, 0f, ForceMode.Force);
            rigid.AddForce(Direction, ForceMode.Force);
            //ObjectToPush

        if(PushTimer <= 0.0f)
            {
                Destroy(this.gameObject);
            }
        }
	}

    void OnTriggerEnter(Collider other)
    {
        if (hasTriggered == false)
        {
            //print("Wat");
            hasTriggered = true;

            IsPushing = true;
        }
    }
}
