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
        MonitorPanicState();
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
            case PanicState.NORMAL:
                OnPanicStateNormal();
                break;
            case PanicState.DANGER:
                OnPanicStateDanger();
                break;
            case PanicState.MAX:
                break;
        }
    }
    #endregion

    #region Panic State Evaluators
    private void OnPanicStateNormal()
    {
        this.oxygen.SetOxygenDecreaseMultiplier(1f);
    }
    private void OnPanicStateDanger()
    {
        this.oxygen.SetOxygenDecreaseMultiplier(2f);
    }
    #endregion
}
