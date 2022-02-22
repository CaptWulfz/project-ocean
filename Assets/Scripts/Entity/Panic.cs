using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PanicState
{
    CALM,
    NORMAL,
    DANGER,
    MAX
}
public class Panic : MonoBehaviour
{
    private const float MAX_THRESHOLD = 100f;
    private const float DANGER_THRESHOLD = 65f;

    private PanicState panicState;
    public PanicState PanicState
    {
        get { return this.panicState; }
    }
    private float panicValue = 0f;

    public void Initialize()
    {
        this.panicState = PanicState.NORMAL;
        this.panicValue = 0f;
    }

    public void ModifyPanicValue(float val)
    {
        this.panicValue += val;
        if (this.panicValue <= DANGER_THRESHOLD)
        {
            this.panicState = PanicState.NORMAL;
        } else if (this.panicValue >= DANGER_THRESHOLD && this.panicValue <= MAX_THRESHOLD)
        {
            this.panicState = PanicState.DANGER;
        } else if (this.panicValue >= MAX_THRESHOLD)
        {
            this.panicState = PanicState.MAX;
        }
    }
}
