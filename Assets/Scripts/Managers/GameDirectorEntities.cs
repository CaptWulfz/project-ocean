using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class GameDirectorMain
{
    private const string DIRECTOR_ENTITIES_MAP_PATH = "AssetFiles/DirectorEntitiesMap";
    private const string SPAWN_POINTS_PATH = "AssetFiles/SpawnPoints";

    private const int MAX_ENTITY_COUNT = 8;

    private List<string> spawnableEntities;

    private Player.DirectionStates playerLookState;

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

    private bool stop = false;
    private bool isFrenzy = false;
    private float resetEntityDelay = 6f;
    private int collectedRelics = 0;

    private SoundSource preloadedSoundSource;
    private int prevIndex;
    private bool isReadyToSpawn = false;
    private float entityDelay;
    private int entityCount;

    public void InitializeEntities()
    {
        this.entitiesMap = Resources.Load<DirectorEntitiesMap>(DIRECTOR_ENTITIES_MAP_PATH);
        this.spawnPoints = Resources.Load<SpawnPoints>(SPAWN_POINTS_PATH);
        this.spawnableEntities = new List<string>();
        this.entityDelay = this.resetEntityDelay;
    }

    public void UpdateEntities()
    {
        if (stop)
            return;

        if (this.entityCount > MAX_ENTITY_COUNT)
            return;

        if (!this.isReadyToSpawn)
            return;

        if ((int)this.entityDelay <= 0)
        {
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

    public void TrackPlayerLookState(Player.DirectionStates lookState)
    {
        this.playerLookState = lookState;
    }

    private void LoadNextSoundModel()
    {
        this.preloadedSoundSource = GetRandomizedSoundSource();
        this.entityDelay = this.resetEntityDelay;
        this.entityDelay += preloadedSoundSource.SoundModel.DelayBeforeSpawn;
        this.isReadyToSpawn = true;
    }

    public void SpawnSoundSourceOutsideOfCamera()
    {
        if (this.entityCount < MAX_ENTITY_COUNT)
        {
            Vector2 coords = GetRandomizedSpawnLocation();
            Vector2 spawnTransform = Camera.main.ViewportToWorldPoint(coords);
            GameObject newSpawn = GameObject.Instantiate(this.preloadedSoundSource.gameObject);
            Debug.Log("Spawning: " + this.preloadedSoundSource.SoundModel.Name);
            newSpawn.SetActive(true);
            newSpawn.GetComponent<SoundSource>().Setup(() =>
            {
                if (this.entityCount > 0)
                    this.entityCount--;
            }, this.isFrenzy);
            newSpawn.transform.position = spawnTransform;
            this.entityCount++;
        }
        LoadNextSoundModel();
    }

    private SoundSource GetRandomizedSoundSource()
    {
        SoundSource source = null;

        int index = Random.Range(0, this.entitiesMap.Entities.Length);
        source = this.entitiesMap.Entities[index];

        if (CheckIfSpawnable(source.SoundModel.Name))
            return source;
        else
            return GetRandomizedSoundSource();
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
                coords = group.SpawnCoord;
                done = true;
            }
        }

        return coords;
    }

    private bool CheckIfSpawnable(string name)
    {

        bool spawnable = false;

        if (this.spawnableEntities.Count > 0)
        {
            spawnable = this.spawnableEntities.Exists((x) => { return x == name; });
        }

        return spawnable;
    }

    #region Injectors
    public void RegisterRelic(RelicType type)
    {
        this.collectedRelics++;
        switch (type)
        {
            case RelicType.SUMMON_WHALE:
                this.spawnableEntities.Add("Whale");
                break;
            case RelicType.SUMMON_WHISPERS:
                this.spawnableEntities.Add("Whispers");
                break;
            case RelicType.SUMMON_SCRATCHES:
                this.spawnableEntities.Add("Scratches");
                break;
            case RelicType.FRENZY:
                this.resetEntityDelay = 2f;
                this.isFrenzy = true;
                break;
        }

        if (!this.isFrenzy)
            this.entityDelay = 6 - this.collectedRelics;

        if (this.collectedRelics == 3)
        {
            EventBroadcaster.Instance.PostEvent(EventNames.ON_THREE_RELICS_COLLECTED);
        } else if (this.collectedRelics >= 4)
        {
            EventBroadcaster.Instance.PostEvent(EventNames.ON_ALL_RELICS_COLLECTED);
        }
    }

    public void StopAllProcess()
    {
        this.stop = true;
        EventBroadcaster.Instance.PostEvent(EventNames.ENTITY_KILL_YOURSELF);
    }

    public void SoftReset()
    {
        EventBroadcaster.Instance.PostEvent(EventNames.SOFT_RESET);
    }
    #endregion
}
