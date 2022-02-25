using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController: MonoBehaviour
{
    [SerializeField] Animator animator;

    public virtual void InitializeAnimator()
    { 
    
    }

    public virtual void UpdateAnimator()
    {
        
        
    }

    protected void EvaluateAnimation(string name, object value = null)
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
