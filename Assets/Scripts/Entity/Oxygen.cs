using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Oxygen : MonoBehaviour
{
    [SerializeField] Player player;

    // In Seconds
    private const float MAX_OXYGEN = 120f;

    private float oxygenTimer;
    private float oxygenDecreaseMultiplier = 0.5f;

    public void Initialize()
    {
        this.oxygenTimer = MAX_OXYGEN;
    }

    private void Update()
    {
        Debug.Log("OXYGEN CONSUMPTION: " + oxygenDecreaseMultiplier);

        Parameters param = new Parameters();
        float updatedOxygenValue = this.oxygenTimer / MAX_OXYGEN;
        if (this.oxygenTimer >= 0.01)
        {
            param.AddParameter<float>("currOxygenValue", updatedOxygenValue);
            EventBroadcaster.Instance.PostEvent(EventNames.ON_OXYGEN_MODIFIED, param);

            this.oxygenTimer -= this.oxygenDecreaseMultiplier * Time.deltaTime;
        } else
        {
            this.player.OnOxygenStageDead();
        }
    }

    public void SetOxygenDecreaseMultiplier(float value)
    {
        if (value < 0.5f)
            value = 0.5f;

        this.oxygenDecreaseMultiplier = value;
    }

    public void DecreaseOxygenTimer(float value)
    {
        this.oxygenTimer -= value;
    }

}
