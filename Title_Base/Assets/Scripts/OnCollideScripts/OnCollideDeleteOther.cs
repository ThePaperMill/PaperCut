/****************************************************************************/
/*!
\file   OnCollideDeleteOther.cs
\author Ian Aemmer
\brief  
    Destroy objects on collide.
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/

using UnityEngine;
using System.Collections;

public class OnCollideDeleteOther : MonoBehaviour {

    public GameObject ObjectToDelete = null;
    public bool DeleteSelf = true;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    void OnTriggerEnter(Collider other)
    {
        print(other.gameObject.tag);
        if(other.gameObject.tag == "Player")
        {
            Destroy(ObjectToDelete);
            if (DeleteSelf == true)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
