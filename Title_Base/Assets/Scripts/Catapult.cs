using UnityEngine;
using System.Collections;

public class Catapult : MonoBehaviour
{
    public Vector3 LaunchVector;
    public float MaxForce;
    public float BoostValue;

    private float CurrentForce;
    private float Timer;
    private bool Launched;
    private GameObject Player;

	// Use this for initialization
	void Start ()
    {

	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Launched == true)
        {
            Timer += Time.deltaTime;

            if (Timer >= 1)
            {
                Timer = 0;
                Launched = false;
                Player.GetComponent<CustomDynamicController>().Active = true;
            }
        }
	}

    void OnCollisionStay(Collision otherObj)
    {
        if (otherObj.gameObject.tag == "Player")
        {
            CurrentForce += otherObj.gameObject.GetComponent<Rigidbody>().mass * BoostValue;

            if(CurrentForce > MaxForce)
            {
                CurrentForce = MaxForce;
            }
            print(CurrentForce);
        }
    }

    void OnCollisionExit(Collision otherObj)
    {
        if(otherObj.gameObject.tag == "Player")
        {
            Player = otherObj.gameObject;
            Player.GetComponent<CustomDynamicController>().Active = false;
            Player.GetComponent<Rigidbody>().AddForce(LaunchVector * CurrentForce);
            CurrentForce = 0;
            Launched = true;
        }
    }
}