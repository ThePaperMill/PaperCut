using ActionSystem;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MoveLeftEveryFrame : MonoBehaviour {
    //public float speed = 1f;
    //public Vector3 movvector = new Vector3(-1, 0, 0);
    public float AnimationDelay = 10.0f;
    bool isAnimating = false;

    Vector3 InitialPos = Vector3.zero;
    public AnimationCurve Curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public Vector3 FinalPosOffset = new Vector3(-19, 0, 0);//.zero;
    Vector3 finalPos = new Vector3(0, 0, 0);//.zero;
    public float EaseTime = 5.0f;
    private ActionGroup grp = new ActionGroup();

    public bool RandomizedStart = true;


    // Use this for initialization
    void Start () {
        InitialPos = this.gameObject.transform.localPosition;
        finalPos = transform.position + FinalPosOffset;

        if(RandomizedStart)
        {
            //I need a random value between -1 and 1, to add to the finalpos offset
            finalPos += new Vector3(Random.Range(-4, 0.5f), 0, 0);
            //I need a random value between 0 and 2 to add to the speed
            EaseTime += Random.Range(0, 2);
        }


        //print("remove me before submit");
        //AnimationDelay -= 8;
        //ActivateAnimation();
        
    }
	
	// Update is called once per frame
	void Update () {
        //transform.localPosition += movvector * speed*Time.deltaTime;

        //I need a check that tells 
        AnimationDelay -= Time.deltaTime;
        if (AnimationDelay <= 0 && isAnimating == false)
        {
            isAnimating = true;
            ActivateAnimation();
        }

        grp.Update(Time.smoothDeltaTime);

        //I'm going to get them all to have a delay
        //afte the delay is up, they begin moving to the left
    }
    void ActivateAnimation()
    {
        var seq = Action.Sequence(grp);
        Action.Property(seq, this.gameObject.transform.GetProperty(o => o.localPosition), finalPos, EaseTime, Curve);
    }
}
