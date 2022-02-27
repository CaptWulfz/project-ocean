using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OceanEvents : MonoBehaviour
{
    [SerializeField] GameObject landingZone;
    [SerializeField] GameObject abyssDropLanding;
    [SerializeField] GameObject landingColliderGate;
    [SerializeField] GameObject endingZone;
    [SerializeField] GameObject endingColliderGate;
    [SerializeField] GameObject abyssDropEnding;

    private void Start()
    {
        this.endingZone.SetActive(false);
        this.abyssDropEnding.SetActive(false);
        this.endingColliderGate.SetActive(true);
        this.landingColliderGate.SetActive(false);
        EventBroadcaster.Instance.AddObserver(EventNames.HIDE_LANDING, OnHideLanding);
        EventBroadcaster.Instance.AddObserver(EventNames.ON_ALL_RELICS_COLLECTED, OnRevealEnding);
    }

    private void OnHideLanding(Parameters param = null)
    {
        this.landingZone.SetActive(false);
        this.abyssDropLanding.SetActive(false);
        this.landingColliderGate.SetActive(true);
    }

    private void OnRevealEnding(Parameters parmam = null)
    {
        this.endingZone.SetActive(true);
        this.abyssDropEnding.SetActive(true);
        this.endingColliderGate.SetActive(false);
    }
}
