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

    protected const string POPUP_OPEN = "PopupOpen";
    protected const string POPUP_CLOSE = "PopupClose";

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
        this.anim.Play(POPUP_CLOSE);
        StartCoroutine(AnimationHandler.WaitForAnimation(this.anim, () =>
        {
            PopupManager.Instance.HidePopup(this.gameObject);
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
