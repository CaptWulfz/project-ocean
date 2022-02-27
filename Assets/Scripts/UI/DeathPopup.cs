using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DeathPopup : Popup
{
    public void Setup()
    {
        this.popupOpen = "PopupFadeOpen_Pixel";
        this.popupClose = "PopupFadeClose_Pixel";
        this.onClose = () =>
        {
            GameLoaderManager.Instance.ReloadGame();
        };
    }


    public void OnButtonClickRestart()
    {
        this.Hide();
    }
}
