using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicInteractable : Interactable
{
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
                Debug.Log("Picked Up Relic!");
                this.gameObject.SetActive(false);
            }
            base.OnSkillCheckFinished(param);
        }
    }
}
