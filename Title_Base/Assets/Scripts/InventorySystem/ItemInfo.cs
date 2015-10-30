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
    Debug.Log("item created");
  }

  public void InitializeItem()
  {
    if (UsePrefabInfo && ItemPrefab != null)
    {
      Debug.Log("Using Prefab");

      ItemName = ItemPrefab.name;
      MRenderer = ItemPrefab.GetComponent<MeshRenderer>();

      DisplayMesh = ItemPrefab.GetComponent<MeshFilter>();

      Debug.Log(MRenderer.sharedMaterial.shader);

      if (MRenderer)
        DisplayMaterial = MRenderer.sharedMaterial;
    }
  }
}
