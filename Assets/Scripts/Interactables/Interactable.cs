using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    protected SkillCheck.PlayerSkillCheckDifficulty skillDifficulty;
    protected PlayerSkillCheckDifficultyModes difficultyMode;
    protected float skillCheckSpeed;
    protected bool skillRotateRandom;

    private void Start()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        this.skillDifficulty = new SkillCheck.PlayerSkillCheckDifficulty();
        skillDifficulty.modes = difficultyMode;
        skillDifficulty.skillCheckSpeed = skillCheckSpeed;
        skillDifficulty.rotateSkillCheckRandom = skillRotateRandom;
    }

    public void TriggerSkillCheck()
    {
        GameDirector.Instance.TriggerSkillCheck(this.transform, skillDifficulty);
        EventBroadcaster.Instance.AddObserver(EventNames.ON_SKILL_CHECK_FINISHED, OnSkillCheckFinished);
    }

    protected virtual void OnSkillCheckFinished(Parameters param = null)
    {
        EventBroadcaster.Instance.RemoveObserverAtAction(EventNames.ON_SKILL_CHECK_FINISHED, OnSkillCheckFinished);
    }


}