using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameDirector : Singleton<GameDirector>
{
    private GameDirectorMain gameDirectorMain;

    private const string DIRECTOR_ENTITIES_MAP_PATH = "AssetFiles/DirectorEntitiesMap";
  
    private DirectorEntitiesMap entitiesMap;

    private bool allowUpdate;

    private bool isDone = false;
    public bool IsDone
    {
        get { return this.isDone; }
    }

    #region Initialization
    public void Initialize()
    {
        this.entitiesMap = Resources.Load<DirectorEntitiesMap>(DIRECTOR_ENTITIES_MAP_PATH);
        this.gameDirectorMain = new GameDirectorMain();
        this.allowUpdate = true;
        this.gameDirectorMain.InitializeSkillCheck();
        StartCoroutine(WaitForInitialization());
    }

    private IEnumerator WaitForInitialization()
    {
        yield return new WaitUntil(() => { return this.entitiesMap != null; });
        this.isDone = true;
    }
    #endregion

    private void Update()
    {
       
        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("Spawn Source Outside of Camera");
            GameDirector.Instance.SpawnSoundSourceOutsideOfCamera();
        }

        if (!allowUpdate)
            return;

        this.gameDirectorMain.UpdateSkillCheck();
    }

    #region Helpers
    public void RegisterSkillCheck(SkillCheck check)
    {
        this.gameDirectorMain.RegisterSkillCheck(check);
    }

    public void TrackPlayerSpeedState(Player.SpeedStates speedState)
    {
        this.gameDirectorMain.TrackPlayerSpeedState(speedState);
    }
    #endregion

    #region Test Functions
    public void SpawnSoundSourceOutsideOfCamera()
    {
        Vector2 spawnTransform = Camera.main.ViewportToWorldPoint(new Vector2(-0.1f, 0.5f));
        SoundSource source = this.entitiesMap.SoundSourceReference;
        GameObject newSpawn = GameObject.Instantiate(source.gameObject);
        newSpawn.SetActive(true);
        newSpawn.GetComponent<SoundSource>().Setup(this.entitiesMap.SoundModels[0]);
        newSpawn.transform.position = spawnTransform;
    }
    #endregion
}
