﻿/****************************************************************************/
/*!
\file   ItemInfo.cs
\author Steven Gallwas
\brief  
    Holds the information about the item.
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/

using UnityEngine;
using System.Collections;
using System;

public enum ITEM_STATUS
{
    IS_REAL,
    IS_CARDBOARD,
}

/****************************************************************************/
/*!
  \brief
 *  The Basic structure representing each item in our inventory
*/
/****************************************************************************/
[Serializable]
public class ItemInfo 
{
    public bool Transformable = false;
    public ITEM_STATUS CurStatus = ITEM_STATUS.IS_CARDBOARD;

    [TextArea]
    public string CardboardItemDescription = "Insert Witty Text Here";

    [TextArea]
    public string RealItemDescription      = "Insert Witty Text Here";

    [HideInInspector]
    public string ItemName = "Default";

    [HideInInspector]
    public GameObject ItemPrefab = null;

    [HideInInspector]
    public string ItemDescription = "";

    public GameObject CardboardItemPrefab = null;
    public GameObject RealItemPrefab      = null;

  public ItemInfo()
  {

  }

  public void InitializeItem()
  {
    if(CurStatus == ITEM_STATUS.IS_CARDBOARD)
    {
       ItemPrefab      = CardboardItemPrefab;
       ItemDescription = CardboardItemDescription;
       ItemName        = CardboardItemPrefab.name;
    }
    else
    {
       ItemPrefab      = RealItemPrefab;
       ItemDescription = RealItemDescription;
       ItemName        = RealItemPrefab.name;
    }   
  }
}
