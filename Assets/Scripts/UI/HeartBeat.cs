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
        EventBroadcaster.Instance.AddObserver(EventNames.HEART_BEAT_CHECKER, HeartBeatSpeed);
    }
    public void HeartBeatSpeed(Parameters param = null)
    {
        this.animator.SetFloat("Heart_Beat_Value", param.GetParameter<float>("Heart_Beat_Value", 0f));
    }
}
