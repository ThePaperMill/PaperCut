using UnityEngine;
using System.Collections;

public class WaterdropAdjust : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
        transform.up = -GetComponent<Rigidbody>().velocity;
	}

    void OnTriggerEnter(Collider ce_)
    {
        if (ce_.gameObject.tag != "Finish")
        {
            Destroy(gameObject);
        }
    }
}