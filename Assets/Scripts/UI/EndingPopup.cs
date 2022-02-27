using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingPopup : Popup
{
    private void OnEnable()
    {
        StartCoroutine(AnimationHandler.WaitForAnimation(this.anim, () =>
        {
            this.anim.Play("PopupCutscene");
            StartCoroutine(AnimationHandler.WaitForAnimation(this.anim, () => {
                this.Hide();
            }));
        }));
    }

    public void Setup()
    {
        this.popupOpen = "PopupFadeOpen_Pixel";
        this.popupClose = "PopupFadeClose_Pixel";
        this.onClose = () =>
        {
            GameLoaderManager.Instance.ReloadGame();
        };
    }
}
