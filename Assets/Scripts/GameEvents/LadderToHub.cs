using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadderToHub : MonoBehaviour
{
    [SerializeField] private GameObject vent;
    private void OnEnable() {
        EventBroadcaster.Instance.AddObserver(EventNames.ON_LADDER_TO_HUB_ENTRY,OnLadderToHubEntry);
    }
    private void OnDisable() {
        EventBroadcaster.Instance.RemoveObserver(EventNames.ON_LADDER_TO_HUB_ENTRY);
    }

    private void OnLadderToHubEntry(Parameters param = null)
    {
        vent.SetActive(true);  
    }
}
