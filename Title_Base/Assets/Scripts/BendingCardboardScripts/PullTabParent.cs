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
    Animator PlayerAnim = null;
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
    [Tooltip("This is used to play make the ease for the cardboard.")]
    [SerializeField]
    AnimationCurve ac;

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
        //If my collider doesn't have a player in it
        if (!NearPlayer)
        {
            LerpBack();
            //skip the update loop
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
//here
        if(Engaged)
        {
            //print(controller.MoveForward);
            UnegagedTimeElapsed = 0;
            Vector3 playerMoveDirection = new Vector3();

            
            Vector2 Lstick = new Vector2(InputManager.GetSingleton.GetLeftStickValues().XPos, InputManager.GetSingleton.GetLeftStickValues().YPos);
            if(Lstick == Vector2.zero)
            {

                if (controller.MoveForward)
                {
                    playerMoveDirection += Player.transform.forward;
                }
                else if (controller.MoveBack)
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
            }
            else
            {
                playerMoveDirection = new Vector3(Lstick.x, 0, Lstick.y);
            }
            //playerMoveDirection.Normalize();
            
            /*
            if (controller.MoveForward)
            {
                playerMoveDirection += Player.transform.forward;
            }
            else if (controller.MoveBack)
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
            }*/

            //print (InputManager.GetSingleton.GetLeftStickValues().XPos) ;

            //I need to raycast the direction the stick is pushing from the player
            // I need to raycast the direction(s) of the pull tab
            Vector3 forward = transform.TransformDirection(Vector3.right);
            Debug.DrawRay(transform.position, forward, Color.green);

            Vector3 backward = transform.TransformDirection(Vector3.left);
            Debug.DrawRay(transform.position, backward, Color.blue);

            //Debug.DrawRay(transform.position, playerMoveDirection, Color.yellow);
            Vector3 direction = Camera.main.transform.TransformDirection(playerMoveDirection);

            //direction = transform.TransformDirection(direction);
            //Debug.DrawRay(transform.position, direction, Color.white);

            //I need to swap the color of the stick pushing from the player if it's closer betwen the two directions

            /*
            I have the vector and the inverse
            I can grab the screentopoint of the the center and the end position



            I need an accurate move vector to test against

            Dot/cross product to determine if my move vector is more towards one angle or another

            The end goal: Determine if the vector my leftstick is in is more towards one angle or the other
            */

            float ford = Vector3.Dot(direction, forward);
            float bacd = Vector3.Dot(direction, backward);
            //print(ford + " is the forward vector " + bacd + " is the backward vector");
            //print(playerMoveDirection);
            if(playerMoveDirection != Vector3.zero && Mathf.Abs(ford - bacd) > 0.5)
            {
                //print(Mathf.Abs(ford - bacd));
                if (ford > bacd)
                {
                    Debug.DrawRay(transform.position, direction, Color.green);
                    playerMoveDirection = new Vector3(1, 0, 0);//we don't need the other vectors
                }
                else if(bacd > ford)
                {
                    Debug.DrawRay(transform.position, direction, Color.blue);
                    playerMoveDirection = new Vector3(-1, 0, 0); // we dont need the other vectors

                }
            }
            else
            {
                playerMoveDirection = Vector3.zero;
            }
            

            
            playerMoveDirection.Normalize();
            playerMoveDirection *= PlayerLerpScalar;

            //The x value of the playerMoveDIrection vector tells us whether or not the tab is getting pushed in. (Positive is out negative is in)
            float speed = playerMoveDirection.x * PlayerLerpScalar * Time.smoothDeltaTime;
            //Modify the player's world position to move along OUR local x axis.
            var dir = transform.parent.TransformDirection(new Vector3(1, 0, 0));
            dir *= speed;
            var pos = Player.transform.position;
            pos += dir;// * PlayerLerpScalar;
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
        //if less time has passed than the time until unengaged
        if (UnegagedTimeElapsed <= UnengagedTimer)
        {
            //progress the time
            UnegagedTimeElapsed += Time.deltaTime;
            //Escape
            return;
        }
        var localPos = transform.localPosition;

        //If I want the object to slide back
        if (!LocksIntoPlace)
        {
            //If I haven't pulled all of the way, I want to slip backwards (This if check prevents the thing from sliding back if it's reached the full pull)
            if (LerpPos <1)
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
            SendTabUpdatedMessage();
        }
        transform.localPosition = localPos;
    }
    //this function sends a message to all of the objects in the fold patter to rotate.
    void SendTabUpdatedMessage()
    {

        LerpPos = Mathf.Clamp01(1 - (PulledOutDistance - transform.localPosition.x) / PullingDistance);

        if (LerpData.value != LerpPos)
        {         
            LerpData.value = ac.Evaluate(LerpPos);
            EventTarget.gameObject.DispatchEvent(Events.TabUpdatedEvent, LerpData);
        }
    }
    //On trigger enter happens when the player walks into the interactive zone
    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            Player = other.gameObject;
//            PlayerAnim = GameObject.Find("PlayerAnim").GetComponent<Animator>();


            NearPlayer = true;

            if (InteractableHightlight)
            {
                InteractableHightlight.transform.position = transform.position + new Vector3(0, 1, 0);
                InteractableHightlight.SetActive(true);
            }
        }
    }
    //On trigger exit happens when the player walks away from the interactible zone
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
    //OnLockBody triggeres when the player holds down the interact button
    void OnLockBody()
    {
        print(PlayerAnim);
        if(PlayerAnim != null)
        {
            PlayerAnim.SetBool("PullLeft", true);

        }

        Engaged = true;
        //lock all player positions that are supposed to be locked
        var body = Player.GetComponent<Rigidbody>();
        body.constraints = RigidbodyConstraints.FreezeAll;
        PlayerYOffset = Player.transform.position.y - transform.parent.position.y;
        
    }
    //OnUnlockBody is run when the player lets go of the interact button. Its job is to remove all of the constraints given to it on lock
    void OnUnlockBody()
    {
        if (PlayerAnim != null)
        {
            PlayerAnim.SetBool("PullLeft", false);

        }

        UnengagedTimer = 1;
        Engaged = false;
        var body = Player.GetComponent<Rigidbody>();
        body.constraints = RigidbodyConstraints.FreezeRotation;
    }
}