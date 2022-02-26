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
        StartCoroutine(LoadFirstScene());
    }

    private IEnumerator LoadFirstScene()
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneNames.OCEAN_SCENE, LoadSceneMode.Single);

        LoadMainHud();

        yield return new WaitUntil(() => { return asyncLoad.isDone && this.isMainHudLoaded; });

        MoveObjectToScene(this.mainCamera.gameObject, SceneManager.GetActiveScene());
        this.mainCamera.FindPlayer();

        this.isDone = true;
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
