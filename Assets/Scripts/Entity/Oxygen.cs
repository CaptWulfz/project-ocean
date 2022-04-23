using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Oxygen : MonoBehaviour
{
    [SerializeField] Player player;
    private bool noOxygen;
    private bool paused;


    // In Seconds
    private const float MAX_OXYGEN = 120f;

    private float oxygenTimer;
    
    private float oxygenDecreaseMultiplier = 0.5f;

    private bool stop = false;

    public float OxygenTimer
    {
        get { return this.oxygenTimer; }
        set { this.oxygenTimer = value; }
    }
    public bool NoOxygen
    {
        get { return this.noOxygen; }
        set { this.noOxygen = value;}
    }
    public void Initialize()
    {
        noOxygen = false;
        this.oxygenTimer = MAX_OXYGEN;
        EventBroadcaster.Instance.AddObserver(EventNames.ENTITY_KILL_YOURSELF, OnEntityKillYourself);
    }

    private void OnEntityKillYourself(Parameters param = null)
    {
        this.stop = true;
    }

    private void Update()
    {
        if (stop)
            return;

        if (noOxygen)
        {
            Debug.Log("NO MORE OXYGEN");
            return;
        }

        if (!this.paused)
        {
            OxygenStart();
        }
        //Parameters param = new Parameters();
        //float updatedOxygenValue = this.oxygenTimer / MAX_OXYGEN;
        //if (this.oxygenTimer >= 0.01)
        //{
        //    param.AddParameter<float>("currOxygenValue", updatedOxygenValue);
        //    EventBroadcaster.Instance.PostEvent(EventNames.ON_OXYGEN_MODIFIED, param);

        //    this.oxygenTimer -= this.oxygenDecreaseMultiplier * Time.deltaTime;
        //} else
        //{
        //    noOxygen = true;
        //    this.player.OnOxygenStageDead();
        //}
    }

    public void OxygenStart()
    {
        Parameters param = new Parameters();
        float updatedOxygenValue = this.oxygenTimer / MAX_OXYGEN;
        if (this.oxygenTimer >= 0.01)
        {
            param.AddParameter<float>("currOxygenValue", updatedOxygenValue);
            EventBroadcaster.Instance.PostEvent(EventNames.ON_OXYGEN_MODIFIED, param);
            //Debug.Log("OXYGEN CHECKER: " + this.oxygenTimer);
            this.oxygenTimer -= this.oxygenDecreaseMultiplier * Time.deltaTime;
        }
        else
        {
            noOxygen = true;
            this.player.OnOxygenStageDead();
        }
    }

    public void CheckPause(bool paused)
    {
        this.paused = paused;
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

    public void AddOxygen(float value)
    {
        this.oxygenTimer += value;
    }

}
