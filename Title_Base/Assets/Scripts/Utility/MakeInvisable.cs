using UnityEngine;
using System.Collections;

[RequireComponent(typeof(MeshRenderer))]
public class MakeInvisable : MonoBehaviour
{

  MeshRenderer MRenderer = null;

	// Use this for initialization
	void Start () 
  {
	  MRenderer = gameObject.GetComponent<MeshRenderer>();
    MRenderer.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
