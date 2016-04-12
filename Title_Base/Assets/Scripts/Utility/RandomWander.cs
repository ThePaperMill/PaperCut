using UnityEngine;
using System.Collections;
using ActionSystem;


public class RandomWander : MonoBehaviour
{
    public float FreeAreaDistance = 6.0f;
    public float MoveSpeed = 3.0f;

    Vector3 StartPosition = new Vector3();

    float WanderTimer = 0.0f;
    float WanderTime = 3.0f;
    float WaitTimer = 0.0f;
    float WaitTime = 2.0f;

    public Vector3 WanderTarget = new Vector3();

    public bool Active = true;

    bool wait = false;

    public LayerMask GroundMask = 0;

    public GameObject ObjectImage = null;

    float BounceAmount = 0.015f;

    public float HeightOffset { get; set; }

    ActionSequence seq = new ActionSequence();

    // Use this for initialization
    void Start()
    {
        seq.LoopingSequence = true;

        Action.Property(seq, ObjectImage.transform.GetProperty(x => x.transform.localPosition), new Vector3(0,0.15f,0.0f), 0.15f, Ease.QuadOut);
        Action.Property(seq, ObjectImage.transform.GetProperty(x => x.transform.localPosition), Vector3.zero, 0.15f, Ease.QuadOut);

        StartPosition = transform.position;
        GetRandomDirection();
    }

    /****************************************************************************/
        /*!
            \brief

        */
        /****************************************************************************/
        void LookatTarget(Vector3 Target)
    {
        var targetPosition = Target;
        targetPosition.y = transform.position.y;
        transform.LookAt(targetPosition);

        Quaternion newRotation = new Quaternion();

        newRotation = Quaternion.LookRotation(targetPosition);

        newRotation.x = 0.0f;
        newRotation.z = 0.0f;

        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime);
    }


    /****************************************************************************/
    /*!
        \brief

    */
    /****************************************************************************/
    bool checkMoveDirection(Vector3 MoveDIR, float moveSpeed = 5.0f)
    {
        RaycastHit test = new RaycastHit();
        bool check = Physics.Raycast(transform.position, MoveDIR, out test, moveSpeed * Time.deltaTime);

        if (check)
        {
          return false;
        }

        // return true if there is no barrier in our way
        return true;
    }

    void GetRandomDirection()
    {
        WanderTimer = 0.0f;
        WanderTime = 3.0f; //Random.Range(1.0f, 2.5f);

        float RandX = Random.Range(StartPosition.x - FreeAreaDistance, StartPosition.x + FreeAreaDistance);
        float RandZ = Random.Range(StartPosition.z - FreeAreaDistance, StartPosition.z + FreeAreaDistance);

        WanderTarget = new Vector3(RandX, 0.0f, RandZ);

        WanderTimer = 0.0f;

        WanderTime = Random.Range(1.0f, 4.0f);

        wait = false;
    }

    /****************************************************************************/
    /*!
        \brief

    */
    /****************************************************************************/
    // Update is called once per frame
    void Update()
    {
        if(wait == true)
        {
            WaitTimer += Time.deltaTime;
            seq.Pause();
            ObjectImage.transform.localPosition = Vector3.zero;

            if (WaitTimer > WaitTime)
            {
                seq.Resume();
                WanderTimer = 0.0f;
                WaitTimer = 0.0f;
                WaitTime = Random.Range(1.0f, 3.0f);
                GetRandomDirection();
            }
        }


        WanderTimer += Time.deltaTime;

        if (WanderTimer > WanderTime)
        {
            wait = true;
        }

        if (wait == false)
        {
            LookatTarget(WanderTarget);

            Vector3 MoveDIR = (WanderTarget - transform.position);
            
            MoveDIR = MoveDIR.normalized * Time.deltaTime;
            
            // project out up direction
            MoveDIR = MoveDIR - Vector3.up * Vector3.Dot(MoveDIR, Vector3.up);

            bool test = checkMoveDirection(MoveDIR.normalized, 1.0f);

            if (test)
            {
                transform.position += MoveDIR;
            }

            if (Vector3.Distance(transform.position, WanderTarget) <= 0.5f)
            {
                wait = true;
            }

            seq.Update(Time.deltaTime);
        }
    }
}
