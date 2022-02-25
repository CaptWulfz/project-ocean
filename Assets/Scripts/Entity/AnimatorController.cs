using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorController : MonoBehaviour
{
    [SerializeField] Animator animator;
    
    public void EvaluateValues(Dictionary<string, object> paramToValue)
    {
        
    }

    protected void EvaluateAnimation(string name, object value)
    {
        Type type = null;

        if (value != null)
            type = typeof(object);

        if (type == typeof(string))
        {
            Debug.Log("This is a string");
        } else if (type == typeof(int))
        {
            Debug.Log("Is Int");
        } else if (type == typeof(float))
        {
            Debug.Log("Is Float");
        } else if (type == null)
        {
            Debug.Log("Is Trigger");
        }
    }
}
