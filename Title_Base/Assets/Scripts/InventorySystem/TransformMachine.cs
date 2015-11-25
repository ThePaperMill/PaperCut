/****************************************************************************/
/*!
\file    TransformMachine.cs
\author Steven Gallwas
\brief  
       The machine that transforms objects.
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;
using ActionSystem;

public class TransformMachine : EventHandler
{
    public GameObject LightningBoltPrefab = null;
    public GameObject ParticleEffect = null;
   
    private ActionGroup grp = new ActionGroup();
    Vector3 ItemPosition = new Vector3();
    ItemInfo Item = null;

    GameObject OldObject = null;
    GameObject NewObject = null;
    GameObject LBolt     = null;
    GameObject Particle  = null;

    int transformations = 0;
    public AudioClip TransformSound = null;

    float Timer = 0.0f;

    bool Triggered = false;
	// Use this for initialization
    void Start () 
    {
        EventSystem.GlobalHandler.Connect(Events.TransformItem, OnTransformItem);

        Ray testRay = new Ray();

        testRay.origin = transform.position;
        testRay.direction = Vector3.down;
        RaycastHit RayResult = new RaycastHit();
        
        bool check = Physics.Raycast(testRay, out RayResult, 10);

        if (check && false)
        {
            print(RayResult.collider.gameObject.name);
            ItemPosition = transform.position - new Vector3(0, 0, RayResult.distance);
        }
        else
        { 
            ItemPosition = transform.position - new Vector3(0, 2.1f, 0);
        }
    }

    void OnTransformItem(EventData eventData)
    {
        // cast as a recieved item event
        var data = eventData as RecievedItemEvent;
        Item = data.Info;

        var test = ActionSystem.Action.Sequence(grp);

        Action.Call(test, CreateOldItem);
        Action.Delay(test, 1.5f);
        Action.Call(test, CreateLightningBolt);
        Action.Delay(test, 0.5f);
        Action.Call(test, CreateParticle);
        Action.Delay(test, 0.45f);
        Action.Call(test, CreateNewItem);
        Action.Delay(test, 0.45f);
        Action.Call(test, MachineFinished);
    }

    void MachineFinished()
    {
        EventSystem.GlobalHandler.DispatchEvent(Events.MachineFinisehd);
    }

    void CreateLightningBolt()
    {
        LBolt = (GameObject)Instantiate(LightningBoltPrefab,transform.position, Quaternion.identity);
        LBolt.AddComponent<Rigidbody>();
    }

    void CreateParticle()
    {
        AudioSource tempaudio = GetComponent<AudioSource>();

        if (tempaudio && TransformSound)
        {
            tempaudio.Stop();
            tempaudio.PlayOneShot(TransformSound);
        }


        GameObject.Destroy(OldObject);

        if (ParticleEffect)
        {
            Instantiate(ParticleEffect, ItemPosition, Quaternion.identity);
            Instantiate(ParticleEffect, ItemPosition + new Vector3(0,1.0f,0), Quaternion.identity);
            Instantiate(ParticleEffect, ItemPosition + new Vector3(0,-1.0f,0), Quaternion.identity);
        }
    }

    void CreateOldItem()
    {
        GameObject Temp = null;

        //if the item is cardboard, create the real version
        if (Item.CurStatus == ITEM_STATUS.IS_CARDBOARD)
        {
            Temp = GameObject.Instantiate(Item.CardboardItemPrefab);
        }
        // if the item is real, create the cardboard version.
        else
        {
            Temp = GameObject.Instantiate(Item.RealItemPrefab);
        }

        Temp.name = ("Item_" + Item.ItemName);

        Temp.transform.localPosition = ItemPosition;
        Temp.transform.position += new Vector3(0, 0, 0.5f);

        // disable all other components
        var comps = Temp.GetComponents<MonoBehaviour>();
        foreach (var c in comps)
        {
            c.enabled = false;
        }

        OldObject = Temp;
    }

    void CreateNewItem()
    {
        GameObject Temp = null;
        transformations++;

        //if the item is cardboard, create the real version
        if (Item.CurStatus == ITEM_STATUS.IS_CARDBOARD)
        {
            Temp = GameObject.Instantiate(Item.RealItemPrefab);
        }
        // if the item is real, create the cardboard version.
        else
        {
            Temp = GameObject.Instantiate(Item.CardboardItemPrefab);
        }

        Temp.name = ("Item_" + Item.ItemName);

        GameObject CollectableBase = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("CollectableBase"));

        CollectableObject CO = CollectableBase.GetComponent<CollectableObject>();
        CO.ItemToGive = Item;
        
        if(Item.CurStatus == ITEM_STATUS.IS_CARDBOARD)
        {
          CO.ItemToGive.CurStatus = ITEM_STATUS.IS_REAL;
        }
        else
        {
          CO.ItemToGive.CurStatus = ITEM_STATUS.IS_CARDBOARD;
        }

        CollectableBase.transform.position = Temp.transform.position;
        CollectableBase.transform.parent = Temp.transform;

        //disable all other components
        var comps = Temp.GetComponents<MonoBehaviour>();
        foreach (var c in comps)
        {
            c.enabled = false;
        }

        Temp.transform.localPosition = ItemPosition;
        Temp.transform.position += new Vector3(0, 0, 0.5f);

        NewObject = Temp;
    }

	// Update is called once per frame
  void Update () 
  {
    grp.Update(Time.deltaTime);

    if(transformations >= 2 && !Triggered)
    {
        Triggered = true;
        EventSystem.GlobalHandler.DispatchEvent(Events.EndTheGame);
        GamestateManager.GetSingleton.CurState = GAME_STATE.GS_CINEMATIC;
    }
    else if(Triggered)
    {
        Timer += Time.deltaTime;
        if(Timer > 3.5f)
        {
            Application.LoadLevel("ScrollingCredits");
        }
    }
  }
}