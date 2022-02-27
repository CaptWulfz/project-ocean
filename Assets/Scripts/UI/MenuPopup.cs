using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MenuPopup : Popup
{
    [Header("Popup Components")]
    [SerializeField] GameObject mainPanel;
    [SerializeField] GameObject creditsPanel;
    [SerializeField] GameObject prologuePanel;
    [SerializeField] GameObject tutorialPanel;
    [Header("Prologue Popups")]
    private int prologuePage = 1;
    [SerializeField] GameObject prologuePage1;
    [SerializeField] GameObject prologuePage2;
    [SerializeField] GameObject prologuePage3;
    [Header("Tutorial Popups")]
    private int tutorialPage = 1;
    [SerializeField] GameObject tutorialPage1;
    [SerializeField] GameObject tutorialPage2;

    public void Setup()
    {
        this.popupOpen = "PopupFadeOpen_Pixel";
        this.popupClose = "PopupFadeClose_Pixel";
        this.onClose = () =>
        {
            GameLoaderManager.Instance.LoadGameScene(); //Loads the Game Scene after closing
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
        this.prologuePanel.SetActive(true);
        this.mainPanel.SetActive(false);

            //Put this after prologue & tutorial
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

    public void OnPrologueButtonClicked()
    {
        //make switch
        Debug.Log("dumbass");
        switch (prologuePage)
        {
            case 1:
                Debug.Log("Page 2");
                this.prologuePage1.SetActive(false);
                this.prologuePage2.SetActive(true);
                prologuePage++;
            break;

            case 2:
                Debug.Log("Page 3");
                this.prologuePage2.SetActive(false);
                this.prologuePage3.SetActive(true);
                prologuePage++;
            break;

            case 3:
                Debug.Log("Next");
                this.prologuePanel.SetActive(false);
                this.tutorialPanel.SetActive(true);
            break;
        }
        
    }

    public void OnTutorialButtonClicked()
    {
        
        Debug.Log("tutorial dumbass");
        switch (tutorialPage)
        {
            case 1:
                Debug.Log("Page 2");
                this.tutorialPage1.SetActive(false);
                this.tutorialPage2.SetActive(true);
                tutorialPage++;
            break;
            case 2:
                Debug.Log("Hide");
                this.tutorialPanel.SetActive(false);
                this.Hide();
            break;
        }
    }
#endregion
}
