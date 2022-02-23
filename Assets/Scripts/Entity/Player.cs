using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{
    [SerializeField] Panic panic;
    [SerializeField] Oxygen oxygen;

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
    public void EvaluatePanicState()
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
            case PanicState.DEAD:
                OnPanicStateDead();
                break;
        }
    }
    #endregion

    #region Panic State Evaluators
    private void OnPanicStateCalm()
    {
        //this.heartBeat.HeartBeatSpeed(1f);
        this.oxygen.SetOxygenDecreaseMultiplier(0.5f);
    }
    private void OnPanicStateNormal()
    {
        //this.heartBeat.HeartBeatSpeed(30f);
        //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.HEART_BEAT);
        //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.BREATHING);
        this.oxygen.SetOxygenDecreaseMultiplier(0.75f);
    }
    private void OnPanicStateDanger()
    {
        //this.heartBeat.HeartBeatSpeed(60f);
        //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.HEART_BEAT);
        //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.BREATHING);
        this.oxygen.SetOxygenDecreaseMultiplier(1f);
    }
    private void OnPanicStateDying()
    {
        //this.heartBeat.HeartBeatSpeed(90f);
        //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.HEART_BEAT);
        //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.BREATHING);
        this.oxygen.SetOxygenDecreaseMultiplier(1.25f);
    }
    private void OnPanicStateDead()
    {
        // Add Panic Death Animation here
        this.EntityControls.Player.Movement.Disable();
        Debug.Log("Character is Scared to Death");
    }
    #endregion

    #region Oxygen Death
    // POSTEVENT = POST VIDEO
    // ADD OBSERVER = NOTIF TO THE VIDEO
    // GET PARAM = Get specific parameter
    public void OnOxygenStageDead()
    {
        this.EntityControls.Player.Movement.Disable();
        Debug.Log("No more Oxygen, Character is Dead");
    }
    #endregion
}
