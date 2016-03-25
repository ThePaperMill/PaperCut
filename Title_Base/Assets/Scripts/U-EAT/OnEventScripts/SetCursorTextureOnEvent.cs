using UnityEngine;
using System.Collections;
using ActionSystem;
using System.Collections.Generic;

public class SetCursorTextureOnEvent : OnEvent
{
    public Texture2D CursorTexture;
    public Vector2 TextureOffset = new Vector2();
    public CursorMode CursorMode = CursorMode.Auto;
    

    void Start()
    {

    }


    public override void OnEventFunc(EventData data)
    {
        if(!CursorTexture)
        {
            throw new System.Exception(this.GetType().Name + " on object " + gameObject.name + " has no cursor texture set.");
        }

        Cursor.SetCursor(CursorTexture, TextureOffset, CursorMode);
    }

    void OnDestroy()
    {

    }
}

