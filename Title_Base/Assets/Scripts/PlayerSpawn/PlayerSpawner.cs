/****************************************************************************/
/*!
\file   PlayerSpawner.cs
\author Steven Gallwas
\brief  
    This file contains the implementation of the level manager
  
    © 2015 DigiPen, All Rights Reserved.
*/
/****************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerSpawner : EventHandler
{
    // list of all spawnpoints in the level 
    List<PlayerSpawnPoint> SpawnPoints = new List<PlayerSpawnPoint>();

    // the position to spawn the player if they don't exist
    public Vector3 DefaultPosition = new Vector3();

    // if we want to overide the spawn points for some reason
    public bool UseDefaultPosition;

    public bool SetCameraTarget = true;

    // bool to check if we spawned the player
    bool PlayerWasSpawned = false;

    // the player archetype to spawn
    public GameObject PlayerPrefab = null;

    GameObject Cam = null;
    GameStateControlledCamera CamController = null;

    // the last level that was loaded
    string PreviousLevel = "";


    public PlayerSpawner()
    {

    }

    void OnAddSpawnPointEvent(EventData eventData)
    {
        var data = eventData as PlayerSpawnEvent;

        AddSpawnPoint(data.SpawnPoint);
    }

    void AddSpawnPoint(PlayerSpawnPoint Pt)
    {
        SpawnPoints.Add(Pt);
    }

    void ClearSpawnPoints()
    {
        SpawnPoints.Clear();
    }

    void Awake () 
    {
        EventSystem.GlobalHandler.Connect(Events.AddSpawnPoint, OnAddSpawnPointEvent);

        Cam = GameObject.FindGameObjectWithTag("MainCamera");
       
        if(Cam)
        {
            CamController = Cam.GetComponent<GameStateControlledCamera>();
        }
    }

    void Start()
    {

    }
	
    Vector3 ChoosePosition()
    {
        if (SpawnPoints.Count == 0 || UseDefaultPosition)
        {
            return DefaultPosition;
        }

        // position to spawn the player
        Vector3 pos = DefaultPosition;

        // iterate over all spawn points 
        foreach (var SP in SpawnPoints)
        {
            // iterate through all level is each spawn point
            foreach (var lvl in SP.PreviousLevel)
            {
            // if the spawn point has the previous level, use that spawn positon
            if (lvl == PreviousLevel)
            {
                pos = SP.Position;
                return pos;
            }
            }
        }

        return pos;
    }


    void Update ()
    {
        // if we haven't spawned the player yet, spawn them
        if (false == PlayerWasSpawned)
        {
            PreviousLevel = LevelManager.GetSingleton.PrevLevel;

            Vector3 spawnPos = ChoosePosition();

            GameObject spawnedPlayer = (GameObject)GameObject.Instantiate(PlayerPrefab, spawnPos, Quaternion.identity);

          if(SetCameraTarget)
          {
            if(CamController && CamController.enabled)
            {
                CamController.AimTarget = spawnedPlayer;
                CamController.PosTarget = spawnedPlayer;
                CamController.SwitchAim(spawnedPlayer, float.Epsilon);
                CamController.UpdatePositionVector();
            }

          }

          PlayerWasSpawned = true;
        }
    }

    void OnDestroy()
    {
        EventSystem.GlobalHandler.Disconnect(Events.AddSpawnPoint, OnAddSpawnPointEvent);
    }
}

/****************************************************************************/
/*!
  \brief
    This is the event that gets sent when we want to move the selector
*/
/****************************************************************************/
public class PlayerSpawnEvent : EventData
{
  public PlayerSpawnPoint SpawnPoint;

  public PlayerSpawnEvent(PlayerSpawnPoint pos)
  {
    SpawnPoint = pos;
  }
}