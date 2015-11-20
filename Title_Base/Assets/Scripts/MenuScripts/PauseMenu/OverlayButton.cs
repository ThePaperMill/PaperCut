using UnityEngine;
using System.Collections;

public class OverlayButton : MenuButton
{
    public GameObject Overlay = null;
    GameObject Cam = null;
    Camera CamScript = null;

    void Start()
    {
        Cam = GameObject.FindGameObjectWithTag("MainCamera");
        Camera temp = Cam.GetComponent<Camera>();
        int startingmask = temp.cullingMask;
    }

  public override void Activate()
  {
        var test = GameObject.Instantiate<GameObject>(Overlay);

        test.transform.position = new Vector3();
  }

  // Update is called once per frame
  void Update()
  {

  }
}
