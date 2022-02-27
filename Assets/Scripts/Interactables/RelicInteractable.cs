using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicInteractable : Interactable
{
    [SerializeField] RelicLandingEvent dialogEvent;
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
                Destroy(this.gameObject);
                //WaitForRelicPickupEventBeforeDestroying();
            }
            base.OnSkillCheckFinished(param);
        }
    }

    //IEnumerator WaitForRelicPickupEventBeforeDestroying()
    //{
    //    yield return new WaitUntil()
    //    Destroy(this.gameObject);
    //}    
}
