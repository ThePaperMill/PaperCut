/****************************************************************************/
/*!
\file   TeleportScript.cs
\author Ian Aemmer
\brief  
    Teleport Cheats.
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/

using UnityEngine;
using System.Collections;

public class TeleportScript : MonoBehaviour {

    public GameObject ObjectToTeleport = null;
    public GameObject Teleport1 = null;
    public GameObject Teleport2 = null;
    public GameObject Teleport3 = null;
    public GameObject Teleport4 = null;
    public GameObject Teleport5 = null;
    public GameObject Teleport6 = null;

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

        if (InputManager.GetSingleton.IsButtonDown(XINPUT_BUTTONS.BUTTON_LEFT_SHOULDER) && InputManager.GetSingleton.IsButtonDown(XINPUT_BUTTONS.BUTTON_Y))
        {
            if (ObjectToTeleport != null && Teleport1 != null)
            {
                //make the transformation of the object to teleport the position of the other thing
                Transform tTransform = ObjectToTeleport.GetComponent<Transform>();
                Transform t2 = Teleport1.GetComponent<Transform>();

                tTransform.position = t2.position;
            }
        }
        if (InputManager.GetSingleton.IsButtonDown(XINPUT_BUTTONS.BUTTON_LEFT_SHOULDER) && InputManager.GetSingleton.IsButtonDown(XINPUT_BUTTONS.BUTTON_X))
        {
            if (ObjectToTeleport != null && Teleport2 != null)
            {
                //make the transformation of the object to teleport the position of the other thing
                Transform tTransform = ObjectToTeleport.GetComponent<Transform>();
                Transform t2 = Teleport2.GetComponent<Transform>();

                tTransform.position = t2.position;
            }
        }
        if (InputManager.GetSingleton.IsButtonDown(XINPUT_BUTTONS.BUTTON_LEFT_SHOULDER) && InputManager.GetSingleton.IsButtonDown(XINPUT_BUTTONS.BUTTON_B))
        {
            if (ObjectToTeleport != null && Teleport3 != null)
            {
                //make the transformation of the object to teleport the position of the other thing
                Transform tTransform = ObjectToTeleport.GetComponent<Transform>();
                Transform t2 = Teleport3.GetComponent<Transform>();

                tTransform.position = t2.position;
            }
        }
        if (InputManager.GetSingleton.IsButtonDown(XINPUT_BUTTONS.BUTTON_LEFT_SHOULDER) && InputManager.GetSingleton.IsButtonDown(XINPUT_BUTTONS.BUTTON_A))
        {
            if (ObjectToTeleport != null && Teleport4 != null)
            {
                //make the transformation of the object to teleport the position of the other thing
                Transform tTransform = ObjectToTeleport.GetComponent<Transform>();
                Transform t2 = Teleport4.GetComponent<Transform>();

                tTransform.position = t2.position;
            }
        }
        if (InputManager.GetSingleton.IsButtonDown(XINPUT_BUTTONS.BUTTON_LEFT_SHOULDER) && InputManager.GetSingleton.IsButtonDown(XINPUT_BUTTONS.BUTTON_RIGHT_SHOULDER))
        {
            if (ObjectToTeleport != null && Teleport5 != null)
            {
                //make the transformation of the object to teleport the position of the other thing
                Transform tTransform = ObjectToTeleport.GetComponent<Transform>();
                Transform t2 = Teleport5.GetComponent<Transform>();

                tTransform.position = t2.position;
            }
        }
        if (InputManager.GetSingleton.IsButtonDown(XINPUT_BUTTONS.BUTTON_LEFT_SHOULDER) && InputManager.GetSingleton.IsButtonDown(XINPUT_BUTTONS.BUTTON_DPAD_DOWN))
        {
            if (ObjectToTeleport != null && Teleport6 != null)
            {
                //make the transformation of the object to teleport the position of the other thing
                Transform tTransform = ObjectToTeleport.GetComponent<Transform>();
                Transform t2 = Teleport6.GetComponent<Transform>();

                tTransform.position = t2.position;
            }
        }

    }
}
