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
using UnityEngine.SceneManagement;
using ActionSystem;

public class TransformMachine : EventHandler
{
    public GameObject LightningBoltPrefab = null;
    public GameObject ParticleEffect = null;

    private ActionGroup grp = new ActionGroup();
    private ActionGroup grp2 = new ActionGroup();
    Vector3 ItemPosition = new Vector3();
    ItemInfo Item = null;

    GameObject OldObject = null;
    GameObject LBolt     = null;

    int transformations = 0;
    public AudioClip TransformSound = null;

    // Use this for initialization

    public GameObject Light = null;
    GameObject MCamera = null;

    public Vector3 CamPosition = new Vector3();
    public float CamDuration = 1.0f;

    Vector3 CamStart = new Vector3();
    //Quaternion CamRotation = new Quaternion();
    Light LightScript = null;

    float ExplosionRadius = 15.0f;
    float ExplosionForce = 200.0f;

    void Start () 
    {
        EventSystem.GlobalHandler.Connect(Events.TransformItem, OnTransformItem);

        // hardcoded as F*** **** **** *** * **** **** ***
        ItemPosition = transform.position - new Vector3(0, 2.1f, 0);

        MCamera = GameObject.FindGameObjectWithTag("MainCamera");

        if(Light)
        {
            LightScript = Light.GetComponent<Light>();
        }
    }

    void AdjustCamera()
    {
        var test = ActionSystem.Action.Group(grp2);

        if (LightScript)
        {
            Action.Property(test, LightScript.GetProperty(o => o.intensity), 0.0f, CamDuration, Ease.Linear);
        }

        if (MCamera)
        {
            CamStart = MCamera.transform.localPosition;
            Action.Property(test, MCamera.transform.GetProperty(o => o.localPosition), CamPosition, CamDuration, Ease.Linear);
        }

        GamestateManager.GetSingleton.CurState = GAME_STATE.GS_CINEMATIC;
    }

    void ReturnCamera()
    {
        var test = ActionSystem.Action.Group(grp2);

        if (LightScript)
        {
            Action.Property(test, LightScript.GetProperty(o => o.intensity), 1.0f, CamDuration, Ease.Linear);
        }

        if (MCamera)
        {
            Action.Property(test, MCamera.transform.GetProperty(o => o.localPosition), CamStart, CamDuration, Ease.Linear);
        }
    }

    void ResetCamera()
    {
        GamestateManager.GetSingleton.CurState = GAME_STATE.GS_GAME;
        GameInfo.GetSingleton.LabDestroyed = true;
        LevelTransitionManager.GetSingleton.ChangeLevel("ScientistLabDestroyed");
    }

    void CreatePointBlast()
    {
        //EventSystem.GlobalHandler.DispatchEvent(Events.CatchFire);

        Vector3 ExplosionPoint = ItemPosition;

        var colliders = Physics.OverlapSphere(ExplosionPoint, ExplosionRadius);

        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null && rb.isKinematic == false)
            {
                rb.AddExplosionForce(ExplosionForce, ExplosionPoint, ExplosionRadius, 3.0F);
            }
        }
    }

    void OnTransformItem(EventData eventData)
    {
        // cast as a recieved item event
        var data = eventData as RecievedItemEvent;
        Item = data.Info;

        var test = ActionSystem.Action.Sequence(grp);

        // create the old item
        Action.Call(test, CreateOldItem);
        Action.Delay(test, 1.5f);

        // move the camera to a different positon, then stay there for the duration of the cutscene
        Action.Call(test, AdjustCamera);
        Action.Delay(test, CamDuration);

        // create the lightning bolt object here
        Action.Call(test, CreateLightningBolt);
        Action.Delay(test, 0.5f);

        // create the particle effect here
        Action.Call(test, CreateParticle);
        if (Item.Explode)
        {
            Action.Call(test, CreatePointBlast);
        }
        Action.Delay(test, 0.45f);

        // create the new object
        Action.Call(test, CreateNewItem);
        Action.Delay(test, 0.45f);


        Action.Call(test, MachineFinished);
        Action.Call(test, ReturnCamera);
        Action.Delay(test, CamDuration);

        Action.Call(test, ResetCamera);
    }

    void MachineFinished()
    {
        EventSystem.GlobalHandler.DispatchEvent(Events.MachineFinished);
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
    }

	// Update is called once per frame
  void Update () 
  {
    grp.Update(Time.deltaTime);

    grp2.Update(Time.deltaTime);

  //  if(transformations >= 2 && !Triggered)
  //  {
  //      Triggered = true;
  //      EventSystem.GlobalHandler.DispatchEvent(Events.EndTheGame);
  //      GamestateManager.GetSingleton.CurState = GAME_STATE.GS_CINEMATIC;
  //  }
  //  else if(Triggered)
  //  {
  //      Timer += Time.deltaTime;
  //      if(Timer > 3.5f)
  //      {
  //          SceneManager.LoadScene("ScrollingCredits");
  //      }
  //  }
  }
}