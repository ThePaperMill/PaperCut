using UnityEngine;
using System.Collections;

public class RotateTowards : MonoBehaviour 
{
  Quaternion DesiredRotation = new Quaternion();

  public Vector3 EulerRotation = new Vector3();

  bool RotationActive = false;

  public float RotationAngle = 90.0f;

    public float DelayTime = 0.0f;
    float delaytimer = 0.0f;

	// Use this for initialization
  void Start () 
  {
	  DesiredRotation = Quaternion.Euler(EulerRotation);
  }
	
  public void Rotate()
  {
    RotationActive = true;
  }

	// Update is called once per frame
	void Update () 
  {
        delaytimer += Time.deltaTime;
        if(RotationActive == false && delaytimer > DelayTime)
        {
            Rotate();
        }


	   if(RotationActive == true)
     {
       transform.rotation = Quaternion.RotateTowards(transform.rotation, DesiredRotation, RotationAngle * Time.deltaTime);
     }

	}
}
