using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeartBeat : MonoBehaviour
{
    private Animator animator;
    void Awake()
    {
        animator = this.GetComponent<Animator>();
        EventBroadcaster.Instance.AddObserver(EventNames.ON_PANIC_MODIFIED, HeartBeatSpeed);
    }
    public void HeartBeatSpeed(Parameters param = null)
    {
        Debug.Log("HEART BEAT: " + param.GetParameter<float>("currPanicValue", 0f));
        this.animator.SetFloat("Heart_Beat_Value", param.GetParameter<float>("currPanicValue", 0f));
    }
}
