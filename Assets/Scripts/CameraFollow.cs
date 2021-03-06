using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

    [SerializeField] [Range(0.01f, 1f)]

    private float smoothCameraSpeed = 0.01f;

    [SerializeField] private Vector3 offset;

    private Vector3 velocity = Vector3.zero;

    private void Start() {
       
    }

    public void FindPlayer()
    {
        target = GameObject.FindGameObjectWithTag(TagNames.PLAYER).transform;
        CameraAudioSource camSource = this.GetComponent<CameraAudioSource>();
        camSource.PlayMusic(MusicKeys.AMBIANCE);
    }

    private void FixedUpdate() {
        if (target == null)
            return;

        Vector3 targetPosition = target.position + offset;
        transform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, smoothCameraSpeed);
    }
}
