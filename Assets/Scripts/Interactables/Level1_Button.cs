using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1_Button : MonoBehaviour
{
    bool isPlayerInteracting = false;

    void Update()
    {
        if (InputManager.Instance.GetControls().Player.Interact.WasPressedThisFrame() && isPlayerInteracting)
            Debug.Log("Level 1: Player Pressed the Button");
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == TagNames.PLAYER)
        {
            isPlayerInteracting = true;
            Debug.Log("Player Entered");
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == TagNames.PLAYER)
        {
            isPlayerInteracting = false;
            Debug.Log("Player Exited");
        }
    }
}
