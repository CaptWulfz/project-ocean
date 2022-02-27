using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicShrineInteractable : Interactable
{
    [SerializeField] GameObject activeShrines;
    [SerializeField] GameObject inactiveShrines;

    protected override void Initialize()
    {
        this.difficultyMode = PlayerSkillCheckDifficultyModes.Hard;
        this.skillCheckSpeed = 75f;
        this.skillRotateRandom = true;
        ToggleShrines(false);
        base.Initialize();
    }

    protected override void OnSkillCheckFinished(Parameters param = null)
    {
        if (param != null)
        {
            bool success = param.GetParameter<bool>(ParameterNames.SKILLCHECK_RESULT, false);
            if (success)
            {
                Debug.Log("Relic Shrine Activated!");
                ToggleShrines(true);
                
            }
            base.OnSkillCheckFinished(param);
        }
    }

    private void ToggleShrines(bool active)
    {
        this.inactiveShrines.SetActive(!active);
        this.activeShrines.SetActive(active);
    }
}
