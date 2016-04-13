// © 2015 DigiPen, All Rights Reserved.
//Written by Ian Aemmer

using UnityEngine;
using System.Collections;


public enum LockedAxis
{
    LocalX,
    LocalY,
    LocalZ
}

public class PullTab : MonoBehaviour
{
    public bool LocksIntoPlace = true;
    float UnengagedTimer = 1.0f;
    public float LerpBackSpeed = 4.0f;
    
    bool NearPlayer = false;
    GameObject Player = null;

    public bool StartPoppedUp = false;
    public LockedAxis Axis = LockedAxis.LocalX;
    bool Engaged = false;
    Rigidbody RBody = null;

    Vector3 StartingPos = Vector3.zero; //where this object spawns at in the level at the start
    public float MaxDistance = 1.0f;

    public float LerpPos = 0.0f;

    Vector3 PosOnStartLock = Vector3.zero;

    public GameObject EventTarget; // the utmost parent of this game object
    float LerpSpeed = 1.8f;
    public float PlayerLerpScalar = 1;
    // Use this for initialization
    FloatEvent LerpData = new FloatEvent();
    GameObject TabBase;
    //FlIP THIS IF THE TAB IS MOVING IN THE WRONG DIRECTION
    public bool DirModPositive = true;
    public bool FlipControls = false;
    public bool UseLeftRightControls = false;
    float DirMulti = 1;
    float YOffset = 1;
    
    GameObject InteractableHightlight = null; 

    //Abandon hope all ye who enter here...
    public bool CreateHighlight = true;
    public bool PoppedUp { get; private set; }
    void Start()
    {
        if(!DirModPositive)
        {
            DirMulti = -1;
        }

        if (CreateHighlight)
            InteractableHightlight = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("PullTabHighlight"));

        if (InteractableHightlight)
        {
            InteractableHightlight.transform.position = transform.position + new Vector3(0, 1, 0);
            InteractableHightlight.SetActive(false);
        }

        //Save the starting position of this game object
        StartingPos = transform.parent.localPosition;
        //Save the Root of this game object
        if(!EventTarget)
        {
            EventTarget = transform.root.gameObject;
        }
        

        //If the object is supposed to start popped up,
        if(StartPoppedUp)
        {
            PoppedUp = true;
            switch (Axis)
            {
                case (LockedAxis.LocalX):
                    {
                        transform.parent.localPosition = new Vector3(StartingPos.x + MaxDistance * DirMulti, StartingPos.y, StartingPos.z);
                    }
                    break;
                case (LockedAxis.LocalY):
                    {
                        transform.parent.localPosition = new Vector3(StartingPos.x, StartingPos.y + MaxDistance * DirMulti, StartingPos.z);
                    }
                    break;
                case (LockedAxis.LocalZ):
                    {
                        transform.parent.localPosition = new Vector3(StartingPos.x, StartingPos.y, StartingPos.z + MaxDistance * DirMulti);
                    }
                    break;
            }
            //set a lerp position
            LerpPos = 0.99f;
            LerpData.value = LerpPos;
            EventTarget.DispatchEvent(Events.TabUpdatedEvent, LerpData);
       }
    }

    // Update is called once per frame
    void Update()
    {
        if (!NearPlayer)
        {
            return;
        }

        if (InteractableHightlight)
        {
            InteractableHightlight.GetComponent<ItemSpin>().StartingPostion = transform.position + new Vector3(0, 1, 0);
            InteractableHightlight.transform.position = new Vector3(transform.position.x, InteractableHightlight.transform.position.y, transform.position.z);
        }

        var controller = Player.GetComponent<CustomDynamicController>();
        if (InputManager.GetSingleton.IsInputTriggered(GlobalControls.TabControls) && controller.IsGrounded() == true)
        {
            AudioSource noise = gameObject.GetComponent<AudioSource>();
            if (noise && !noise.isPlaying)
            {
                GetComponent<AudioSource>().Play();
            }
            OnLockBody();
        }
        else if (InputManager.GetSingleton.IsInputReleased(GlobalControls.TabControls))
        {
            OnUnlockBody();
        }
        if(Engaged)
        {
            bool pushIn;
            bool pushOut;

            if (FlipControls)
            {
                if(UseLeftRightControls)
                {
                    pushIn = controller.MoveLeft;
                    pushOut = controller.MoveRight;
                }
                else
                {
                    pushIn = controller.MoveBack;
                    pushOut = controller.MoveForward;
                }
            }
            else
            {
                if (UseLeftRightControls)
                {
                    pushIn = controller.MoveRight;
                    pushOut = controller.MoveLeft;
                }
                else
                {
                    pushIn = controller.MoveForward;
                    pushOut = controller.MoveBack;
                }
            }


            float speed = 0;
            if(pushIn)
            {
                bool move = false;
                if(DirModPositive)
                {
                    move = GetCurrentVal() <= GetMaxMinVal(false);
                }
                else
                {
                    move = GetCurrentVal() >= GetMaxMinVal(true);
                }
                if(move)
                {
                    speed = LerpSpeed * DirMulti;
                }
            }
            else if (pushOut)
            {
                bool move = false;
                if (DirModPositive)
                {
                    move = GetCurrentVal() >= GetMinVal();
                }
                else
                {
                    move = GetCurrentVal() <= GetMinVal();
                }
                if (move)
                {
                    speed = -LerpSpeed * DirMulti;
                }
            }
            else
            {
                return;
            }
            
            var dir = transform.parent.InverseTransformDirection(new Vector3(1, 0, 0));
            dir *= speed * Time.smoothDeltaTime * DirMulti;
            var pos = Player.transform.position;
            pos += dir * PlayerLerpScalar;
            pos.y = transform.parent.position.y + YOffset;
            Player.transform.position = pos;

            transform.parent.position += dir;
            
            LerpPos = Mathf.Clamp01(1 - Mathf.Abs((GetMaxMinVal(!DirModPositive) - GetCurrentVal()) / MaxDistance));
            
            
            //Debug.Log(GetCurrentVal());
            //Debug.Log(GetMaxMinVal(false));
            if (LerpData.value != LerpPos)
            {
                LerpData.value = LerpPos;
                EventTarget.gameObject.DispatchEvent(Events.TabUpdatedEvent, LerpData);
            }

            if (LerpPos >= 0.99 && !PoppedUp)
            {
                PoppedUp = true;
                transform.root.DispatchEvent("TabOut");
            }
            else if (LerpPos <= 0.01 && PoppedUp)
            {
                PoppedUp = false;
                transform.root.DispatchEvent("TabIn");
            }
        }
        
        //if (NearPlayer == true && Player != null)
        //{
        //    if (InputManager.GetSingleton.IsInputTriggered(GlobalControls.TabControls) && Player.GetComponent<CustomDynamicController>().IsGrounded() == true)
        //    {
        //        //print("Pressed a");

        //        AudioSource noise = gameObject.GetComponent<AudioSource>();
        //        if (noise && !noise.isPlaying)
        //        {
        //            gameObject.GetComponent<AudioSource>().Play();
        //        }

        //        OnLockBody();

        //    }
        //    if (InputManager.GetSingleton.IsInputReleased(GlobalControls.TabControls))
        //    {
        //        //print("released a");
        //        OnUnlockBody();
        //    }

        //    if (InputManager.GetSingleton.IsInputDown(GlobalControls.TabControls) && Engaged == true)
        //    {
        //        transform.parent.position = Player.transform.position - PosOnStartLock;

        //        if (LockX == false)
        //        {
        //            if (transform.parent.position.x > StartingPos.x + distance)
        //            {
        //                transform.parent.position = new Vector3(StartingPos.x + distance, StartingPos.y, StartingPos.z);
        //                Player.transform.position = new Vector3(StartingPos.x + distance + PosOnStartLock.x, Player.transform.position.y, Player.transform.position.z);
        //            }

        //            if (transform.parent.position.x < StartingPos.x)
        //            {
        //                transform.parent.position = StartingPos;
        //                Player.transform.position = new Vector3(StartingPos.x + PosOnStartLock.x, Player.transform.position.y, Player.transform.position.z);
        //            }

        //        }
        //        if (LockZ == false)
        //        {
        //            if (transform.parent.position.z > StartingPos.z + distance)
        //            {
        //                transform.parent.position = new Vector3(StartingPos.x, StartingPos.y, StartingPos.z + distance);
        //                Player.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, StartingPos.z + distance + PosOnStartLock.z);
        //            }
        //            if (transform.parent.position.z < StartingPos.z)
        //            {
        //                transform.parent.position = StartingPos;
        //                Player.transform.position = new Vector3(Player.transform.position.x, Player.transform.position.y, StartingPos.z + PosOnStartLock.z);
        //            }
        //        }

        //        LerpPos = Vector3.SqrMagnitude(transform.parent.position - StartingPos) / (distance * distance);
        //        if (LerpData.value != LerpPos)
        //        {
        //            LerpData.value = LerpPos;
        //            EventTarget.gameObject.DispatchEvent(Events.TabUpdatedEvent, LerpData);
        //        }
        //        //#SHAME #SHAME #SHAME #SHAME #SHAME #SHAME #SHAME #SHAME #SHAME #SHAME #SHAME #SHAME
        //        //#SHAME #SHAME #SHAME #SHAME #SHAME #SHAME #SHAME #SHAME #SHAME #SHAME #SHAME #SHAME
        //        //#SHAME #SHAME #SHAME #SHAME #SHAME #SHAME #SHAME #SHAME #SHAME #SHAME #SHAME #SHAME
        //        //#SHAME var Siblings = transform.root.GetComponentsInChildren<PullTabChild>();#SHAME
        //        //#SHAME #SHAME #SHAME #SHAME #SHAME #SHAME #SHAME #SHAME #SHAME #SHAME #SHAME #SHAME
        //        //#SHAME #SHAME #SHAME #SHAME #SHAME #SHAME #SHAME #SHAME #SHAME #SHAME #SHAME #SHAME
        //        //#SHAME #SHAME #SHAME #SHAME #SHAME #SHAME #SHAME #SHAME #SHAME #SHAME #SHAME #SHAME

        //        //foreach (var sib in Siblings)
        //        //{

        //        //    sib.UpdateRot(LerpPos);
        //        //    print(LerpPos);

        //        //}

        //    }
        //}
        //else if (Player != null)
        //{
        //    Player = null;
        //    OnUnlockBody();
        //}
        //if (Engaged == false && LerpPos > 0)
        //{
        //    //I need to push the object BACK to origin.
        //    if(LocksIntoPlace == false)
        //    {
        //        transform.parent.position = Vector3.Lerp(StartingPos, transform.parent.position, UnengagedTimer);

        //    }
        //    UnengagedTimer -= Time.deltaTime/ LerpBackSpeed;
        //    LerpPos = Vector3.SqrMagnitude(transform.parent.position - StartingPos) / (distance * distance);
        //    //LerpPos -= Time.deltaTime;
        //    if (LerpPos < 0.0f)
        //    {
        //        LerpPos = 0.0f;
        //    }
        //    if(LerpPos > 0.99f && LocksIntoPlace == false)
        //    {
        //        LerpPos = 0.99f;
        //    }
        //    if(LerpData.value != LerpPos)
        //    {
        //        LerpData.value = LerpPos;
        //        EventTarget.DispatchEvent(Events.TabUpdatedEvent, LerpData);
        //    }



        //}
    }
    float GetCurrentVal()
    {
        switch(Axis)
        {
            case (LockedAxis.LocalX):
            {
                 return transform.parent.localPosition.x;
            }
            case (LockedAxis.LocalY):
            {
                 return transform.parent.localPosition.y;
            }
            case (LockedAxis.LocalZ):
            {
                 return transform.parent.localPosition.z;
            }
        }
        return 0;
    }

    float GetMaxMinVal(bool minusDistance = false)
    {
        float multiplierDist = 1;
        if(minusDistance)
        {
            multiplierDist = -1;
        }
        multiplierDist = multiplierDist * MaxDistance;
        switch (Axis)
        {
            case (LockedAxis.LocalX):
                {
                    return StartingPos.x + multiplierDist;
                }
            case (LockedAxis.LocalY):
                {
                    return StartingPos.y + multiplierDist;
                }
            case (LockedAxis.LocalZ):
                {
                    return StartingPos.z + multiplierDist;
                }
        }
        return 0;
    }

    float GetMinVal()
    {
        switch (Axis)
        {
            case (LockedAxis.LocalX):
                {
                    return StartingPos.x;
                }
            case (LockedAxis.LocalY):
                {
                    return StartingPos.y;
                }
            case (LockedAxis.LocalZ):
                {
                    return StartingPos.z;
                }
        }
        return 0;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player = other.gameObject;
            //print(other.gameObject);
            NearPlayer = true;

            if (InteractableHightlight)
            {
                InteractableHightlight.transform.position = transform.position + new Vector3(0, 1, 0);
                InteractableHightlight.SetActive(true);
            }
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "Player")
        {
            NearPlayer = false;

            if (InteractableHightlight)
            {
                InteractableHightlight.transform.position = transform.position + new Vector3(0, 1, 0);
                InteractableHightlight.SetActive(false);
                InteractableHightlight.GetComponent<ItemSpin>().StartingPostion = transform.position + new Vector3(0, 1, 0);
            }
        }
        if (Engaged)
        {
            OnUnlockBody();
            Engaged = false;
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

        //if(LockX)
        //{
        //    RBody.constraints |= RigidbodyConstraints.FreezePositionX; //It now freees on position X
        //    RBody.constraints ^= RigidbodyConstraints.FreezePositionX; //It unfreezes on position X
        //}
    }
    void OnLockBody()
    {
        Engaged = true;

        //Store the player's current position
        //PlayerPosOnStartLock = Player.transform.position;
        PosOnStartLock = Player.transform.position - transform.parent.position;

        //lock all player positions that are supposed to be locked
        var body = Player.GetComponent<Rigidbody>();
        body.constraints = RigidbodyConstraints.FreezeAll;
        YOffset = Player.transform.position.y - transform.parent.position.y;
        
    }
    void OnUnlockBody()
    {
        UnengagedTimer = 1;
        Engaged = false;
        var body = Player.GetComponent<Rigidbody>();
        body.constraints = RigidbodyConstraints.FreezeRotation;
    }
}
