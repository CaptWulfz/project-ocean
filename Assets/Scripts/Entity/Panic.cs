using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum PanicState
{
    CALM, // 1-29
    NORMAL, // 30-59
    DANGER, // 60-89
    DYING, // 90-99
    DEAD // 100
}
public class Panic : MonoBehaviour
{
    [SerializeField] Player player;

    private const float MAX_THRESHOLD = 100f;
    private const float DYING_THRESHOLD = 90f;
    private const float DANGER_THRESHOLD = 60f;
    private const float NORMAL_THRESHOLD = 30f;

    private PanicState panicState;
    public PanicState PanicState
    {
        get { return this.panicState; }
    }

    private float panicValue = 0f;

    private float panicMultiplier = 0f;

    private void Awake()
    {
        EventBroadcaster.Instance.AddObserver(EventNames.ON_SKILL_CHECK_FINISHED, SkillCheckResult);
    }

    public void Initialize()
    {
        this.panicValue = 0f;
        this.panicState = PanicState.CALM;
    }

    private void Update()
    {
        this.panicValue += this.panicMultiplier * Time.deltaTime;

        DeterminePanicState();
    }

    public void IncreasePanicValue(float val)
    {
        this.panicValue += val;
    }

    public void ApplyPanicPressure(float value)
    {
        this.panicMultiplier += value;
    }

    public void RemovePanicPressure(float value)
    {
        this.panicMultiplier -= value;
    }

    public void DecreasePanicValue(float value)
    {      
        this.panicValue -= value;
    }

    private void DeterminePanicState()
    {
        if (this.panicValue < NORMAL_THRESHOLD) //30
        {
            this.panicState = PanicState.CALM;

        } else if (this.panicValue >= NORMAL_THRESHOLD && this.panicValue < DANGER_THRESHOLD) //30-59    60
        {
            this.panicState = PanicState.NORMAL;

        } else if (this.panicValue >= DANGER_THRESHOLD && this.panicValue < DYING_THRESHOLD) //60-89     90
        {
            this.panicState = PanicState.DANGER;

        } else if (this.panicValue >= DYING_THRESHOLD && this.panicValue < MAX_THRESHOLD)// 100
        {
            this.panicState = PanicState.DYING;
        } else if (this.panicValue >= MAX_THRESHOLD)
        {
            this.panicState = PanicState.DEAD;
        }

        Parameters param = new Parameters();
        //float updatedPanicValue = this.panicValue / MAX_THRESHOLD;
        param.AddParameter<float>("currPanicValue", this.panicValue);
        param.AddParameter<float>("maxPanicValue", MAX_THRESHOLD);
        EventBroadcaster.Instance.PostEvent(EventNames.ON_PANIC_MODIFIED, param);

        this.player.EvaluatePanicState();
    }

    private void SkillCheckResult(Parameters param = null)
    {
        bool skillCheck = param.GetParameter<bool>(ParameterNames.SKILLCHECK_RESULT, false); // you got the value here

        if (!skillCheck)
        {
            IncreasePanicValue(3.5f);
        }
    }
}