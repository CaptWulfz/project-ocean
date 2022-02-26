using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MineAnimation : AnimatorController
{
    private Mine mine;

    public override void InitializeAnimator()
    {
        this.mine = GetComponent<Mine>();
    }

    public override void UpdateAnimator()
    {
        EvaluateAnimation("mineExplode", this.mine.MineExplode);
    }
}
