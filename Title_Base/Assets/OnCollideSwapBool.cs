using UnityEngine;
using System.Collections;

public class OnCollideSwapBool : MonoBehaviour {

    public bool IsOn = false;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter(Collider other)
    {
        print(other.gameObject.tag);
        if (other.gameObject.tag == "Player")
        {
            IsOn = true;
        }
    }
}
