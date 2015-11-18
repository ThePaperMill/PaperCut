// © 2015 DigiPen, All Rights Reserved.
//Written by Ian Aemmer

using UnityEngine;
using System.Collections;




public class PullTab : MonoBehaviour
{
    public bool LocksIntoPlace = true;
    float UnengagedTimer = 1.0f;
    public float LerpBackSpeed = 4.0f;
    
    bool NearPlayer = false;
    GameObject Player = null;

    public bool LockX = true;
    public bool LockY = true;
    public bool LockZ = false;
    bool Engaged = false;
    Rigidbody RBody = null;

    Vector3 StartingPos = Vector3.zero;
    public float distance = 1.0f;

    public float LerpPos = 0.0f;

    Vector3 PosOnStartLock = Vector3.zero;

    // Use this for initialization
    void Start()
    {
        StartingPos = transform.parent.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (NearPlayer == true && Player != null)
        {
            if ((InputManager.GetSingleton.IsButtonTriggered(XINPUT_BUTTONS.BUTTON_X) || InputManager.GetSingleton.IsKeyTriggered(KeyCode.E)) && Player.GetComponent<CustomDynamicController>().IsGrounded() == true)
            {
                print("Pressed a");

                OnLockBody();

            }
            if (InputManager.GetSingleton.IsButtonReleased(XINPUT_BUTTONS.BUTTON_X) || InputManager.GetSingleton.IsKeyReleased(KeyCode.E))
            {
                print("released a");
                OnUnlockBody();
            }

            if ((InputManager.GetSingleton.IsButtonDown(XINPUT_BUTTONS.BUTTON_X) || InputManager.GetSingleton.IsKeyDown(KeyCode.E)) && Engaged == true)
            {
                transform.parent.position = Player.transform.position - PosOnStartLock;

                if (LockX == false)
                {
                    if (transform.parent.position.x > StartingPos.x + distance)
                    {
                        transform.parent.position = new Vector3(StartingPos.x + distance, StartingPos.y, StartingPos.z);
                        Player.transform.position = new Vector3(StartingPos.x + distance + PosOnStartLock.x, Player.transform.position.y, Player.transform.position.z);
                    }

                    if (transform.parent.position.x < StartingPos.x)
                    {
                        transform.parent.position = StartingPos;
                        Player.transform.position = new Vector3(StartingPos.x + PosOnStartLock.x, Player.transform.position.y, Player.transform.position.z);
                    }

                }
                if (LockZ == false)
                {
                    if (transform.parent.position.z > StartingPos.z + distance)
                    {
                        transform.parent.position = new Vector3(StartingPos.x, StartingPos.y, StartingPos.z + distance);
                        Player.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, StartingPos.z + distance + PosOnStartLock.z);
                    }
                    if (transform.parent.position.z < StartingPos.z)
                    {
                        transform.parent.position = StartingPos;
                        Player.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, StartingPos.z + PosOnStartLock.z);
                    }
                }

                LerpPos = Vector3.Distance(transform.parent.position, StartingPos) / distance;

                var Siblings = transform.root.GetComponentsInChildren<PullTabChild>();
                foreach (var sib in Siblings)
                {
                    
                    sib.UpdateRot(LerpPos);
                    print(LerpPos);

                }

            }
        }
        else if (Player != null)
        {
            Player = null;
            OnUnlockBody();
        }
        if (Engaged == false && LerpPos > 0)
        {
            //I need to push the object BACK to origin.
            if(LocksIntoPlace == false)
            {
                transform.parent.position = Vector3.Lerp(StartingPos, transform.parent.position, UnengagedTimer);

            }
            UnengagedTimer -= Time.deltaTime/ LerpBackSpeed;
            LerpPos = Vector3.Distance(transform.parent.position, StartingPos)* distance;
            //LerpPos -= Time.deltaTime;
            if (LerpPos < 0.0f)
            {
                LerpPos = 0.0f;
            }
            if(LerpPos > 0.99f && LocksIntoPlace == false)
            {
                LerpPos = 0.99f;
            }
            print(LerpPos);
            var Siblings = transform.root.GetComponentsInChildren<PullTabChild>();
            foreach (var sib in Siblings)
            {

                if(LocksIntoPlace == false)
                {
                    sib.UpdateRot(LerpPos);
                }
                

            }

        }
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player = other.gameObject;
            print(other.gameObject);
            NearPlayer = true;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            NearPlayer = false;
        }
    }

    void ContrainRBody()
    {
        if(null != Player)
        {
            RBody = Player.GetComponent<Rigidbody>();
        }
        else
        {
            print("this is like not possible bro");
            return;
        }

        if(LockX)
        {
            RBody.constraints |= RigidbodyConstraints.FreezePositionX; //It now freees on position X
            RBody.constraints ^= RigidbodyConstraints.FreezePositionX; //It unfreezes on position X
        }
    }
    void OnLockBody()
    {
        if (Player != null)
        {
            Engaged = true;

            //I need to make all of my siblings lose box collider
            var boxcolliders = transform.root.GetComponentsInChildren<BoxCollider>();
            foreach(var bCol in boxcolliders)
            {
                if(bCol.gameObject != gameObject)
                {
                    bCol.enabled = false;
                }
            }

            //Shrink player body
            


            //Store the player's current position
            //PlayerPosOnStartLock = Player.transform.position;
            PosOnStartLock = Player.transform.position - transform.parent.position;

            //lock all player positions that are supposed to be locked
            var rig = Player.GetComponent<Rigidbody>();
            if (LockX)
            {
                rig.constraints |= RigidbodyConstraints.FreezePositionX; //It now freees on position X
            }
            if (LockY)
            {
                rig.constraints |= RigidbodyConstraints.FreezePositionY; //It now freees on position y
            }
            if (LockZ)
            {
                rig.constraints |= RigidbodyConstraints.FreezePositionZ; //It now freees on position z
            }
        }
    }
    void OnUnlockBody()
    {
        if (Player != null)
        {
            UnengagedTimer = 1;
            print(Time.time);
            //I need to make all of my siblings gain box collider
            var boxcolliders = transform.root.GetComponentsInChildren<BoxCollider>();
            foreach (var bCol in boxcolliders)
            {
                if (bCol.gameObject != gameObject)
                {
                    bCol.enabled = true;
                }
            }
            Engaged = false;
            var rig = Player.GetComponent<Rigidbody>();
            rig.constraints = RigidbodyConstraints.None; // unlock  everything, including rotation

            rig.constraints |= RigidbodyConstraints.FreezeRotation; //It now freees rotation all
        }
    }
}
