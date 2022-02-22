using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oxygen : MonoBehaviour
{
    // In Seconds
    private const float MAX_OXYGEN = 120f;

    private float oxygenTimer;
    private float oxygenDecreaseMultiplier = 1f;

    public void Initialize()
    {
        this.oxygenTimer = MAX_OXYGEN;
    }

    private void Update()
    {
        if (this.oxygenTimer >= 0.01)
        {
            this.oxygenTimer -= this.oxygenDecreaseMultiplier * Time.deltaTime;
        }
    }

    public void SetOxygenDecreaseMultiplier(float value)
    {
        if (value < 1f)
            value = 1f;

        this.oxygenDecreaseMultiplier = value;
    }

    public void DecreaseOxygenTimer(float value)
    {
        this.oxygenTimer -= value;
    }
}
