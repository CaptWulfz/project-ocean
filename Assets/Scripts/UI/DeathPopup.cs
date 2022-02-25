using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class DeathPopup : Popup
{
    public void OnButtonClickRestart()
    {
        this.Hide();
        SceneManager.LoadScene("StartupScene", LoadSceneMode.Single);
        // Show Main Menu
    }
}
