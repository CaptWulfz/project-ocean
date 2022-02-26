using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameDirector : Singleton<GameDirector>
{
    private GameDirectorMain gameDirectorMain;
    private EventDialogGroup topicList;
    private EventDialogManager dialogManager;

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
        this.topicList = Resources.Load<EventDialogGroup>("AssetFiles/Dialog/TopicList");
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
        //this.gameDirectorMain.UpdateSkillCheck();
    }

    #region Helpers
    public void StartGame()
    {
        this.gameStart = true;
        this.gameDirectorMain.StartEntities();
    }
    public void RegisterSkillCheck(SkillCheck check)
    {
        this.gameDirectorMain.RegisterSkillCheck(check);
    }

    public void TrackPlayerSpeedState(Player.SpeedStates speedState)
    {
//        this.gameDirectorMain.TrackPlayerSpeedState(speedState);
    }

    public void TrackPlayerLookState(Player.LookStates lookState)
    {
        this.gameDirectorMain.TrackPlayerLookState(lookState);
    }

    public void TriggerSkillCheck(Transform target, SkillCheck.PlayerSkillCheckDifficulty difficulty)
    {
        this.gameDirectorMain.TriggerSkillCheck(target, difficulty);
    }

    public void StartDialogSequence(TopicList topic)
    {
        EventDialog dialog = this.topicList.EventDialogs[(int)topic];
        this.dialogManager.GenerateDialogSequence(dialog);
        Time.timeScale = 0;
    }

    public void RegisterEventDialogManager(EventDialogManager dialogManager)
    {
        this.dialogManager = dialogManager;
    }
    #endregion
}

public enum TopicList
{
    INTRO,
    INTRO_EXIT,
    RELIC_LANDING
}