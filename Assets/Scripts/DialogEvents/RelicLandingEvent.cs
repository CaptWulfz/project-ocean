using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//THIS SCRIPT IS TO BE REFINED.
public class RelicLandingEvent : MonoBehaviour
{
    public void StartDialogEvent()
    {
        if (this.gameObject.name == "RELIC_LANDING")
            GameDirector.Instance.StartDialogSequence(TopicList.RELIC_LANDING);
        if (this.gameObject.name == "RELIC_ABYSS_LEFT_BOTTOM")
            GameDirector.Instance.StartDialogSequence(TopicList.ABYSS_SECOND_RELIC);
        if (this.gameObject.name == "RELIC_ABYSS_MIDDLE")
            GameDirector.Instance.StartDialogSequence(TopicList.ABYSS_FOURTH_RELIC);
        if (this.gameObject.name == "RELIC_ABYSS_RIGHT_TOP")
            GameDirector.Instance.StartDialogSequence(TopicList.ABYSS_THIRD_RELIC);
    }
}
