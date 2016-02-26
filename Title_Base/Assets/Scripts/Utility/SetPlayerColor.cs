using UnityEngine;
using System.Collections;

public class SetPlayerColor : MonoBehaviour
{
    public Material PlayerColor = null;

    // Use this for initialization
    void Start ()
    {
        GameInfo.GetSingleton.PlayerColor = PlayerColor;
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
