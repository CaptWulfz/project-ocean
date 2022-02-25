using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MouseLook : MonoBehaviour
{

    [Header("Mouse Settings")]  
    [SerializeField] private float mouseLookSpeed = 0.013f; //Speed of Player look towards Mouse Pointer

    private void Update(){
        LookAtMouse();
    }

    private void LookAtMouse()
    {
        Vector3 direction = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, mouseLookSpeed);
    }
}
