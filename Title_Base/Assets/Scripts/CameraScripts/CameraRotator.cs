/****************************************************************************/
/*!
\file   CameraTotater.cs
\author Ian aemmer
\brief  
    this does stuff to the camera.
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;

using System.Collections;

public class CameraRotator : MonoBehaviour {
    public GameObject Player = null;
    public float TriggerDistance = 1.0f;
    GameObject CamChild = null;

    Vector3 distfromcamchild = Vector3.zero;

	// Use this for initialization
	void Start () {
        CamChild = transform.GetChild(0).gameObject;
        distfromcamchild = CamChild.transform.localPosition;
	}

    // Update is called once per frame
    void Update()
    {

        if(Player == null)
        {
            //print("Fix this");
            //print("Search level for an object with tag player");
            Player = GameObject.FindGameObjectWithTag("Player");
                //if it exists, set that player to that
            if(Player != null)
            {
                this.transform.position = Player.transform.position;
            }
        }
        else
        {

            //Move the camera on the XZ axis
            SetXZ();
            //Move the camera on the Y axis
            SetY();

            CamChild.transform.LookAt(Player.transform.position);
            Vector3 VecFromPlay = new Vector3(CamChild.transform.position.x - Player.transform.position.x, 0, CamChild.transform.position.z - Player.transform.position.z);
            RaycastHit hit;
            //I need to raycast to camchild from player
            if (Physics.Raycast(new Ray(Player.transform.position, VecFromPlay), out hit, Vector3.Distance(Player.transform.position, transform.position + distfromcamchild)))
            {
                if (hit.collider.tag != "IgnorebyCamera")
                {
                    CamChild.transform.position = hit.point + VecFromPlay.normalized * -1.0f * 0.1f;
                    //print(hit.collider.gameObject.name + "Is blocking camera currently");
                    //print("IgnorebyCamera");
                }

            }
            else
            {
                CamChild.transform.localPosition = Vector3.Lerp(CamChild.transform.localPosition, distfromcamchild, Time.deltaTime);
            }
            //If collision is within 1 unit vector, snap to 0 local z, angle and rotate towards player on this frame, then rotate until outside of 1 unit vector and snap to that.
            //If collision is outside 1 unit vector, snap to the collision spot + 0.2


            //Sent ray from player to cam
            //if collide, zoom camera to that spot + 0.11 
            //Otherwise, interpolate back to normal dist

            /*
            //Detect if the camera is overlapping any object
            Collider[] ObjectsInRange = Physics.OverlapSphere(CamChild.transform.position, 0.5f);
            int i = 0;
            while (i < ObjectsInRange.Length)
            {
                if(ObjectsInRange[i].tag != "IgnorebyCamera")
                {
                    print(ObjectsInRange[i].tag);
                    print(ObjectsInRange[i].name);

                }
                i++;
            }
            */
        }

    }
    void SetXZ()
    {
        //The vector from x to y is given by y−x.
        Vector3 VecToPlayer = new Vector3(Player.transform.position.x - transform.position.x, 0, Player.transform.position.z - transform.position.z);

        //If the player is further from the camera cylinder than TRIGGERDISTANCE, then move the camcylinder
        if (VecToPlayer.magnitude > TriggerDistance)
        {
            //Normalize that vector
            VecToPlayer.Normalize();
            //Invert the vector, so that it points from the player
            VecToPlayer *= -1;
            //and them multiply that vector by the triggerdistance. 
            VecToPlayer *= TriggerDistance;
            //starting from the player's position, use the vector to find the new position CamRotator should be located at.
            Vector3 newPosition = Player.transform.position + VecToPlayer;
            //Neutralize the y position
            newPosition.y = transform.position.y;
            //set the new position
            transform.position = newPosition;
        }
    }
    void SetY()
    {
        //The vector from x to y is given by y−x. Nuetralize the non y values
        Vector3 VecToPlayer = new Vector3(0, Player.transform.position.y - transform.position.y, 0);

        //If the player is further from the camera cylinder than TRIGGERDISTANCE, then move the camcylinder
        if (VecToPlayer.magnitude > TriggerDistance)
        {
            //Normalize that vector
            VecToPlayer.Normalize();
            //Invert the vector, so that it points from the player
            VecToPlayer *= -1;
            //and them multiply that vector by the triggerdistance. 
            VecToPlayer *= TriggerDistance;
            //starting from the player's position, use the vector to find the new position CamRotator should be located at.
            Vector3 newPosition = Player.transform.position + VecToPlayer;
            //Neutralize the x position
            newPosition.x = transform.position.x;
            //Neutralize the z position
            newPosition.z = transform.position.z;
            //set the new position
            transform.position = newPosition;
        }
    }
}
