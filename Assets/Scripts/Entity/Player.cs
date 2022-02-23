using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{
    [SerializeField] Panic panic;
    [SerializeField] Oxygen oxygen;

    private void OnEnable()
    {
        //AudioManager.Instance.RegisterAudioSource(AudioKeys.SFX, this.sourceName, this.source);
        //AudioManager.Instance.RegisterAudioSource(AudioKeys.MUSIC, this.sourceName, this.source);
    }

    // onDisabled uncheck in Object
    private void OnDisable()
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

    #region Receivers

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

    #region Listeners

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == TagNames.DAMAGE)
        {
            Debug.Log("Enter");
            Damage damage = collision.GetComponent<Damage>();
            this.panic.ApplyPanicPressure(damage.PanicInfliction);
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == TagNames.DAMAGE)
        {
            Debug.Log("Exit");
            Damage damage = collision.GetComponent<Damage>();
            this.panic.RemovePanicPressure(damage.PanicInfliction);
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == TagNames.HOSTILE)
        {
            Debug.Log("Collide with Damage");
            SoundSource source = collision.gameObject.GetComponent<SoundSource>();
            this.panic.IncreasePanicValue(source.InflictedPanicValue * 3);
        }
    }
    #endregion
}
