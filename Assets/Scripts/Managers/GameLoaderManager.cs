using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLoaderManager : Singleton<GameLoaderManager>
{
    private const string MAIN_HUD_PATH = "Prefabs/UI/MainHud";

    private const string MAIN_SOURCE = "MAIN_SOURCE";

    private Canvas mainCanvas;
    private CameraFollow mainCamera;

    private MainHud mainHud;

    private bool isMainHudLoaded = false;

    private bool isDone = false;
    public bool IsDone
    {
        get { return this.isDone; }
    }

    #region Initialization
    public void Initialize()
    {
        this.mainCanvas = GameObject.FindGameObjectWithTag(TagNames.HUD_CANVAS).GetComponent<Canvas>();
        this.mainCamera = GameObject.FindGameObjectWithTag(TagNames.MAIN_CAMERA).GetComponent<CameraFollow>();
        StartCoroutine(WaitForInitialization());
    }

    private IEnumerator WaitForInitialization()
    {
        LoadMainHud();

        yield return new WaitUntil(() => { return this.isMainHudLoaded; });

        this.isDone = true;
    }

    public void ReloadGame()
    {
        SceneManager.LoadScene(SceneNames.STARTUP_SCENE, LoadSceneMode.Single);
        Destroy(GameObject.Find("Root"));
    }

    public void LoadGameScene()
    {
        StartCoroutine(LoadFirstScene());
    }

    private IEnumerator LoadFirstScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneNames.OCEAN_SCENE, LoadSceneMode.Single);

        yield return new WaitUntil(() => { return asyncLoad.isDone; });

        ToggleMainHud(true);
        MoveObjectToScene(this.mainCamera.gameObject, SceneManager.GetActiveScene());
        this.mainCamera.GetComponent<AudioListener>().enabled = false;
        this.mainCamera.GetComponent<CameraAudioSource>().PlayMusic(MusicKeys.AMBIANCE);
        this.mainCamera.FindPlayer();
    }

    private void LoadMainHud()
    {
        GameObject mainHud = Resources.Load<GameObject>(MAIN_HUD_PATH);
        GameObject hud = Instantiate(mainHud, mainCanvas.transform);
        hud.SetActive(false);
        this.mainHud = hud.GetComponent<MainHud>();
        this.mainHud.Initialize();
        this.isMainHudLoaded = true;
    }
    #endregion

    /// <summary>
    /// Plays the Main Theme of the Game
    /// </summary>
    public void PlayMainTheme()
    {
        AudioManager.Instance.PlayAudio(AudioKeys.MUSIC, MAIN_SOURCE, MusicKeys.MAIN_THEME);
        AudioManager.Instance.SetAudioGroupVolume(AudioKeys.MUSIC, 1f);
        AudioManager.Instance.ToggleAudioGroupLoop(AudioKeys.MUSIC, true);
    }

    private void MoveObjectToScene(GameObject obj, Scene scene)
    {
        obj.transform.SetParent(null);
        SceneManager.MoveGameObjectToScene(obj, scene);
    }

    /// <summary>
    /// Toggles the Main Hud on or off
    /// </summary>
    /// <param name="active">Main Hud active value to be set. true or false</param>
    public void ToggleMainHud(bool active)
    {
        this.mainHud.gameObject.SetActive(active);
    }
}
