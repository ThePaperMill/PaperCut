using UnityEngine;
using System.Collections;

public class OnCollidePush : MonoBehaviour {

    public GameObject ObjectToPush = null;
    bool IsPushing = false;
    public float PushTimer = 5.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
        if(IsPushing == true )
        {
            PushTimer -= Time.deltaTime;
            print(PushTimer);

            Rigidbody rigid = ObjectToPush.GetComponent<Rigidbody>();
            rigid.AddForce(0f, 0f, 1f);
            //ObjectToPush
        }
	}

    void OnTriggerEnter(Collider other)
    {
        //print("wat");
        IsPushing = true;
    }
}
