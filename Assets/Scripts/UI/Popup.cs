using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Popup : MonoBehaviour
{
    [Header("Popup Animation")]
    [SerializeField] Animation anim;

    /// <summary>
    /// Overlay object in the popup. Serves to make sure that anything behind the Popup is not clickable
    /// </summary>
    [Header("Overlay")]
    [SerializeField] GameObject overlay;

    protected string popupOpen = "PopupOpen";
    protected string PopupOpen
    {
        set { this.popupOpen = value; }
    }

    protected string popupClose = "PopupClose";
    protected string PopupClose
    {
        set { this.popupClose = value; }
    }

    protected Action onClose;

    /// <summary>
    /// Shows the Popup. Enables both the game object and the Overlay.
    /// </summary>
    public virtual void Show()
    {
        this.overlay.SetActive(true);
        this.gameObject.SetActive(true);
    }

    /// <summary>
    /// Hides the Popup. Plays the Hide Animation first before destroying the Popup
    /// </summary>
    protected virtual void Hide()
    {
        this.anim.Play(this.popupClose);
        StartCoroutine(AnimationHandler.WaitForAnimation(this.anim, () =>
        {
            PopupManager.Instance.HidePopup(this.gameObject);
            this.onClose();
        }));
    }

    /// <summary>
    /// Function to be called when the user interacts with the Close Button
    /// </summary>
    public virtual void OnCloseButton()
    {
        Hide();
    }
}
