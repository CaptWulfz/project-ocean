using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public partial class GameDirectorMain
{
    private const float SKILL_CHECK_DELAY = 4f;
    private const int EASY_SKILL_CHECK_LIMIT = 2;
    private const int MEDIUM_SKILL_CHECK_LIMIT = 4;

    private const float EASY_SKILL_CHECK_SPEED = 20f;
    private const float MEDIUM_SKILL_CHECK_SPEED = 35f;
    private const float HARD_SKILL_CHECK_SPEED = 60f;

    // Skill Checks
    private Player.SpeedStates playerSpeedState;
    private SkillCheck skillCheck;
    private SkillCheck.PlayerSkillCheckDifficulty skillCheckDifficulty;
    private PlayerSkillCheckDifficultyModes currentSkillCheckMode;
    private float currentSkillCheckSpeed;
    private bool currentRotateRandom;
    private bool isSkillCheckActive;
    private float skillCheckDelay;
    private int successfulSkillChecks;

    public void InitializeSkillCheck()
    {
        InitializeSkillCheckSettings();
        EventBroadcaster.Instance.AddObserver(EventNames.ON_SKILL_CHECK_FINISHED, OnSkillCheckFinished);
    }

    private void InitializeSkillCheckSettings()
    {
        this.successfulSkillChecks = 0;
        this.skillCheckDelay = SKILL_CHECK_DELAY;
        this.isSkillCheckActive = false;
        this.currentSkillCheckMode = PlayerSkillCheckDifficultyModes.Easy;

        EvaluateSuccessfulSkillChecks();
    }

    public void UpdateSkillCheck()
    {
        if (this.skillCheck == null)
            return;

        if (this.playerSpeedState == Player.SpeedStates.MAX)
        {
            if (this.isSkillCheckActive)
                return;

            this.skillCheckDelay -= Time.deltaTime;
            Debug.Log("DELAY: " + (int) this.skillCheckDelay);
            if ((int) this.skillCheckDelay <= 0)
            {
                this.skillCheck.TriggerSkillCheck(this.skillCheckDifficulty, TagNames.PLAYER);
                this.isSkillCheckActive = true;
            }
        } else
        {
            if (this.isSkillCheckActive)
            {
                // Force Terminate Skill Check
            }
        }
    }

    private void DetermineSkillCheckDifficulty(PlayerSkillCheckDifficultyModes mode, float skillSpeed, bool rotateRandom)
    {
        this.skillCheckDifficulty.modes = mode;
        this.skillCheckDifficulty.skillCheckSpeed = skillSpeed;
        this.skillCheckDifficulty.rotateSkillCheckRandom = rotateRandom;
    }

    private void EvaluateSkillCheckDifficulty(bool fromFailure = false)
    {
        switch (this.currentSkillCheckMode)
        {
            case PlayerSkillCheckDifficultyModes.Easy:
                this.currentSkillCheckSpeed = EASY_SKILL_CHECK_SPEED;
                this.currentRotateRandom = false;
                break;
            case PlayerSkillCheckDifficultyModes.Medium:
                this.currentSkillCheckSpeed = MEDIUM_SKILL_CHECK_SPEED;
                this.currentRotateRandom = true;
                break;
            case PlayerSkillCheckDifficultyModes.Hard:
                float hardSkillCheckSpeed = !fromFailure ? this.currentSkillCheckSpeed : this.currentSkillCheckSpeed + 15f;
                this.currentSkillCheckSpeed = hardSkillCheckSpeed;
                this.currentRotateRandom = true;
                break;
        }

        DetermineSkillCheckDifficulty(this.currentSkillCheckMode, this.currentSkillCheckSpeed, this.currentRotateRandom);
    }

    private void EvaluateSuccessfulSkillChecks()
    {
        if (this.successfulSkillChecks <= EASY_SKILL_CHECK_LIMIT)
        {   
            if ((int) this.currentSkillCheckMode < (int) PlayerSkillCheckDifficultyModes.Easy)
                this.currentSkillCheckMode = PlayerSkillCheckDifficultyModes.Easy;
        } else if (this.successfulSkillChecks > EASY_SKILL_CHECK_LIMIT && this.successfulSkillChecks <= MEDIUM_SKILL_CHECK_LIMIT)
        {
            if ((int)this.currentSkillCheckMode < (int)PlayerSkillCheckDifficultyModes.Medium)
                this.currentSkillCheckMode = PlayerSkillCheckDifficultyModes.Medium;
        } else if (this.successfulSkillChecks > MEDIUM_SKILL_CHECK_LIMIT)
        {
            if ((int)this.currentSkillCheckMode < (int)PlayerSkillCheckDifficultyModes.Hard)
            {
                this.currentSkillCheckMode = PlayerSkillCheckDifficultyModes.Hard;
                this.currentSkillCheckSpeed = HARD_SKILL_CHECK_SPEED;
            }
        }

        EvaluateSkillCheckDifficulty();
    }

    #region Injector Methods
    public void TrackPlayerSpeedState(Player.SpeedStates state)
    {
        this.playerSpeedState = state; ;
        if (state != Player.SpeedStates.MAX)
        {
            InitializeSkillCheckSettings();
            //Debug.Log("QQQ NO SPEED");
        } else
        {
            //Debug.Log("QQQ MAX SPEED");
        }

    }

    public void RegisterSkillCheck(SkillCheck check)
    {
        this.skillCheck = check;
        this.skillCheckDifficulty = new SkillCheck.PlayerSkillCheckDifficulty();
        EvaluateSuccessfulSkillChecks();
    }
    #endregion

    #region Event Broadcaster Events
    private void OnSkillCheckFinished(Parameters param = null)
    {
        if (param != null)
        {
            bool success = param.GetParameter<bool>(ParameterNames.SKILLCHECK_RESULT, false);
            if (success)
            {
                this.successfulSkillChecks += 1;
                EvaluateSuccessfulSkillChecks();
            } else
            {
                if (this.currentSkillCheckMode == PlayerSkillCheckDifficultyModes.Easy)
                    this.currentSkillCheckMode = PlayerSkillCheckDifficultyModes.Medium;
                else if (this.currentSkillCheckMode == PlayerSkillCheckDifficultyModes.Medium)
                {
                    this.currentSkillCheckMode = PlayerSkillCheckDifficultyModes.Hard;
                    this.currentSkillCheckSpeed = HARD_SKILL_CHECK_SPEED;
                }


                EvaluateSkillCheckDifficulty(true);
            }

            this.skillCheckDelay = SKILL_CHECK_DELAY;
            this.isSkillCheckActive = false;
        }
    }
    #endregion
}
