using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Reactive : MonoBehaviour
{
    public bool Active = true;
    public bool MouseEvents = true;
    public bool InstantiationEvents = true;
    public bool UpdateEvents = false;
    public bool CollisionEvents = false;
    public bool GameEvents = false;

    void Awake()
    {
        if(!Active)
        {
            return;
        }
        if (!InstantiationEvents)
        {
            return;
        }
        gameObject.DispatchEvent(Events.Create);
    }

    void Start()
    {
        if (!Active)
        {
            return;
        }
        if (!InstantiationEvents)
        {
            return;
        }
        gameObject.DispatchEvent(Events.Initialize);
    }

    void OnMouseEnter()
    {
        if (!Active)
        {
            return;
        }
        if (!MouseEvents)
        {
            return;
        }
        gameObject.DispatchEvent(Events.MouseEnter);
    }

    void OnMouseExit()
    {
        if (!Active)
        {
            return;
        }
        if (!MouseEvents)
        {
            return;
        }
        gameObject.DispatchEvent(Events.MouseExit);
    }

    void OnMouseUp()
    {
        if (!Active)
        {
            return;
        }
        if (!MouseEvents)
        {
            return;
        }
        gameObject.DispatchEvent(Events.MouseUp);
    }

    void OnMouseDown()
    {
        if (!Active)
        {
            return;
        }
        if (!MouseEvents)
        {
            return;
        }
        gameObject.DispatchEvent(Events.MouseDown);
    }

    void OnMouseOver()
    {
        if (!Active)
        {
            return;
        }
        if (!MouseEvents)
        {
            return;
        }
        gameObject.DispatchEvent(Events.MouseOver);
    }

    void OnMouseDrag()
    {
        if (!Active)
        {
            return;
        }
        if (!MouseEvents)
        {
            return;
        }
        gameObject.DispatchEvent(Events.MouseDrag);
    }

    void Update()
    {
        if (!Active)
        {
            return;
        }
        if (UpdateEvents)
        {
            gameObject.DispatchEvent(Events.LogicUpdate);
        }
        
    }

    void LateUpdate()
    {
        if (!Active)
        {
            return;
        }
        if (!UpdateEvents)
        {
            return;
        }
        gameObject.DispatchEvent(Events.LateUpdate);
    }

    void OnDestroy()
    {
        if (!Active)
        {
            return;
        }
        if (!InstantiationEvents)
        {
            return;
        }
        gameObject.DispatchEvent(Events.Destroy);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (!Active)
        {
            return;
        }
        if (!CollisionEvents)
        {
            return;
        }

        gameObject.DispatchEvent(Events.OnCollisionEnter2D, new CollisionEvent2D(collision));
        
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (!Active)
        {
            return;
        }
        if (!CollisionEvents)
        {
            return;
        }
        gameObject.DispatchEvent(Events.OnCollisionExit2D, new CollisionEvent2D(collision));
    }

    void OnCollisionStay2D(Collision2D collision)
    {
        if (!Active)
        {
            return;
        }
        if (!CollisionEvents)
        {
            return;
        }
        gameObject.DispatchEvent(Events.OnCollisionStay2D, new CollisionEvent2D(collision));
        
    }

    //void OnTriggerEnter2D(Collision2D collision)
    //{
    //    if (!Active)
    //    {
    //        return;
    //    }
    //    if (!CollisionEvents)
    //    {
    //        return;
    //    }

    //    gameObject.DispatchEvent(Events.OnCollisionEnter2D, new CollisionEvent2D(collision));

    //}

    //void OnTriggerExit2D(Collision2D collision)
    //{
    //    if (!Active)
    //    {
    //        return;
    //    }
    //    if (!CollisionEvents)
    //    {
    //        return;
    //    }
    //    gameObject.DispatchEvent(Events.OnCollisionExit2D, new CollisionEvent2D(collision));
    //}

    //void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (!Active)
    //    {
    //        return;
    //    }
    //    if (!CollisionEvents)
    //    {
    //        return;
    //    }
    //    gameObject.DispatchEvent(Events.OnCollisionStay2D, new CollisionEvent2D(collision));

    //}

    void OnCollisionEnter(Collision collision)
    {
        if (!Active)
        {
            return;
        }
        if (!CollisionEvents)
        {
            return;
        }

        gameObject.DispatchEvent(Events.OnCollisionEnter, new CollisionEvent3D(collision));

    }

    void OnCollisionExit(Collision collision)
    {
        if (!Active)
        {
            return;
        }
        if (!CollisionEvents)
        {
            return;
        }
        gameObject.DispatchEvent(Events.OnCollisionExit, new CollisionEvent3D(collision));
    }

    void OnCollisionStay(Collision collision)
    {
        if (!Active)
        {
            return;
        }
        if (!CollisionEvents)
        {
            return;
        }
        gameObject.DispatchEvent(Events.OnCollisionStay, new CollisionEvent3D(collision));

    }

    void OnTriggerEnter(Collider collision)
    {
        if (!Active)
        {
            return;
        }
        if (!CollisionEvents)
        {
            return;
        }

        gameObject.DispatchEvent(Events.OnCollisionEnter, new CollisionEvent3D());

    }

    void OnTriggerExit(Collider collision)
    {
        if (!Active)
        {
            return;
        }
        if (!CollisionEvents)
        {
            return;
        }
        gameObject.DispatchEvent(Events.OnCollisionExit, new CollisionEvent3D());
    }

    void OnTriggerStay(Collider collision)
    {
        if (!Active)
        {
            return;
        }
        if (!CollisionEvents)
        {
            return;
        }
        gameObject.DispatchEvent(Events.OnCollisionStay, new CollisionEvent3D());

    }

}

