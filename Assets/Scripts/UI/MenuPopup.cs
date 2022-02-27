using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MenuPopup : Popup
{
    [Header("Popup Components")]
    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject creditsPanel;

    public void Setup()
    {
        this.popupOpen = "PopupFadeOpen_Pixel";
        this.popupClose = "PopupFadeClose_Pixel";
        this.onClose = () =>
        {
            GameLoaderManager.Instance.LoadGameScene();
        };
    }

    private void ToggleMainPanel(bool active)
    {
        this.mainPanel.SetActive(active);
        this.creditsPanel.SetActive(!active);
    }

    #region Unity Button Events
    public void OnPlayButtonClicked()
    {
        this.Hide();
    }

    public void OnCreditsButtonClicked()
    {
        ToggleMainPanel(false);
    }

    public void OnQuitButtonClicked()
    {
#if !UNITY_EDITOR
        Application.Quit();
#else
        EditorApplication.isPlaying = false;
#endif
    }

    public void OnCreditsBackButtonClicked()
    {
        ToggleMainPanel(true);
    }
#endregion
}
