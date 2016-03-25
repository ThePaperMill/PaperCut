using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CycleSpriteOnEvent : EditOnEvent
{
    public List<Sprite> SpriteList = new List<Sprite>(0);
    public List<Sprite>.Enumerator NextSprite;
    SpriteRenderer Renderer;
    bool Dispatch = false;
    
    // Use this for initialization
    public override void Awake ()
    {
        base.Awake();
        Renderer = GetComponent<SpriteRenderer>();
        SpriteList.Add(Renderer.sprite);
        NextSprite = SpriteList.GetEnumerator();
        NextSprite.MoveNext();
        this.DelayedDispatch = true;
    }

    public override void OnEventFunc(EventData data)
    {
        Renderer.sprite = NextSprite.Current;
        if(!NextSprite.MoveNext())
        {
            NextSprite = SpriteList.GetEnumerator();
            NextSprite.MoveNext();
        }

        if(DispatchOnFinish)
        {
            Dispatch = true;
        }
        else
        {
            DispatchEvent();
        }
        
    }

    void Update()
    {
        if(Dispatch)
        {
            this.DispatchEvent();
            Dispatch = false;
        }
    }
}
