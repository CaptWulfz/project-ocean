using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowIndicator : MonoBehaviour
{
    [SerializeField] Transform playerTransform;
    [SerializeField] List<GameObject> relics;
    [SerializeField] List<GameObject> frequencies;

    Vector2 nearestRelic;
    private void Start()
    {
        nearestRelic = Vector2.zero;
        foreach (GameObject freq in frequencies)
            freq.SetActive(false);
    }

    void Update()
    {
        RotateToNearestRelic();
        UpdateSignalStrength();
    }

    void UpdateSignalStrength()
    {
        if (ComputeDistanceToNearestRelic() > 60)
        {
            foreach (GameObject freq in frequencies)
                freq.SetActive(false);
        }
        else if (ComputeDistanceToNearestRelic() > 30 && ComputeDistanceToNearestRelic() < 50)
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
