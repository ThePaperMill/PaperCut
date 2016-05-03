// © 2015 DigiPen, All Rights Reserved.
//Written by Ian Aemmer

using UnityEngine;
using System.Collections;


[RequireComponent(typeof(Collider))]
public class PullTabParent : MonoBehaviour
{
    [Tooltip("Whether or not the tab should start pulled all the way out and be pushed in instead of pulled out.")]
    public bool StartPoppedUp = false;
    [Tooltip("The distance the tab is to be pulled out/pushed in.")]
    public float PullingDistance = 0.75f;
    [Tooltip("Whether or not the tab slowly lerps back to its original position.")]
    public bool LocksIntoPlace = true;
    [Tooltip("How fast the pull tab lerps back to its starting position.")]
    public float LerpBackSpeed = 4.0f;
    [Tooltip("How fast the player player moves when pulling the tab. Used primarily when multiple tabs are in close proximity.")]
    public float PlayerLerpScalar = 1;
    [Tooltip("Where the \"TabUpdatedEvent\" messages are sent. Default: Root")]
    public GameObject EventTarget;
    public bool PoppedUp { get; private set; }

    float UnengagedTimer = 1.0f;
    float UnegagedTimeElapsed = 0;
    bool NearPlayer = false;
    GameObject Player = null;
    float PlayerYOffset = 1;
    bool Engaged = false;
    Vector3 StartingPos = Vector3.zero; //where this object spawns at in the level at the start
    float LerpPos = 0.0f; //Percentage that the tab is pulled out.
    float LerpSpeed = 1.8f;
    float MinDistanceFromWall = 0.2f; //How close to a wall can the player get?
    float PulledOutDistance
    {
        get
        {
            return StartingPos.x + PullingDistance;
        }
    }
    // Use this for initialization
    FloatEvent LerpData = new FloatEvent();
    GameObject InteractableHightlight = null; 
    bool CreateHighlight = true;
    
    void Start()
    {
        if(!transform.parent)
        {
            throw new System.Exception("YO, THE PULL-TAB PARENT NEEDS A PARENT, DUMBO!");
        }
        if (CreateHighlight)
        {
            InteractableHightlight = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("PullTabHighlight"));
        }   
        if (InteractableHightlight)
        {
            InteractableHightlight.transform.position = transform.position + new Vector3(0, 1, 0);
            InteractableHightlight.SetActive(false);
        }

        //Save the starting position of this game object
        StartingPos = transform.localPosition;
        //Save the Root of this game object
        if(!EventTarget)
        {
            EventTarget = transform.root.gameObject;
        }
        

        //If the object is supposed to start popped up,
        if(StartPoppedUp)
        {
            PoppedUp = true;
            transform.localPosition = new Vector3(PulledOutDistance, StartingPos.y, StartingPos.z);
            //set a lerp position
            SendTabUpdatedMessage();
       }
    }

    // Update is called once per frame
    void Update()
    {
        if (!NearPlayer)
        {
            LerpBack();
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
            UnegagedTimeElapsed = 0;
            Vector3 playerMoveDirection = new Vector3();
            if(controller.MoveForward)
            {
                playerMoveDirection += Player.transform.forward;
            }
            else if(controller.MoveBack)
            {
                playerMoveDirection -= Player.transform.forward;
            }
            if (controller.MoveLeft)
            {
                playerMoveDirection -= Player.transform.right;
            }
            else if (controller.MoveRight)
            {
                playerMoveDirection += Player.transform.right;
            }
            //Get the players movement direction according to our parent's local coordinates.
            playerMoveDirection = transform.parent.InverseTransformDirection(playerMoveDirection);
            playerMoveDirection.z = 0; //We don't need the Z anymore.
            
            //The x value of the playerMoveDIrection vector tells us whether or not the tab is getting pushed in. (Positive is out negative is in)
            float speed = playerMoveDirection.x * LerpSpeed * Time.smoothDeltaTime;
            //Modify the player's world position to move along OUR local x axis.
            var dir = transform.parent.TransformDirection(new Vector3(1, 0, 0));
            dir *= speed;
            var pos = Player.transform.position;
            pos += dir * PlayerLerpScalar;
            pos.y = transform.parent.position.y + PlayerYOffset;
            
            transform.position += dir;

            var localPos = transform.localPosition;
            if (localPos.x <= StartingPos.x)
            {
                localPos.x = StartingPos.x;
            }
            else if(localPos.x >= StartingPos.x + PullingDistance)
            {
                localPos.x = StartingPos.x + PullingDistance;
            }
            else
            {
                //To prevent cheating.
                if(!Physics.Raycast(new Ray(Player.transform.position, dir), MinDistanceFromWall))
                {
                    Player.transform.position = pos;
                }
            }
            transform.localPosition = localPos;
            SendTabUpdatedMessage();
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
        else
        {
            LerpBack();
        }

    }

    void LerpBack()
    {
        if (UnegagedTimeElapsed <= UnengagedTimer)
        {
            UnegagedTimeElapsed += Time.deltaTime;
            return;
        }
        var localPos = transform.localPosition;
        if (!LocksIntoPlace)
        {
            float startingPosX;
            if (StartPoppedUp)
            {
                startingPosX = PulledOutDistance;
            }
            else
            {
                startingPosX = StartingPos.x;
            }
            localPos.x = Mathf.Lerp(localPos.x, startingPosX, LerpBackSpeed * Time.smoothDeltaTime);
            SendTabUpdatedMessage();
        }
        transform.localPosition = localPos;
    }

    void SendTabUpdatedMessage()
    {
        LerpPos = Mathf.Clamp01(1 - (PulledOutDistance - transform.localPosition.x) / PullingDistance);
        if (LerpData.value != LerpPos)
        {
            LerpData.value = LerpPos;
            EventTarget.gameObject.DispatchEvent(Events.TabUpdatedEvent, LerpData);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player = other.gameObject;
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

    void OnLockBody()
    {
        Engaged = true;
        //lock all player positions that are supposed to be locked
        var body = Player.GetComponent<Rigidbody>();
        body.constraints = RigidbodyConstraints.FreezeAll;
        PlayerYOffset = Player.transform.position.y - transform.parent.position.y;
        
    }
    void OnUnlockBody()
    {
        UnengagedTimer = 1;
        Engaged = false;
        var body = Player.GetComponent<Rigidbody>();
        body.constraints = RigidbodyConstraints.FreezeRotation;
    }
}