using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class GameDirectorMain
{
    private const string DIRECTOR_ENTITIES_MAP_PATH = "AssetFiles/DirectorEntitiesMap";
    private const string SPAWN_POINTS_PATH = "AssetFiles/SpawnPoints";

    private const float ENTITY_DELAY = 6f;

    private Player.LookStates playerLookState;

    private DirectorEntitiesMap entitiesMap;
    public DirectorEntitiesMap EntitiesMap
    {
        get { return this.entitiesMap; }
    }

    private SpawnPoints spawnPoints;
    public SpawnPoints SpawnPoints
    {
        get { return this.spawnPoints; }
    }

    private float entityDelay;

    public void InitializeEntities()
    {
        this.entitiesMap = Resources.Load<DirectorEntitiesMap>(DIRECTOR_ENTITIES_MAP_PATH);
        this.spawnPoints = Resources.Load<SpawnPoints>(SPAWN_POINTS_PATH);
        this.entityDelay = ENTITY_DELAY;
    }

    public void UpdateEntities()
    {
        //if (Keyboard.current.spaceKey.wasPressedThisFrame)
        //{
        //    Debug.Log("Spawn Source Outside of Camera");
        //    SpawnSoundSourceOutsideOfCamera();
        //}

        if ((int)this.entityDelay <= 0)
        {
            this.entityDelay = ENTITY_DELAY;
            SpawnSoundSourceOutsideOfCamera();
        } else
        {
            this.entityDelay -= Time.deltaTime;
        }
    }

    public void TrackPlayerLookState(Player.LookStates lookState)
    {
        this.playerLookState = lookState;
    }

    public void SpawnSoundSourceOutsideOfCamera()
    {
        Vector2 coords = GetRandomizedSpawnLocation();
        Vector2 spawnTransform = Camera.main.ViewportToWorldPoint(coords);
        SoundSource source = this.entitiesMap.SoundSourceReference;
        GameObject newSpawn = GameObject.Instantiate(source.gameObject);
        newSpawn.SetActive(true);
        newSpawn.GetComponent<SoundSource>().Setup(this.entitiesMap.SoundModels[0]);
        newSpawn.transform.position = spawnTransform;
    }

    private Vector2 GetRandomizedSpawnLocation()
    {
        Vector2 coords = Vector2.zero;
        bool done = false;

        while (!done)
        {

            int index = Random.Range(0, 7);
            SpawnPoints.LookStateToVector2 group = this.spawnPoints.DirectionToCoord[index];
            if (group.LookState != this.playerLookState)
            {
                Debug.Log("QQQ SPAWNING AT: " + group.LookState);
                coords = group.SpawnCoord;
                done = true;
            }
        }

        return coords;
    }
}
