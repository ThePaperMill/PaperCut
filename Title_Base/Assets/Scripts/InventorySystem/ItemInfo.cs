using UnityEngine;
using System.Collections;
using System;

/****************************************************************************/
/*!
  \brief
 *  The Basic structure representing each item in our inventory
*/
/****************************************************************************/
[Serializable]
public class ItemInfo 
{
  public bool UsePrefabInfo = true;

  public string ItemDescription   = "Insert Witty Text Here";
  public string ItemName          = "Default";
  public MeshRenderer MRenderer   = null;
  public Material DisplayMaterial = null;
  public GameObject ItemPrefab    = null;
  public MeshFilter DisplayMesh         = null;

  public ItemInfo()
  {

  }

  public void InitializeItem()
  {
    if (UsePrefabInfo && ItemPrefab != null)
    {
      ItemName = ItemPrefab.name;
      MRenderer = ItemPrefab.GetComponent<MeshRenderer>();

      DisplayMesh = ItemPrefab.GetComponent<MeshFilter>();

      if (MRenderer)
        DisplayMaterial = MRenderer.sharedMaterial;
    }
  }
}
