using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SplashScreen : MonoBehaviour
{
    [SerializeField] Slider loadingBar;
    [SerializeField] Text loadingProgressText;
    
    /// <summary>
    /// Sets the Loading Bar Slider Progress
    /// </summary>
    /// <param name="value">New Value of the Loading Bar Slider. from 0.0 to 1.0</param>
    public void SetLoadingProgress(float value)
    {
        this.loadingBar.value = value;
    }

    /// <summary>
    /// Hides and destroys the Splash Screen
    /// </summary>
    public void Hide()
    {
        this.gameObject.SetActive(false);
        Destroy(this.gameObject);
    }

    #region On Value Changed
    public void OnLoadingValueChanged()
    {
        this.loadingProgressText.text = string.Format("{0}%", this.loadingBar.value * 100);
    }
    #endregion
}
