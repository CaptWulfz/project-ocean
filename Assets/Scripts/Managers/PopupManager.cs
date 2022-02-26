using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupManager : Singleton<PopupManager>
{
    [SerializeField] Canvas popupCanvas;

    private const string POPUPS_PATH = "Prefabs/UI/{0}";

    private bool isDone = false;
    public bool IsDone
    {
        get { return this.isDone; }
    }

    #region Initialization
    public void Initialize()
    {
        this.popupCanvas = GameObject.FindGameObjectWithTag(TagNames.POPUP_CANVAS).GetComponent<Canvas>();
        StartCoroutine(WaitForPopupCanvas());
    }

    public IEnumerator WaitForPopupCanvas()
    {
        yield return new WaitUntil(() => { return this.popupCanvas != null; });
        this.isDone = true;
    }
    #endregion

    /// <summary>
    /// Loads and instantiates a clone of the Popup from the Resources Folder given a name
    /// </summary>
    /// <typeparam name="T">Class Component of the Popup to be returned</typeparam>
    /// <param name="popupName">Name of the Popup</param>
    /// <returns></returns>
    public T ShowPopup<T>(string popupName)
    {
        string path = string.Format(POPUPS_PATH, popupName);
        GameObject popup = Resources.Load<GameObject>(path);
        popup.SetActive(true);
        GameObject deploy = GameObject.Instantiate(popup, this.popupCanvas.transform);

        return deploy.GetComponent<T>();
    }

    /// <summary>
    /// Destroys the Popup game object
    /// </summary>
    /// <param name="popup">The Popup game object</param>
    public void HidePopup(GameObject popup)
    {
        Destroy(popup);
    }
}

public class PopupNames
{
    public const string MENU_POPUP = "MenuPopup";
    public const string DEATH_POPUP = "DeathPopup";

}
