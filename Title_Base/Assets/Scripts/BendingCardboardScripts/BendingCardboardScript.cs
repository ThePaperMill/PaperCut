// © 2015 DigiPen, All Rights Reserved.
//Written by Ian Aemmer

using UnityEngine;
using System.Collections;


public class BendingCardboardScript : MonoBehaviour {

    GameObject Parent = null;

    //public Vector3 begin = Vector3.zero;
    public Vector3 endForward = Vector3.zero;
    public Vector3 endBackwards = Vector3.zero;

    public float DistFromCenterScalar = 1.0f;
    public float Speed = 1.0f;

    GameObject Player = null;
    bool CurrentDirection = true;


    Quaternion Begin = Quaternion.Euler(Vector3.zero);
    //Quaternion End = Quaternion.Euler(Vector3.zero);
    float SliderTime = 0.0f;

    bool HasCollided = false;



    // Use this for initialization
    void Start () {
        Parent = transform.parent.gameObject;

        Begin = Parent.transform.localRotation;
        //End = Quaternion.Euler(endBackwards);

        //Call a function that will bend the cardboard.
        BendCardboard();
	}
	
	// Update is called once per frame
	void Update () {


        

        if (HasCollided == true)
        {
            
            //SliderTime += Time.deltaTime*Speed;

            if (SliderTime > 1)
            {
                SliderTime = 1.0f;
            }
        }
        else if(HasCollided == false && Player != null)
        {
            SliderTime -= Time.deltaTime*Speed;
            if (SliderTime < 0)
            {
                SliderTime = 0.0f;
            }

            if(CurrentDirection)
            {
                Parent.transform.rotation = ActionSystem.ActionMath<Quaternion>.CubicInOut(SliderTime, Begin, Quaternion.Euler(endBackwards), 1);
            }
            else
            {
                Parent.transform.rotation = ActionSystem.ActionMath<Quaternion>.CubicInOut(SliderTime, Begin, Quaternion.Euler(endForward), 1);
            }
            
        }



    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            print("I have ACTUALLY collided with player");
            HasCollided = true;
        }

        
    }
    void OnTriggerStay(Collider other)
    {
        //If the colliding object is the player
        if (other.tag == "Player")
        {
            Player = other.gameObject;

            var DistFromPlayer = Vector3.Distance(this.transform.position, Player.transform.position);
            print(DistFromPlayer);

            SliderTime += Time.deltaTime * Speed / DistFromPlayer;

            //If in editor, draw debug lines for green in this object's local right vector, and blue in this object's local left vector
            Debug.DrawRay(transform.position, transform.right, Color.green);
            Debug.DrawRay(transform.position, transform.right * -1, Color.blue);

            //Get the direction from this object to the colliding player object, then normalize it
            var direction = other.transform.position - transform.position;
            direction = direction.normalized;
            //If in editor, draw debug lines for black towards the player.
            Debug.DrawRay(transform.position, direction, Color.black);

            //Get the dot product between right and to player
            var grn = Vector3.Dot(direction, transform.right);
            //get the dot product between left and to player
            var blu = Vector3.Dot(direction, transform.right * -1);

            //If the dot product of green is larger than the dot product of blue, it means the vector to the player is closer to green
            if (grn > blu)
            {
                //print("green is closer");
                Parent.transform.rotation = Quaternion.Slerp(Begin, Quaternion.Euler(endBackwards), SliderTime);
                CurrentDirection = true;
            }
            //otherwise, The vector to player is closer to blue
            else
            {
               // print("blue is closer");
                Parent.transform.rotation = Quaternion.Slerp(Begin, Quaternion.Euler(endForward), SliderTime);
                CurrentDirection = false;
            }

        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            print("I have STOPPED collided with player");
            HasCollided = false;
            //SliderTime = 0.0f;
            //Parent.transform.rotation = Quaternion.Slerp(Begin, Quaternion.Euler(endForward), 0.0f);
        }
    }
    void BendCardboard()
    {

    }

    bool DetermineCloserVector(Transform other)
    {
        //If in editor, draw debug lines for green in this object's local right vector, and blue in this object's local left vector
        Debug.DrawRay(transform.position, transform.right, Color.green);
        Debug.DrawRay(transform.position, transform.right * -1, Color.blue);

        //Get the direction from this object to the colliding player object, then normalize it
        var direction = other.position - transform.position;
        direction = direction.normalized;
        //If in editor, draw debug lines for black towards the player.
        Debug.DrawRay(transform.position, direction, Color.black);

        //Get the dot product between right and to player
        var grn = Vector3.Dot(direction, transform.right);
        //get the dot product between left and to player
        var blu = Vector3.Dot(direction, transform.right * -1);

        //If the dot product of green is larger than the dot product of blue, it means the vector to the player is closer to green
        if (grn > blu)
        {
            //print("green is closer");
            return true;
        }
        //otherwise, The vector to player is closer to blue
        else
        {
           // print("blue is closer");
            return false;
        }
    }
}
