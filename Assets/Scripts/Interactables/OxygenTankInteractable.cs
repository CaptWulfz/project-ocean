using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenTankInteractable : Interactable
{
    Player player;
    bool isFirstOxygenTank = true;
    protected override void Initialize()
    {
        this.difficultyMode = PlayerSkillCheckDifficultyModes.Medium;
        this.skillCheckSpeed = 60f;
        this.skillRotateRandom = true;
        base.Initialize();
    }

    protected override void OnSkillCheckFinished(Parameters param = null)
    {
        if (param != null)
        {
            bool success = param.GetParameter<bool>(ParameterNames.SKILLCHECK_RESULT, false);
            if (success)
            {
                this.player.AddOxygen(30f);
                this.gameObject.SetActive(false);
            }
            base.OnSkillCheckFinished(param);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.gameObject.tag == TagNames.PLAYER)
        {
            this.player = collision.GetComponent<Player>();

            if (isFirstOxygenTank)
            {
                GameDirector.Instance.PerformDialogSequence(TopicList.ABYSS_FIRST_USE_OXYGEN);
                isFirstOxygenTank = false;
            }
        }
    }
}
