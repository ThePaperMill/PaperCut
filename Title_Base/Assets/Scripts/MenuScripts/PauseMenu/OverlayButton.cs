/****************************************************************************/
/*!
\file   OverlayButton.cs
\author Steven Gallwas.cs
\brief  
    Triggers and overlay.
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/

using UnityEngine;
using System.Collections;

public class OverlayButton : MenuButton
{
    public GameObject Overlay = null;
    GameObject Cam = null;
    Camera CamScript = null;
    GameObject OverlayObject = null;
    int StartingMask = 0;

    public string CameraTag = "MainCamera";

    void Start()
    {
        Cam          = GameObject.FindGameObjectWithTag(CameraTag);
        CamScript    = Cam.GetComponent<Camera>();
        StartingMask = CamScript.cullingMask;
        EventSystem.GlobalHandler.Connect(Events.CancelOverlay, OnCancelOverlay);
    }

    void OnCancelOverlay(EventData dt)
    {
        if(OverlayObject)
        {
            Destroy(OverlayObject);
            CamScript.cullingMask = StartingMask;
        }
    }

    void OnDestroy()
    {
        EventSystem.GlobalHandler.Disconnect(Events.CancelOverlay, OnCancelOverlay);
    }

  public override void Activate()
  {
        OverlayObject = GameObject.Instantiate<GameObject>(Overlay);

        OverlayObject.transform.position = Cam.transform.position + new Vector3(0, 0, 3.5f);

        CamScript.cullingMask = (1 << LayerMask.NameToLayer("PauseOverlay"));

        EventSystem.GlobalHandler.DispatchEvent(Events.OverlayActive);
    }

  // Update is called once per frame
  void Update()
  {

  }
}
