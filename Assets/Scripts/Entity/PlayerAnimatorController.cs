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
        //EvaluateAnimation("Speed");
    }
}
