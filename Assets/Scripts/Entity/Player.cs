using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{

    [SerializeField] Panic panic;
    [SerializeField] Oxygen oxygen;

    // Heart Monitor
    // Oxygen

    private void onEnable()
    {
        //AudioManager.Instance.RegisterAudioSource(AudioKeys.SFX, this.sourceName, this.source);
        //AudioManager.Instance.RegisterAudioSource(AudioKeys.MUSIC, this.sourceName, this.source);
    }

    // onDisabled uncheck in Object
    private void onDisable()
    {
        //AudioManager.Instance.UnregisterAudioSource(AudioKeys.SFX, this.sourceName);
        //AudioManager.Instance.UnregisterAudioSource(AudioKeys.MUSIC, this.sourceName);
    }

    private void Start()
    {
        Initialize();
    }

    private void Awake()
    {
        EventBroadcaster.Instance.AddObserver(EventNames.ON_PLAYER_DIED_PANIC, OnPanicStateDead);
        EventBroadcaster.Instance.AddObserver(EventNames.ON_PLAYER_DIED_OXYGEN, OnOxygenStageDead);
    }

    protected override void Initialize()
    {
        base.Initialize();
        this.Speed = 5f;
        this.EntityControls.Player.Enable();
        this.panic.Initialize();
        this.oxygen.Initialize();
    }

    private void Update()
    {
        MonitorPanicState();
        if (Keyboard.current.pKey.wasReleasedThisFrame)
        {
            Debug.Log("Increased Panic by 10");
            this.panic.IncreasePanicValue(10f); // stimuli (collision)
        }
        if (Keyboard.current.oKey.wasReleasedThisFrame)
        {
            Debug.Log("Decreased Oxygen by 3.5"); // Bump into something
            this.oxygen.DecreaseOxygenTimer(3.5f);
        }
        if (Keyboard.current.lKey.wasReleasedThisFrame)
        {
            Debug.Log("Decreased Panic by 10 + Good points");
            this.panic.DecreasePanicValue(10f); // Panic reduced when looking at source of sounds
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MovePlayer()
    {
        Vector2 move = this.EntityControls.Player.Movement.ReadValue<Vector2>();
        this.MovePosition(move * this.Speed);
    }

    #region State Listeners
    private void MonitorPanicState()
    {
        switch (this.panic.PanicState)
        {
            case PanicState.CALM:
                OnPanicStateCalm();
                break;
            case PanicState.NORMAL:
                OnPanicStateNormal();
                break;
            case PanicState.DANGER:
                OnPanicStateDanger();
                break;
            case PanicState.DYING:
                OnPanicStateDying();
                break;
            case PanicState.MAX:
                OnPanicStateDead();
                break;
        }
    }
    #endregion

    #region Panic State Evaluators
    private void OnPanicStateCalm()
    {
        this.oxygen.SetOxygenDecreaseMultiplier(0.5f);
    }
    private void OnPanicStateNormal()
    {
        //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.HEART_BEAT);
        //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.BREATHING);
        this.oxygen.SetOxygenDecreaseMultiplier(0.75f);
    }
    private void OnPanicStateDanger()
    {
        //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.HEART_BEAT);
        //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.BREATHING);
        this.oxygen.SetOxygenDecreaseMultiplier(1f);
    }
    private void OnPanicStateDying()
    {
        //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.HEART_BEAT);
        //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.BREATHING);
        this.oxygen.SetOxygenDecreaseMultiplier(1.25f);
    }
    private void OnPanicStateDead(Parameters param = null)
    {
        if (param != null)
        {
            // Add Panic Death Animation here
            param.GetParameter<float>("deathPanic", 1f);
            this.EntityControls.Player.Movement.Disable();
            Debug.Log("Character is Scared to Death");
        }
        //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.PANIC_DEATH);
    }
    #endregion

    #region Oxygen Death
    // POSTEVENT = POST VIDEO
    // ADD OBSERVER = NOTIF TO THE VIDEO
    // GET PARAM = Get specific parameter
    private void OnOxygenStageDead(Parameters param = null)
    {
        if (param != null)
        {
            // Add Oxygen Death Animation here
            param.GetParameter<float>("deathOxygen", 0f);
            this.EntityControls.Player.Movement.Disable();
            Debug.Log("No more Oxygen, Character is Dead");
        }
        //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.PANIC_OXYGEN);
    }
    #endregion
}
