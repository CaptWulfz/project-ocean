using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadingOverlay : Singleton<LoadingOverlay>
{
    [SerializeField] GameObject overlay;
    [SerializeField] Text loadingText;

    private bool isOverlayActive = false;

    public void ShowOverlay(string loadingText)
    {
        if (!this.isOverlayActive)
        {
            this.overlay.SetActive(true);
            this.loadingText.text = string.Format("{0}...", loadingText);
            this.isOverlayActive = true;
        }
    }

    public void HideOverlay()
    {
        if (this.isOverlayActive)
        {
            this.overlay.SetActive(false);
            this.isOverlayActive = false;
        }
    }
}
