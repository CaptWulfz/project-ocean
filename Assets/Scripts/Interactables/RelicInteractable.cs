using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicInteractable : Interactable
{
    [SerializeField] RelicLandingEvent dialogEvent;
    [SerializeField] RelicType relicType;
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
                Parameters param2 = new Parameters();
                param2.AddParameter<int>("relicID", this.gameObject.GetInstanceID());
                EventBroadcaster.Instance.PostEvent(EventNames.ON_RELIC_PICK_UP, param2);
                dialogEvent.StartDialogEvent();
                GameDirector.Instance.RegisterRelic(this.relicType);
                Destroy(this.gameObject);
                //WaitForRelicPickupEventBeforeDestroying();
            }
            base.OnSkillCheckFinished(param);
        }
    }
}

public enum RelicType
{
    SUMMON_WHALE,
    SUMMON_SCRATCHES,
    SUMMON_WHISPERS,
    FRENZY
}
