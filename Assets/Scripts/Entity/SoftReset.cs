using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoftReset : MonoBehaviour
{
    Panic panic;
    Oxygen oxgen;
    private Transform initialTransform;

    void Start()
    {
        this.initialTransform = this.gameObject.transform;
        EventBroadcaster.Instance.AddObserver(EventNames.SOFT_RESET, PerformSoftReset);
    }

    public void PerformSoftReset(Parameters param = null)
    {
        this.gameObject.transform.position = this.initialTransform.position;
    }

}
