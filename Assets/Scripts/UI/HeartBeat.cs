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

    private void OnDestroy()
    {
        EventBroadcaster.Instance.RemoveObserverAtAction(EventNames.ON_PANIC_MODIFIED, HeartBeatSpeed);
    }

    public void HeartBeatSpeed(Parameters param = null)
    {
        //Debug.Log("HEART BEAT: " + param.GetParameter<float>("currPanicValue", 0f));
        this.animator.SetFloat("Heart_Beat_Value", param.GetParameter<float>("currPanicValue", 0f));
    }

    public void HeartStop()
    {
        //im dead lmao -gelo
        this.animator.SetBool("Heart_Beat_Dead", true);
        
    }
}
