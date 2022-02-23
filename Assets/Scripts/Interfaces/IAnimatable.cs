using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IAnimatable
{
    struct AnimatorParameters
    {
        public string name;
        public object value;
    }

    void SendAnimatorParameters(AnimatorParameters animEvent);
}
