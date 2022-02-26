using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController: MonoBehaviour
{
    [SerializeField] protected Animator animator;
    public Animator Animator
    {
        get { return this.animator; }
    }

    public virtual void InitializeAnimator()
    { 
    
    }
    public virtual void UpdateAnimator()
    {


    }

    public bool IsAnimationPlaying() // Animation play time
    {
        return this.animator.GetCurrentAnimatorStateInfo(0).length >
                this.animator.GetCurrentAnimatorStateInfo(0).normalizedTime;
    }

    public IEnumerator WaitForAnimationToFinish(string stateName, Action onComplete = null)
    {
        while (!this.animator.GetCurrentAnimatorStateInfo(0).IsName(stateName))
            yield return null;
        yield return new WaitUntil(() => { return !IsAnimationPlaying(); });
        onComplete?.Invoke();
    }

    public void EvaluateAnimation(string name, object value = null)
    {
        Type type = null;

        if (value != null)
            type = value.GetType();

        if (type == typeof(bool))
        {
            //Debug.Log("Is Bool");
            this.animator.SetBool(name, (bool) value);
        } else if (type == typeof(int))
        {
            //Debug.Log("Is Int");
            this.animator.SetInteger(name, (int) value);
        } else if (type == typeof(float))
        {
            //Debug.Log("Is Float");
            this.animator.SetFloat(name, (float) value);
        } else if (type == null)
        {
            //Debug.Log("Is Trigger");
            this.animator.SetTrigger(name);
        }
    }
}
