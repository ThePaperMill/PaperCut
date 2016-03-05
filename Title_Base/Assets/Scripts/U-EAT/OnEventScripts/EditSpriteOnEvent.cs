using UnityEngine;
using System.Collections;
using ActionSystem;

public class EditSpriteOnEvent : OnEvent
{
    public Sprite TargetSprite = null;
    //public float Duration = 1;
    //public Curve EasingCurve = Ease.Linear;

    SpriteRenderer Renderer;
	// Use this for initialization
	void Start ()
    {
        Renderer = GetComponent<SpriteRenderer>();
    }
	
    public override void OnEventFunc(EventData data)
    {
        
        Renderer.sprite = TargetSprite;
    }
	// Update is called once per frame
	void Update ()
    {
	
	}


}

