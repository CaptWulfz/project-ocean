using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hallway1 : MonoBehaviour
{
   [SerializeField] private GameObject door;
    private void OnEnable() {
        EventBroadcaster.Instance.AddObserver(EventNames.ON_H1_DIALOG_END,OnH1DialogEnd);
    }
    private void OnDisable() {
        EventBroadcaster.Instance.RemoveObserver(EventNames.ON_H1_DIALOG_END);
    }

    private void OnH1DialogEnd(Parameters param = null)
    {
        door.SetActive(false);  
    }
}
