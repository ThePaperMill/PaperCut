using UnityEngine;
using System.Collections;

public class HideDuringFade : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (LevelTransitionManager.GetSingleton.Fading == true)
        {
            GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            GetComponent<MeshRenderer>().enabled = true;
        }
    }
}
