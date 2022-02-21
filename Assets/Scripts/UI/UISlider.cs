using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISlider : MonoBehaviour
{
    [Header("Slider Components")]
    [SerializeField] Slider slider;
    [SerializeField] Image sliderFill;

    private bool isSliderEnabled = false;

    public float Value
    {
        get { return this.slider.value; }
        set { this.slider.value = value; }
    }

    public void ToggleActiveState(bool active)
    {
        this.isSliderEnabled = active;
        this.slider.interactable = active;

        Color fillColor = this.sliderFill.color;
        float alphaValue = active ? 1f : 0f;
        this.sliderFill.color = new Color(fillColor.r, fillColor.g, fillColor.b, alphaValue);
    }


}
