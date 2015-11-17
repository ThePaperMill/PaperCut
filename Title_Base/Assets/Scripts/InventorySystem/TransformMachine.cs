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
  private ActionGroup grp = new ActionGroup();
  Vector3 ItemPosition = new Vector3();

	// Use this for initialization
  void Start () 
  {
    EventSystem.GlobalHandler.Connect(Events.TransformItem, OnTransformItem);
    ItemPosition = transform.position - new Vector3(0,2.0f,0);
  }

  void OnTransformItem(EventData eventData)
  {
    // cast as a recieved item event
    var data = eventData as RecievedItemEvent;
    ItemInfo Item = data.Info;

        CreateItem(Item);

  }

     void CreateItem(ItemInfo Item)
    {
        //if the item is cardboard, create the real version
        if (Item.CurStatus == ITEM_STATUS.IS_CARDBOARD)
        {
            GameObject Temp = GameObject.Instantiate(Item.RealItemPrefab);

            Temp.name = ("Item_" + Item.ItemName);

            Temp.transform.localPosition = ItemPosition;
            Temp.transform.position += new Vector3(0, 0, 0.5f);

            // disable all other components
            var comps = Temp.GetComponents<MonoBehaviour>();
            foreach (var c in comps)
            {
                c.enabled = false;
            }
        }
        // if the item is real, create the cardboard version.
        else
        {
            GameObject Temp = GameObject.Instantiate(Item.RealItemPrefab);

            Temp.name = ("Item_" + Item.ItemName);

            Temp.transform.localPosition = ItemPosition;
            Temp.transform.position += new Vector3(0, 0, 0.5f);

            // disable all other components
            var comps = Temp.GetComponents<MonoBehaviour>();
            foreach (var c in comps)
            {
                c.enabled = false;
            }
        }
    }

	// Update is called once per frame
  void Update () 
  {
    grp.Update(Time.deltaTime);
	}
}