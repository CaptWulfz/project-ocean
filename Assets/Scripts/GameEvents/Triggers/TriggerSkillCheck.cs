using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerSkillCheck : Interactable //Temp
{
    Player player;

    protected override void Initialize()
    {
        this.difficultyMode = PlayerSkillCheckDifficultyModes.Easy;
        this.skillCheckSpeed = 40f;
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
                this.gameObject.SetActive(false);
            }   
            base.OnSkillCheckFinished(param);
            //Dialogue
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        if (collision.gameObject.tag == TagNames.PLAYER)
        {
            this.player = collision.GetComponent<Player>();
            TriggerSkillCheck();
             //GameDirector.Instance.PerformDialogSequence(TopicList.ABYSS_FIRST_USE_OXYGEN);
        }
    }
}
