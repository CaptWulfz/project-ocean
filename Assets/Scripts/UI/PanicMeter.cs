using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PanicMeter : MonoBehaviour
{
    [SerializeField] Slider panic;
    private void OnEnable()
    {
        // Adding observer to a script, if this happened then trigger function
        EventBroadcaster.Instance.AddObserver(EventNames.ON_PANIC_INCREASE, OnPanicModified);
        EventBroadcaster.Instance.AddObserver(EventNames.ON_PANIC_DECREASE, OnPanicModified);
    }

    private void OnPanicModified(Parameters param = null)
    {
        if (param != null)
        {
            float value = param.GetParameter<float>("currPanicValue", 0f);
            this.panic.value = value;
        }
    }
}
