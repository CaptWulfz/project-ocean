using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchRoom : MonoBehaviour
{
    [SerializeField] private GameObject door;
    private void OnEnable() {
        EventBroadcaster.Instance.AddObserver(ButtonNames.L1_SWITCH_ROOM,OnSwitchRoomButton);
    }
    private void OnDisable() {
        EventBroadcaster.Instance.RemoveObserver(ButtonNames.L1_SWITCH_ROOM);
    }

    private void OnSwitchRoomButton(Parameters param = null)
    {
        door.SetActive(false);  
    }
}
