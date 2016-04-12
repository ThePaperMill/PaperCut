/****************************************************************************/
/*!
\file  NonUniformParentOnEvent.cs
\author Joshus Biggs 
\brief  
 
  
    © 2016 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;
using ActionSystem;
public class NonUniformParentOnEvent : OnEvent
{
    public GameObject Parent;
    public Vector3 Offset = new Vector3();
    public bool PosX = true;
    public bool PosY = true;
    public bool PosZ = true;
	void Start()
    {
	}

    public override void OnEventFunc(EventData data)
    {
        if(!Parent)
        {
            return;
        }
        Vector3 parentPos = Parent.transform.position + Offset;
        if (!PosX)
        {
            parentPos.x = transform.position.x;
        }
        if (!PosY)
        {
            parentPos.y = transform.position.y;
        }
        if (!PosZ)
        {
            parentPos.z = transform.position.z;
        }
        transform.position = parentPos;
    }

}
