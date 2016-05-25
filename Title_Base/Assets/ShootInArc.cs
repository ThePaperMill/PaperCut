using UnityEngine;
using System.Collections;

public class ShootInArc : MonoBehaviour
{
    private float Timer;

    [SerializeField]
    Transform target;
    [SerializeField]
    private GameObject ObjectToShoot;
    [SerializeField]
    private float ShootRate;
    [SerializeField]
    float initialAngle;

    private Vector3 ThrowVector;

    // Use this for initialization
    void Start ()
    {
        ThrowVector = CalculateThrow();
    }
	
	// Update is called once per frame
	void Update ()
    {
        Timer += Time.fixedDeltaTime;

        if(Timer >= ShootRate)
        {
            GameObject ShotObject = Instantiate(ObjectToShoot, transform.position + new Vector3(0,0.1f,0), transform.rotation) as GameObject;
            ShotObject.GetComponent<Rigidbody>().velocity = ThrowVector;

            Timer = 0;
        }
	}

    Vector3 CalculateThrow()
    {
        Vector3 p = target.position;

        float gravity = Physics.gravity.magnitude;
        // Selected angle in radians
        float angle = initialAngle * Mathf.Deg2Rad;

        // Positions of this object and the target on the same plane
        Vector3 planarTarget = new Vector3(p.x, 0, p.z);
        Vector3 planarPostion = new Vector3(transform.position.x, 0, transform.position.z);

        // Planar distance between objects
        float distance = Vector3.Distance(planarTarget, planarPostion);
        // Distance along the y axis between objects
        float yOffset = transform.position.y - p.y;

        float initialVelocity = (1 / Mathf.Cos(angle)) * Mathf.Sqrt((0.5f * gravity * Mathf.Pow(distance, 2)) / (distance * Mathf.Tan(angle) + yOffset));
        
        Vector3 velocity = new Vector3(0, initialVelocity * Mathf.Sin(angle), initialVelocity * Mathf.Cos(angle));

        // Rotate our velocity to match the direction between the two objects
        float angleBetweenObjects = Vector3.Angle(Vector3.forward, planarTarget - planarPostion);
        Vector3 finalVelocity = Quaternion.AngleAxis(angleBetweenObjects, Vector3.up) * velocity;
        
        if(planarTarget.x < planarPostion.x)
        {
            finalVelocity.x = -finalVelocity.x;
        }

        return finalVelocity;
    }
}