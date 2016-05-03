/****************************************************************************/
/*!
\file  WardrobeSystem.cs
\author Steven Gallwas 
\brief  
 
  
    © 2016 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class WardrobeSystem : MonoBehaviour
{
    public List<Material> Outfits = new List<Material>();

    List<GameObject> OutfitObjects = new List<GameObject>();
    Stack<GameObject> ObjectsToDelete = new Stack<GameObject>();

    GameObject HudCamera = null;

    bool active = false;

	// Use this for initialization
	void Start ()
    {
        HudCamera = GameObject.FindGameObjectWithTag("HudCamera");
        gameObject.Connect(Events.InteractedWith, OnInteractedWith);
    }
	
    void Activate()
    {
        if(HudCamera == null)
        HudCamera = GameObject.FindGameObjectWithTag("HudCamera");

        active = true;
        Vector3 offset = new Vector3();
        GamestateManager.GetSingleton.CurState = GAME_STATE.GS_MENU;

        foreach (var mat in Outfits)
        {
            GameObject temp = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("BasePlayer"));
            MeshRenderer tempRenderer = temp.GetComponent<MeshRenderer>();
            tempRenderer.material = mat;
            temp.layer = LayerMask.NameToLayer("UI");

            if (HudCamera)
            {
                temp.transform.position = HudCamera.transform.position + offset + HudCamera.transform.forward * 2;
                offset += new Vector3(1, 0, 0);
                temp.transform.parent = HudCamera.transform;
            }

            OutfitObjects.Add(temp);
        }
    }

    void OnInteractedWith(EventData data)
    {
        if(active == false)
            Activate();
    }

    void Deactivate()
    {
        active = false;
        GamestateManager.GetSingleton.CurState = GAME_STATE.GS_GAME;

        for(int i = 0; i < OutfitObjects.Count; ++i)
        {
            ObjectsToDelete.Push(OutfitObjects[i]);
        }

        while(ObjectsToDelete.Count > 0)
        {
            var temp = ObjectsToDelete.Pop();
            GameObject.Destroy(temp);
        }

        OutfitObjects.Clear();
    }

	// Update is called once per frame
	void Update ()
    {
	    if(active)
        {
            if(InputManager.GetSingleton.IsButtonTriggered(XINPUT_BUTTONS.BUTTON_START) || InputManager.GetSingleton.IsButtonTriggered(XINPUT_BUTTONS.BUTTON_B) || InputManager.GetSingleton.IsKeyTriggered(KeyCode.Escape))
            {
                Deactivate();
                return;
            }
        }
	}
}
