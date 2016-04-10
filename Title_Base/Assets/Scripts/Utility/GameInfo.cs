using UnityEngine;
using System.Collections;

public class GameInfo : Singleton<GameInfo>
{
   public Material PlayerColor = null;
    public bool LabDestroyed = false;
    public bool FinaleReady = false;

    // Use this for initialization
    void Start ()
    {
	
	}
	
    public void ResetBools()
    {
        LabDestroyed = false;
    }

	// Update is called once per frame
	void Update ()
    {
	
	}
}
