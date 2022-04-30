using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SmartWatch : MonoBehaviour
{
    [SerializeField] public Slider smartWatch;

    private void OnEnable()
    {
        EventBroadcaster.Instance.AddObserver(EventNames.ON_OXYGEN_MODIFIED, onOxygenModified);
    }

    private void OnDestroy()
    {
        EventBroadcaster.Instance.RemoveObserverAtAction(EventNames.ON_OXYGEN_MODIFIED, onOxygenModified);
    }

    private void onOxygenModified(Parameters param = null)
    {
        if (param != null)
        {
            float value = param.GetParameter<float>("currOxygenValue", 0f);
            this.smartWatch.value = value;
        }
    }

    public void ShowSmartWatch()
    {
        this.gameObject.SetActive(true);
    }

    public void HideSmartWatch()
    {
        this.gameObject.SetActive(false);
    }
}
