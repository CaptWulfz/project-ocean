using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DeathPopup : Popup
{
    private Player player;
    public void Setup()
    {
        this.popupOpen = "PopupFadeOpen_Pixel";
        this.popupClose = "PopupFadeClose_Pixel";
        this.onClose = () =>
        {
            player.Start();
            //GameLoaderManager.Instance.ReloadGame();
        };
    }


    public void OnButtonClickRestart()
    {
        this.Hide();
    }
}
