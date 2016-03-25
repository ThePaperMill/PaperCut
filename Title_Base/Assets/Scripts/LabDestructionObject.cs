using UnityEngine;
using System.Collections;

public class LabDestructionObject : MonoBehaviour
{
    public bool DestroyWithLab = true;

    // Use this for initialization
    void Start()
    {
        if(DestroyWithLab == true && GameInfo.GetSingleton.LabDestroyed == true)
        {
            Destroy(gameObject);
        }
        else if (DestroyWithLab == false && GameInfo.GetSingleton.LabDestroyed == false)
        {
            Destroy(gameObject);
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
