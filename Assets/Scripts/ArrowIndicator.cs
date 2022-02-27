using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowIndicator : MonoBehaviour
{
    [SerializeField] GameObject[] arrowType;

    [SerializeField] Transform playerTransform;
    [SerializeField] List<GameObject> relics;
    [SerializeField] List<GameObject> frequencies;

    [SerializeField] Transform relicShrine;

    private bool trackRelicShrine = false;
    private bool isDone = false;
    Vector2 nearestRelic;

    private void Start()
    {
        nearestRelic = Vector2.zero;
        foreach (GameObject freq in frequencies)
            freq.SetActive(false);

        this.arrowType[0].SetActive(true);
        this.isDone = false;
        EventBroadcaster.Instance.AddObserver(EventNames.ON_RELIC_PICK_UP, OnRelicPickup);
        EventBroadcaster.Instance.AddObserver(EventNames.ON_ALL_RELICS_COLLECTED, OnAllRelicsCollected);
        EventBroadcaster.Instance.AddObserver(EventNames.RELIC_SHRINE_FINISHED, OnRelicShrineFinished);
    }

    private void OnDestroy()
    {
        EventBroadcaster.Instance.RemoveObserverAtAction(EventNames.ON_RELIC_PICK_UP, OnRelicPickup);
    }

    void Update()
    {
        if (isDone)
            return;

        if (this.trackRelicShrine)
        {
            TrackRelicShrine();
            return;
        }

        if (relics.Count > 0)
        {
            RotateToNearestRelic();
            UpdateSignalStrength();
        }
        else
        {
            foreach (GameObject freq in frequencies)
                freq.SetActive(false);
        }
    }

    private void TrackRelicShrine()
    {
        float znewVal = Mathf.Atan2(this.relicShrine.position.y - playerTransform.position.y, this.relicShrine.position.x - playerTransform.position.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.Euler(this.transform.rotation.x, this.transform.rotation.y, znewVal);
    }

    private void OnAllRelicsCollected(Parameters param = null)
    {
        this.trackRelicShrine = true;
        this.arrowType[0].SetActive(false);
        this.arrowType[1].SetActive(true);
    }

    private void OnRelicShrineFinished(Parameters param = null)
    {
        this.isDone = true;
        this.arrowType[0].SetActive(false);
        this.arrowType[1].SetActive(false);
    }

    void OnRelicPickup(Parameters param = null)
    {
        int temp = param.GetParameter<int>("relicID", 0);
        GameObject relic = this.relics.Find((x) => { return x.GetInstanceID() == temp; });
        
        if (relic != null)
            this.relics.Remove(relic);
    }

    void UpdateSignalStrength()
    {
        //if (ComputeDistanceToNearestRelic() > 80)
        //{
        //    foreach (GameObject freq in frequencies)
        //        freq.SetActive(false);
        //}
        if (ComputeDistanceToNearestRelic() > 30)
        {
            frequencies[0].SetActive(true);
            frequencies[1].SetActive(false);
            frequencies[2].SetActive(false);
        }
        else if (ComputeDistanceToNearestRelic() > 10 && ComputeDistanceToNearestRelic() < 30)
        {
            frequencies[0].SetActive(true);
            frequencies[1].SetActive(true);
            frequencies[2].SetActive(false);
        }
        else if (ComputeDistanceToNearestRelic() > 0 && ComputeDistanceToNearestRelic() < 10)
        {
            foreach (GameObject freq in frequencies)
                freq.SetActive(true);
        }
    }

    void RotateToNearestRelic()
    {
        float lowestDistance = -1f;
        float tempDist = 0f;
        foreach (GameObject relic in relics)
        {
            if (!relic.activeSelf)
                continue;

            tempDist = Vector2.Distance(playerTransform.position, relic.transform.position);
            if (lowestDistance > tempDist || lowestDistance == -1f)
            {
                nearestRelic = relic.transform.position;
                lowestDistance = tempDist;
            }
        }

        float znewVal = Mathf.Atan2(nearestRelic.y - playerTransform.position.y, nearestRelic.x - playerTransform.position.x) * Mathf.Rad2Deg;
        this.transform.rotation = Quaternion.Euler(this.transform.rotation.x, this.transform.rotation.y, znewVal);
    }

    float ComputeDistanceToNearestRelic()
    {
        return Vector2.Distance(playerTransform.transform.position, nearestRelic);
    }
}
