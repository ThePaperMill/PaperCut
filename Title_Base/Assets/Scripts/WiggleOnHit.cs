using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(HingeJoint))]
public class WiggleOnHit : MonoBehaviour {


    Rigidbody rig = null;
    public float force = 1000f;


    // Use this for initialization
    void Start () {
	
        rig = gameObject.GetComponent<Rigidbody>();
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
