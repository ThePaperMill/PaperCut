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
  public ItemInfo()
  {
     if(UsePrefabInfo && ItemPrefab != null)
     {
         ItemName = ItemPrefab.name;
         DisplayMesh = ItemPrefab.GetComponent<MeshFilter>();
        
         MeshRenderer temp = ItemPrefab.GetComponent<MeshRenderer>();

            if (temp)
                DisplayMaterial = temp.sharedMaterial;
     }
  }

  public bool UsePrefabInfo = true;

  public string ItemDescription   = "Insert Witty Text Here";
  public string ItemName          = "Default";
  public MeshFilter DisplayMesh   = null;
  public Material DisplayMaterial = null;
  public GameObject ItemPrefab    = null;
}
