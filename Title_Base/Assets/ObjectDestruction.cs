using UnityEngine;
using System.Collections;

public class ObjectDestruction : MonoBehaviour
{
    private enum GameEvent { GameStart, LabDestroyed, Part1PickedUp, Part2PickedUp, FinaleReady, GameEnd};

    [SerializeField]
    private GameEvent CreationEvent = GameEvent.GameStart;
    [SerializeField]
    private GameEvent DestructionEvent = GameEvent.GameEnd;

    // Use this for initialization
    void Start ()
    {
        /*
                    CREATION EVENTS
        */

        if (CreationEvent == GameEvent.GameStart || CreationEvent == GameEvent.GameEnd)
        {
            // leave this blank. This is intended to skip over other elseif Creation statements
        }
        else if (CreationEvent == GameEvent.LabDestroyed && GameInfo.GetSingleton.LabDestroyed == false)
        {
            Destroy(gameObject);
        }
        else if (CreationEvent == GameEvent.Part1PickedUp && GameInfo.GetSingleton.Part1PickedUp == false)
        {
            Destroy(gameObject);
        }
        else if (CreationEvent == GameEvent.Part2PickedUp && GameInfo.GetSingleton.Part2PickedUp == false)
        {
            Destroy(gameObject);
        }
        else if (CreationEvent == GameEvent.FinaleReady && GameInfo.GetSingleton.FinaleReady == false)
        {
            Destroy(gameObject);
        }

        /*
                    DESTRUCTION EVENTS
        */

        if (DestructionEvent == GameEvent.GameStart || DestructionEvent == GameEvent.GameEnd)
        {
            // leave this blank. This is intended to skip over other elseif Destruction statements
        }
        else if (DestructionEvent == GameEvent.LabDestroyed && GameInfo.GetSingleton.LabDestroyed == true)
        {
            Destroy(gameObject);
        }
        else if (DestructionEvent == GameEvent.Part1PickedUp && GameInfo.GetSingleton.Part1PickedUp == true)
        {
            Destroy(gameObject);
        }
        else if (DestructionEvent == GameEvent.Part2PickedUp && GameInfo.GetSingleton.Part2PickedUp == true)
        {
            Destroy(gameObject);
        }
        else if (DestructionEvent == GameEvent.FinaleReady && GameInfo.GetSingleton.FinaleReady == true)
        {
            Destroy(gameObject);
        }

        //if (LabDestruction == false && GameInfo.GetSingleton.LabDestroyed == true)
        //{
        //    Destroy(gameObject);
        //}
    }
}
