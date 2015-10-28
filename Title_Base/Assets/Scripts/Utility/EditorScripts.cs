/****************************************************************************/
/*!
\file   SimpleCharacterController.cs
\author Steven Gallwas
\brief  
    This file contains the implementation for some simple editor scrips.
 
  © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;

#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode] 
public class EditorScripts : MonoBehaviour
{
#if UNITY_EDITOR
    void Start()
    {

    }

    // Update is called once per frame
    void OnRenderObject()
    {
        if (Application.isPlaying)
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                if (Application.isPlaying == true)
                {
                    EditorApplication.isPlaying = false;
                    return;
                }
            }
        }
        else
        {
            if (Input.GetKey(KeyCode.Space))
            {
                print("Literally anything");
            }

        }
    }
#endif

}
