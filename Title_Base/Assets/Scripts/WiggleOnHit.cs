using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider))]
[RequireComponent(typeof(EditorWiggle))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(HingeJoint))]
public class WiggleOnHit : MonoBehaviour {


    Rigidbody rig = null;
    HingeJoint hin = null;
    public float force = 1000f;


    // Use this for initialization
    void Start () {
	
        rig = gameObject.GetComponent<Rigidbody>();

        /*
        hin = gameObject.GetComponent<HingeJoint>();
        if(hin.anchor.y >= 0)
        {
            //print(gameObject.name);
            hin.anchor = new Vector3(hin.anchor.x, -hin.anchor.y, hin.anchor.z);
        }

        if (hin.useSpring == false)
        {
            hin.useSpring = true;
        }

        JointSpring hingeSpring = hin.spring;


        if (hingeSpring.spring <= 0)
        {
            hingeSpring.spring = 20f;
        }
        if(hingeSpring.damper <= 0)
        {
            hingeSpring.damper = 1;
        }


        hin.spring = hingeSpring;
        */
    }

    // Update is called once per frame
    void Update () {
	
	}

    void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            //print("I hit the player working" + Time.time);

            //get the direction from the player
            Vector3 dir = transform.position - col.gameObject.transform.position;

            //add a force from the player towards the plant
            rig.AddForce(dir*1000);
            //asdgasdhol
        }
    }
}
