/*********************************
 * GameStateControlledCamera.cs
 * Ian Aemmer and Jerry Nacier
 * Created 9/7/2015
 * Copyright © 2015 DigiPen Institute of Technology, All Rights Reserved
 *********************************/

using UnityEngine;
using System.Collections;

public class GameStateControlledCamera : MonoBehaviour {

	public GameObject blarg;

	//target Aim is the location of the object we want our camera to EVENTUALLY be focusing on
	public Vector3 targetAim;
	//lastAim is the location of Where we were focusing on BEFORE we switched targets
	public Vector3 lastAim;
	//This is where we are now.
	public Vector3 currentAim;
	//This is the entire period of time that we want to take to switch between our current target and our last target
	public float aimTime;
	//This is the currently reached period of time that we are in since switching targets.
	public float currentAimTime;

	//public Vector3 targetPos;
	//public Vector3 targetPos;
	//public float posTime;

	// Use this for initialization
	void Start () {


	}
	
	// Update is called once per frame
	void Update () {


	}


	public void SwitchAim(Vector3 newAim, float newTime)
	{
	
	}
}
