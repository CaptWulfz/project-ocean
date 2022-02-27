using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RelicCollectionEvent : MonoBehaviour
{
    private void Start()
    {
        EventBroadcaster.Instance.AddObserver(EventNames.ON_THREE_RELICS_COLLECTED, OnThreeRelicsCollected);
        this.gameObject.SetActive(false);
    }

    private void OnDestroy()
    {
        EventBroadcaster.Instance.RemoveObserverAtAction(EventNames.ON_THREE_RELICS_COLLECTED, OnThreeRelicsCollected);
    }

    private void OnThreeRelicsCollected(Parameters param  = null)
    {
        this.gameObject.SetActive(true);
    }
}
