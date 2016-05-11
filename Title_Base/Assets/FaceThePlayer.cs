using UnityEngine;
using System.Collections;

public class FaceThePlayer : MonoBehaviour
{
    GameObject Player = null;
    Quaternion TargetRot = Quaternion.identity;

    public float FaceDistance = 1.5f;

	// Use this for initialization
	void Start ()
    {
        Player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update ()
    {
	    if(Player)
        {
            Vector3 pos = Player.transform.position - transform.position;
            float test = Vector3.Distance(Player.transform.position, transform.position);

            if (pos.magnitude < FaceDistance)
            {
                pos = new Vector3(pos.x, 0, pos.z);
                TargetRot = Quaternion.LookRotation(pos);
            }
        }
        else
        {
            Player = GameObject.FindGameObjectWithTag("Player");
        }


        transform.rotation = Quaternion.Slerp(transform.rotation, TargetRot, 3.0f*Time.smoothDeltaTime);
    }
	
}
