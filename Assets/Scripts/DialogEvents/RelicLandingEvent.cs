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
    }
}
