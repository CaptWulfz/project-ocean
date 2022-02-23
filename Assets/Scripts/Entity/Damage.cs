using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damage : MonoBehaviour
{
    private float panicInfliction;
    public float PanicInfliction
    {
        get { return this.panicInfliction; }
    }
    private float oxygenInfliction;
    public float OxygenInfliction
    {
        get { return this.oxygenInfliction; }
    }

    public void SetupInflictionValues(float panicValue = 3f, float oxygenValue = 0.25f)
    {
        this.panicInfliction = panicValue;
        this.oxygenInfliction = oxygenValue;
    }
}
