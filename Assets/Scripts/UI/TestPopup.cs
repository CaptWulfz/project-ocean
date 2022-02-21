using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestPopup : Popup
{
    [Header("Popup Components")]
    [SerializeField] Text frontText;
    [SerializeField] Text confirmButtonLabel;

    #region Overrides
    public override void Show()
    {
        base.Show();
    }

    protected override void Hide()
    {
        base.Hide();
    }
    #endregion

    public void Setup(string frontText, string confirmLabel)
    {
        this.frontText.text = frontText;
        this.confirmButtonLabel.text = confirmLabel;
    }
}
