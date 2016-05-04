using UnityEngine;
using System.Collections;

public class LerpBackAndForth : MonoBehaviour {

    public float timelength = 5.0f;
    public AnimationCurve Curve = AnimationCurve.EaseInOut(0, 0, 1, 1);
    public Vector3 endPos = Vector3.zero;
    Vector3 startPos = Vector3.zero;// = transform.position;

    // Use this for initialization
    void Start () {
        startPos = transform.localPosition;
        //I need to have an ease curve and josh's animation system

        //endPos = transform.InverseTransformDirection(endPos);
        StartCoroutine("LerpUp");
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    IEnumerator LerpUp()
    {
        //transform.InverseTransformDirection(Vector3.forward);

        //float elapsedTime = 0f;

        for (float elapsedTime = 0f; elapsedTime < timelength; elapsedTime += Time.deltaTime)
        {
            transform.localPosition = Vector3.Lerp(startPos, endPos, Curve.Evaluate(elapsedTime / timelength));
            yield return null;
        }

       // yield return new WaitForSeconds(timelength);
        //print("yes");

        StartCoroutine("LerpDown");
    }
    IEnumerator LerpDown()
    {

        //float elapsedTime = 0f;

        for (float elapsedTime = 0f; elapsedTime < timelength; elapsedTime += Time.deltaTime)
        {
            transform.localPosition = Vector3.Lerp( endPos, startPos, Curve.Evaluate(elapsedTime / timelength));
            yield return null;
        }

        // yield return new WaitForSeconds(timelength);
        print("yes");

        StartCoroutine("LerpUp");
    }
}
