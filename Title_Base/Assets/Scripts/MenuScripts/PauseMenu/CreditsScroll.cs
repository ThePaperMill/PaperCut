using UnityEngine;
using System.Collections;

public class CreditsScroll : MonoBehaviour 
{
  public float ScrollSpeed = 25.0f;

  float Timer = 0.0f;

  public float Delay = 30.0f;

	// Use this for initialization
	void Start () 
  {
	
	}
	
	// Update is called once per frame
	void Update () 
  {
    if (Timer > Delay)
      Application.LoadLevel("MainMenu");

    Timer += Time.deltaTime;

    print(Timer);
      transform.position += Vector3.up * ScrollSpeed * Time.deltaTime;
	}
}
