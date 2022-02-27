using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimatorController : AnimatorController
{
    private Player player;

    public override void InitializeAnimator()
    {
        this.player = GetComponent<Player>();
    }

    public override void UpdateAnimator()
    {
        EvaluateAnimation("playerSpeed", this.player.CurrentSpeed);
        EvaluateAnimation("playerIsFloating", this.player.PlayerIsFloating);
        EvaluateAnimation("Oxygen", this.player.OxygenTimer);
        EvaluateAnimation("Panic", this.player.PanicValue);
        EvaluateAnimation("playerExplode", this.player.PlayerExplode);
    }
}
