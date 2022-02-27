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
    private CameraAudioSource camSource;

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
        
    }

    #region Helpers
    public void StartGame()
    {
        this.gameStart = true;
        this.gameDirectorMain.StartEntities();
    }

    public void RegisterRelic(RelicType relicType)
    {
        this.gameDirectorMain.RegisterRelic(relicType);
    }

    public void RegisterCamAudioSource(CameraAudioSource camSource)
    {
        this.camSource = camSource;
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
    //https://docs.google.com/document/d/1nhuJ1ToxMY_Xo6KdJGspUvjosH8EbcQUx89hB3ad4b0/edit
    //page 7 onwards

    INTRO, //Dialog Intro
    INTRO_EXIT, //Dialog Level 1
    RELIC_LANDING, //Dialog Level 1 prologue
    ABYSS_FIRST_MINE, //done
    ABYSS_REVEALS_FIRST_MIRAGE_MINE, //to do
    ABYSS_PASS_THROUGH_FIRST_MIRAGE_MINE, //to do
    ABYSS_FIRST_USE_OXYGEN, //done
    ABYSS_FIRST_MONSTER_ENCOUNTER, //done
    ABYSS_SECOND_RELIC, //done -- to attach
    ABYSS_ENCOUNTER_FEW_MORE_ENTITIES, //done
    PLAYER_REACHES_50_PANIC, //done
    PLAYER_REACHES_90_PANIC, //done
    ABYSS_THIRD_RELIC, //done -- to attach
    ABYSS_FOURTH_RELIC //done -- to attach
}