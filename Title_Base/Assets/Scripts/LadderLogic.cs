using UnityEngine;
using System.Collections;

public class LadderLogic : MonoBehaviour
{
    bool Active;
    bool MoveForward;
    bool MoveBack;
    bool MoveLeft;
    bool MoveRight;
    bool InteractPressed;
    bool InteractReleased;

    GameObject Player;

    // Use this for initialization
    void Start ()
    {
	    
	}

    // Update is called once per frame
    void Update()
    {
        if (Active == false)
        {
            return;
        }

        UpdateInput();

        print("Working");

        if (MoveForward)
        {
            Player.transform.localPosition += this.transform.up * Time.deltaTime * 2;
        }
        if (MoveBack)
        {
            Player.transform.localPosition -= this.transform.up * Time.deltaTime * 2;
        }
        if (MoveRight)
        {
            Player.transform.localPosition -= this.transform.forward * Time.deltaTime * 2;
        }
        if (MoveLeft)
        {
            Player.transform.localPosition += this.transform.forward * Time.deltaTime * 2;
        }

        if(InteractPressed)
        {
            Player.GetComponent<Rigidbody>().AddForce(-this.transform.right * 25);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        CustomDynamicController controller = collision.gameObject.GetComponent<CustomDynamicController>();

        if (controller != null)
        {
            print("SLOPPY");

            Player = collision.gameObject;
            controller.Active = false;
            Player.GetComponent<Rigidbody>().useGravity = false;
            Active = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        CustomDynamicController controller = collision.gameObject.GetComponent<CustomDynamicController>();

        if (controller != null)
        {
            print("Ended");

            controller.Active = true;
            Player.GetComponent<Rigidbody>().useGravity = true;
            Active = false;
        }
    }

    void UpdateInput()
    {
        MoveForward = InputManager.GetSingleton.IsButtonDown(XINPUT_BUTTONS.BUTTON_DPAD_UP) || InputManager.GetSingleton.IsKeyDown(KeyCode.UpArrow) || InputManager.GetSingleton.IsKeyDown(KeyCode.W);
        MoveBack = InputManager.GetSingleton.IsButtonDown(XINPUT_BUTTONS.BUTTON_DPAD_DOWN) || InputManager.GetSingleton.IsKeyDown(KeyCode.DownArrow) || InputManager.GetSingleton.IsKeyDown(KeyCode.S);
        MoveLeft = InputManager.GetSingleton.IsButtonDown(XINPUT_BUTTONS.BUTTON_DPAD_LEFT) || InputManager.GetSingleton.IsKeyDown(KeyCode.LeftArrow) || InputManager.GetSingleton.IsKeyDown(KeyCode.A);
        MoveRight = InputManager.GetSingleton.IsButtonDown(XINPUT_BUTTONS.BUTTON_DPAD_RIGHT) || InputManager.GetSingleton.IsKeyDown(KeyCode.RightArrow) || InputManager.GetSingleton.IsKeyDown(KeyCode.D);
        InteractPressed = InputManager.GetSingleton.IsButtonTriggered(XINPUT_BUTTONS.BUTTON_A) || InputManager.GetSingleton.IsKeyTriggered(KeyCode.Space);
        InteractReleased = InputManager.GetSingleton.IsButtonReleased(XINPUT_BUTTONS.BUTTON_A) || InputManager.GetSingleton.IsKeyReleased(KeyCode.Space);
    }
}
