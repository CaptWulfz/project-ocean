using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class Startup : MonoBehaviour
{
    [SerializeField] SplashScreen splashScreen;

    private void Start()
    {
        DontDestroyOnLoad(this);
        StartCoroutine(Initialize());
    }

    /// <summary>
    /// Initializes the different Game Managers and Assets needed before the game is properly displayed
    /// </summary>
    /// <returns></returns>
    private IEnumerator Initialize()
    {
        //DataManager.Instance.Initialize();
        //yield return new WaitUntil(() => { return DataManager.Instance.IsDone; });
        //this.splashScreen.SetLoadingProgress(0.25f);

        AudioManager.Instance.Initialize();
        yield return new WaitUntil(() => { return AudioManager.Instance.IsDone; });
        this.splashScreen.SetLoadingProgress(0.33f);

        PopupManager.Instance.Initialize();
        yield return new WaitUntil(() => { return PopupManager.Instance.IsDone; });
        this.splashScreen.SetLoadingProgress(0.66f);

        GameManager.Instance.Initialize();
        yield return new WaitUntil(() => { return GameManager.Instance.IsDone; });
        this.splashScreen.SetLoadingProgress(1.0f);

        yield return new WaitForSeconds(2f);

        this.splashScreen.Hide();
        GameManager.Instance.ToggleMainHud(true);
        GameManager.Instance.PlayMainTheme();
    }
}
