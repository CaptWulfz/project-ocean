using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameDirector : Singleton<GameDirector>
{
    private GameDirectorMain gameDirectorMain;

    private bool gameStart = false;
    public bool GameStart
    {
        get { return this.gameStart; }
        set { this.gameStart = value; }
    }

    private bool isDone = false;
    public bool IsDone
    {
        get { return this.isDone; }
    }

    #region Initialization
    public void Initialize()
    {
        this.gameDirectorMain = new GameDirectorMain();
        this.gameDirectorMain.InitializeSkillCheck();
        this.gameDirectorMain.InitializeEntities();
        StartCoroutine(WaitForInitialization());
    }

    private IEnumerator WaitForInitialization()
    {
        yield return new WaitUntil(() => { return this.gameDirectorMain.EntitiesMap != null && this.gameDirectorMain.SpawnPoints != null; });
        this.isDone = true;
    }
    #endregion

    private void Update()
    {
        if (!this.gameStart)
            return;

        this.gameDirectorMain.UpdateEntities();
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

    public void TrackPlayerLookState(Player.LookStates lookState)
    {
        this.gameDirectorMain.TrackPlayerLookState(lookState);
    }
    #endregion
}
