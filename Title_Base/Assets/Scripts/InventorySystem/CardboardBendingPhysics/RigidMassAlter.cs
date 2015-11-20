using UnityEngine;
using System.Collections;

public class RigidMassAlter : MonoBehaviour 
{
    public Rigidbody rb;
	// Use this for initialization
	void Start () 
  {
        rb = GetComponent<Rigidbody>();
        //print(rb.centerOfMass);
        //rb.centerOfMass = this.transform.position;
        if(rb)
          rb.centerOfMass = Vector3.zero;
    }
}
