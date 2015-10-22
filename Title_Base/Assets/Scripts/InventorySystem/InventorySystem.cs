/****************************************************************************/
/*!
\file   Inventory.csSystem
\author Steven Gallwas
\brief  
    This file contains the implementation of the Inventory System
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Scripts.ConversationSystem;


// Used to represent the state of the inventory
public enum InventoryState
{
  INVENTORY_VIEW, // general viewing of the inventory
  INVENTORY_GIVE, // when prompted to give an item
}

/****************************************************************************/
/*!
  \brief
    The inventroy system is a singleton
*/
/****************************************************************************/
public class InventorySystem : Singleton<InventorySystem> 
{
  // the vector of iteminfo structs
  List<ItemInfo> Inventory;

  // Vector of actual item objects
  List<GameObject> Inventory_Items;

  // the state of our inventory viewing or giving items
  InventoryState CurState = InventoryState.INVENTORY_VIEW;

  // Boolean to check if the inventory is open
  private bool InventoryOpen = false;

  // our current position in the inventory array 
  private int CurPosition = 0;

  // the current item we have selected
  GameObject CurItem = null;

  // the speed at which the selected item rotates
  float RotationSpeed = 10.0f;

  // boolean values for input
  bool MoveLeft  = false;
  bool MoveRight = false;
  bool Activate  = false;

  // this is where we want the hud to go by default
  Vector3 InventoryLand = new Vector3();

  // this represents the space between items when they are laid out in the world.
  Vector3 ItemWidth = new Vector3(1.5f, 0, 0);

  /****************************************************************************/
  /*!
    \brief
   *  The Basic structure representing each item in our inventory
  */
  /****************************************************************************/
  InventorySystem()
  {
    Inventory       = new List<ItemInfo>();
    Inventory_Items = new List<GameObject>();
    EventSystem.GlobalHandler.Connect(Events.RequestItem, OnRequestItem);
    EventSystem.GlobalHandler.Connect(Events.RecievedProperItem, OnRecievedProperItem);
  }

    void OnRequestItem(EventData data)
    {
        StringEvent eventData = data as StringEvent;
        print(eventData.Message);
        var info = new ItemInfo();
        info.ItemName = "DefaultCrate";
        EventSystem.GlobalHandler.DispatchEvent(Events.RecievedItem, new RecievedItemEvent(info));
    }

    void OnRecievedProperItem(EventData eventData)
    {
        var data = eventData as BoolEvent;
        
        if(data.IsTrue)
        {
            //remove item
        }
    }

  /****************************************************************************/
  /*!
    \brief
    *  The Basic structure representing each item in our inventory
  */
  /****************************************************************************/
  bool HasItem(ItemInfo item)
  {
    foreach(var i in Inventory)
    {
      if(i.ItemName == item.ItemName)
      {
        return true;
      }
    }

    return false;
  }

  /****************************************************************************/
  /*!
    \brief
     remove all items from the inventory
  */
  /****************************************************************************/
  public void ClearInventory()
  {
    Inventory.Clear();
  }

  /****************************************************************************/
  /*!
    \brief
      Sort 
  */
  /****************************************************************************/
  public void SortInventory()
  {
    for (int i = 0; i < Inventory.Count; ++i)
    {
      int j = i;
      var current = Inventory[i];

      while ((j > 0) && (string.Compare(Inventory[j - 1].ItemName, current.ItemName, true)) < 0)
      {
        Inventory[j] = Inventory[j - 1];
        j--;
      }

      Inventory[j] = current;
    }
  }

  /****************************************************************************/
  /*!
    \brief
   *  The Basic structure representing each item in our inventory
  */
  /****************************************************************************/
  private void RemoveItem(ItemInfo Item)
  {
    foreach (var i in Inventory)
    {
      if (i == Item)
      {
        Inventory.Remove(i);        
      }
    }
  }

  private void RemoveFirstByName(string name)
  {
    foreach (var i in Inventory)
    {
      if (i.ItemName == name)
      {
        Inventory.Remove(i);
      }
    }
  }

  /****************************************************************************/
  /*!
    \brief
      Adds an item to the inventory, opens the inventory, and moves to the item.
  */
  /****************************************************************************/
  public void AddItem(ItemInfo Item)
  {
    Inventory.Add(Item);
  }

  /****************************************************************************/
  /*!
    \brief
     The Basic structure representing each item in our inventory
  */
  /****************************************************************************/
  void MoveToItem(ItemInfo item)
  {
    for(int i = 0; i < Inventory.Count; ++i)
    {
      if (Inventory[i].ItemName == item.ItemName)
      {
        CurPosition = i;
        return;
      }
    }

    return;
  }

  /****************************************************************************/
  /*!
    \brief
   *  The Basic structure representing each item in our inventory
  */
  /****************************************************************************/
  bool isInventoryOpen()
  {
    return InventoryOpen;
  }

  /****************************************************************************/
  /*!
    \brief
   *  The Basic structure representing each item in our inventory
  */
  /****************************************************************************/
  public void OpenInventory(InventoryState state)
  {
    // choose which state to open the menu in
    CurState = state;

    // set inventory to open
    InventoryOpen = true;

    // create each of the objects in the inventory.
    for (int i = 0; i < Inventory.Count; ++i)
    {
      CreateItem(InventoryLand + i * ItemWidth, Inventory[i]);
    }

    // send a activate selector event, we don't use message data, so null is okay
    EventSystem.GlobalHandler.DispatchEvent(Events.ActivateSelector);

    // set our current position at the start of the items
    CurPosition = 0;
  }

  /****************************************************************************/
  /*!
    \brief
   *  The Basic structure representing each item in our inventory
  */
  /****************************************************************************/
  public void CloseInventory()
  {
    if(InventoryOpen == false)
    {
      return;
    }

    // destroy each game object.
    foreach (var i in Inventory_Items)
    {
      Destroy(i);
    }

    Inventory_Items.Clear();
   
    CurPosition = 0;
    CurItem = null;
    InventoryOpen = false;
    CurState = InventoryState.INVENTORY_VIEW;
  }

  /****************************************************************************/
  /*!
    \brief
      Initialize the class
  */
  /****************************************************************************/
  void Start() 
  {
	
  }

  /****************************************************************************/
  /*!
        \brief
       Creates a single item.
  */
  /****************************************************************************/
  void CreateItem(Vector3 pos, ItemInfo item)
  {
    GameObject Temp = new GameObject("");

    Temp.AddComponent<MeshRenderer>();
    Temp.AddComponent<MeshFilter>();
    Temp.AddComponent<ItemLogic>();

    MeshFilter tempMesh = Temp.GetComponent<MeshFilter>();
    tempMesh.sharedMesh = item.DisplayMesh.sharedMesh;

    Temp.transform.position = pos;
  }


  /****************************************************************************/
  /*!
    \brief
      
  */
  /****************************************************************************/
  void Update () 
  {
    // only check input when the inventory is open.
	  if(InventoryOpen)
    {
      UpdateInput();

      UpdateCurrentItem();

      // rotate the current item here.
      if(CurItem)
      {
         CurItem.transform.Rotate(Vector3.up, RotationSpeed * Time.deltaTime);
      }

    }
  }

  /****************************************************************************/
  /*!
    \brief
      Updates the current input for this frame.

  */
  /****************************************************************************/
  void UpdateInput()
  {
    MoveLeft  = InputManager.GetSingleton.IsButtonTriggered(XINPUT_BUTTONS.BUTTON_DPAD_LEFT)  || InputManager.GetSingleton.IsKeyTriggered(KeyCode.LeftArrow);
    MoveRight = InputManager.GetSingleton.IsButtonTriggered(XINPUT_BUTTONS.BUTTON_DPAD_RIGHT) || InputManager.GetSingleton.IsKeyTriggered(KeyCode.RightArrow);
    Activate  = InputManager.GetSingleton.IsButtonTriggered(XINPUT_BUTTONS.BUTTON_A)          || InputManager.GetSingleton.IsKeyTriggered(KeyCode.Space);
  }

  /****************************************************************************/
  /*!
    \brief
      Updates the current input for this frame.
  */
  /****************************************************************************/
  void UpdateCurrentItem()
  {
    // move the items left
    if (MoveLeft)
    {
      --CurPosition;

      if (CurPosition < 0)
      {
        CurPosition = Inventory.Count - 1;
      }

      CurItem = Inventory_Items[CurPosition];

      MoveItemEvent MIE = new MoveItemEvent(-ItemWidth);

      EventSystem.GlobalHandler.DispatchEvent(Events.MoveItem, MIE);
    }

    // move the items right
    else if (MoveRight)
    {
      ++CurPosition;

      if (CurPosition > Inventory.Count)
      {
        CurPosition = 0;
      }

      CurItem = Inventory_Items[CurPosition];

      MoveItemEvent MIE = new MoveItemEvent(ItemWidth);

      EventSystem.GlobalHandler.DispatchEvent(Events.MoveItem, MIE);
    }

    else if(Activate)
    {
      ActivateButton();
    }
  }

  /****************************************************************************/
  /*!
    \brief
      depending on the inventory state, dispatch events here
  */
  /****************************************************************************/
  void ActivateButton()
  {
      // if the current state is to give an item, then we want to dispatch an event with the current item
      if (CurState == InventoryState.INVENTORY_GIVE)
      {
        ItemInfo temp = Inventory[CurPosition];

        RecievedItemEvent give = new RecievedItemEvent(temp);

        EventSystem.GlobalHandler.DispatchEvent(Events.RecievedItem, give);
      }
  }
}

/************************************************************************************/
                               // Custom Events //
/************************************************************************************/
/****************************************************************************/
/*!
  \brief
    This is the event that gets sent when we are recieving an item.

*/
/****************************************************************************/
public class RecievedItemEvent : EventData
{
    public ItemInfo Info;
    public RecievedItemEvent(ItemInfo info)
    {
        Info = info;
    }
}

/****************************************************************************/
/*!
  \brief
    This is the event that gets sent when we want to move the selector
*/
/****************************************************************************/
public class MoveItemEvent : EventData
{
    public Vector3 MoveAmount;

    public MoveItemEvent(Vector3 pos)
    {
        MoveAmount = pos;
    }
}