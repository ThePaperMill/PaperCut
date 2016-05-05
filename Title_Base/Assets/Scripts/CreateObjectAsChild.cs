/****************************************************************************/
/*!
\file   CreateObjectAsChild.cs
\author Michael Van Zant
\brief  
 
  
    © 2016 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;

public class CreateObjectAsChild : MonoBehaviour
{
    [SerializeField] GameObject ItemToCreate;
    private GameObject CreatedObject;

    // Use this for initialization
    void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}

    void OnTriggerEnter(Collider ce_)
    {
        if (ce_.gameObject.tag == "Player")
        {
            CreatedObject = Instantiate(ItemToCreate, ce_.gameObject.transform.position, ce_.gameObject.transform.rotation) as GameObject;
            CreatedObject.transform.parent = ce_.gameObject.transform;
        }
    }
    void OnTriggerExit(Collider ce_)
    {
        if (ce_.gameObject.tag == "Player")
        {
            Destroy(CreatedObject);
        }
    }
}
