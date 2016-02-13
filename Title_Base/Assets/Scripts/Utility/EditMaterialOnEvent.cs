/****************************************************************************/
/*!
\file   RotateTowards.cs
\author Steven Gallwas
\brief  
	makes things rotate to a given angle.
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/

using UnityEngine;
using System.Collections;

public class EditMaterialOnEvent : OnEvent 
{
    public Material TargetMaterial = null;

    MeshRenderer Renderer;

    public override void Awake()
    {
        base.Awake();
        Renderer = GetComponent<MeshRenderer>();
    }

    public override void OnEventFunc(EventData data)
    {
        if(TargetMaterial)
        {
            Renderer.material = TargetMaterial;
        }
    }
}
