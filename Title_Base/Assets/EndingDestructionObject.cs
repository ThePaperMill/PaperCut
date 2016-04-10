using UnityEngine;
using System.Collections;

public class EndingDestructionObject : MonoBehaviour
{

    public bool DestroyOnReady = true;

    // Use this for initialization
    void Start()
    {
        if (DestroyOnReady == true && GameInfo.GetSingleton.FinaleReady == true)
        {
            Destroy(gameObject);
        }
        else if (DestroyOnReady == false && GameInfo.GetSingleton.FinaleReady == false)
        {
            Destroy(gameObject);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
