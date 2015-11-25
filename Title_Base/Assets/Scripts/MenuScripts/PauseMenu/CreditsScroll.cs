using UnityEngine;
using System.Collections;

public class CreditsScroll : MonoBehaviour 
{
  public float ScrollSpeed = 25.0f;

  float Timer = 0.0f;

  public float Delay = 30.0f;

    bool escape = false;

	// Use this for initialization
	void Start () 
  {
	
	}
	
	// Update is called once per frame
	void Update () 
  {
        UpdateInput();

    if (Timer > Delay || escape)
      Application.LoadLevel("MainMenu");

    Timer += Time.deltaTime;

      transform.position += Vector3.up * ScrollSpeed * Time.deltaTime;
	}

    void UpdateInput()
    {
        escape = InputManager.GetSingleton.IsButtonTriggered(XINPUT_BUTTONS.BUTTON_START) || InputManager.GetSingleton.IsKeyTriggered(KeyCode.Escape);
    }
}
