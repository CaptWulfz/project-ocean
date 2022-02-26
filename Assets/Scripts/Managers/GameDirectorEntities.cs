using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class GameDirectorMain
{
    private const string DIRECTOR_ENTITIES_MAP_PATH = "AssetFiles/DirectorEntitiesMap";
    private const string SPAWN_POINTS_PATH = "AssetFiles/SpawnPoints";

    private const float ENTITY_DELAY = 2f;

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

    private SoundSource preloadedSoundSource;
    private int prevIndex;
    private bool isReadyToSpawn = false;
    private float entityDelay;

    public void InitializeEntities()
    {
        this.entitiesMap = Resources.Load<DirectorEntitiesMap>(DIRECTOR_ENTITIES_MAP_PATH);
        this.spawnPoints = Resources.Load<SpawnPoints>(SPAWN_POINTS_PATH);
        this.entityDelay = ENTITY_DELAY;
    }

    public void UpdateEntities()
    {
        if (!this.isReadyToSpawn)
            return;

        if ((int)this.entityDelay <= 0)
        {
            this.entityDelay = ENTITY_DELAY;
            SpawnSoundSourceOutsideOfCamera();
        } else
        {
            this.entityDelay -= Time.deltaTime;
        }
    }

    public void StartEntities()
    {
        LoadNextSoundModel();
    }

    public void TrackPlayerLookState(Player.LookStates lookState)
    {
        this.playerLookState = lookState;
    }

    private void LoadNextSoundModel()
    {
        this.preloadedSoundSource = GetRandomizedSoundSource();
        this.entityDelay += preloadedSoundSource.SoundModel.DelayBeforeSpawn;
        this.isReadyToSpawn = true;
    }

    public void SpawnSoundSourceOutsideOfCamera()
    {
        Vector2 coords = GetRandomizedSpawnLocation();
        Vector2 spawnTransform = Camera.main.ViewportToWorldPoint(coords);
        GameObject newSpawn = GameObject.Instantiate(this.preloadedSoundSource.gameObject);
        Debug.Log("Spawning: " + this.preloadedSoundSource.SoundModel.Name);
        newSpawn.SetActive(true);
        newSpawn.GetComponent<SoundSource>().Setup();
        newSpawn.transform.position = spawnTransform;
        LoadNextSoundModel();
    }

    //private SoundModel GetRandomizedSoundModel()
    //{
    //    SoundModel model = null;
    //    bool done = false;

    //    //int index = 0;
    //    //while (!done)
    //    //{
    //    //    index = Random.Range(0, this.entitiesMap.SoundModels.Length);
    //    //    if (index != this.prevIndex)
    //    //        done = true;
    //    //}

    //    int index = Random.Range(0, this.entitiesMap.SoundModels.Length);

    //    this.prevIndex = index;
    //    model = this.entitiesMap.SoundModels[index];

    //    return model;
    //}

    private SoundSource GetRandomizedSoundSource()
    {
        SoundSource source = null;
        //bool done = false;

        //int index = 0;
        //while (!done)
        //{
        //    index = Random.Range(0, this.entitiesMap.SoundModels.Length);
        //    if (index != this.prevIndex)
        //        done = true;
        //}

        int index = Random.Range(0, this.entitiesMap.Entities.Length);

        this.prevIndex = index;
        source = this.entitiesMap.Entities[index];

        return source;
    }


    private Vector2 GetRandomizedSpawnLocation()
    {
        Vector2 coords = Vector2.zero;
        bool done = false;

        while (!done)
        {

            int index = Random.Range(0, this.spawnPoints.DirectionToCoord.Length);
            SpawnPoints.LookStateToVector2 group = this.spawnPoints.DirectionToCoord[index];
            if (group.LookState != this.playerLookState)
            {
                //Debug.Log("QQQ SPAWNING AT: " + group.LookState);
                coords = group.SpawnCoord;
                done = true;
            }
        }

        return coords;
    }
}
