using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationHandler : MonoBehaviour
{
    public static IEnumerator WaitForAnimation(Animation anim, Action onComplete = null)
    {
        yield return new WaitUntil(() => { return !anim.isPlaying; });

        onComplete?.Invoke();
    }
}
