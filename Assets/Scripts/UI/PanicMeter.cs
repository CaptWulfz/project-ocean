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
        EventBroadcaster.Instance.AddObserver(EventNames.ON_PANIC_MODIFIED, OnPanicModified);
    }

    private void OnPanicModified(Parameters param = null)
    {
        if (param != null)
        {
            float updatedPanicValue = param.GetParameter<float>("currPanicValue", 0f) / param.GetParameter<float>("maxPanicValue", 0f);
            this.panic.value = updatedPanicValue;
        }
    }
}
