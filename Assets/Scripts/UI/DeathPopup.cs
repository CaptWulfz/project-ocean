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
            SceneManager.LoadScene("StartupScene", LoadSceneMode.Single);
        };
    }

    public void OnButtonClickRestart()
    {
        this.Hide();
    }
}
