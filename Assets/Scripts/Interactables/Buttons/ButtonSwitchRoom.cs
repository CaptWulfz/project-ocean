using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonSwitchRoom : MonoBehaviour
{
    bool isPlayerInteracting = false;

    void Update()
    {
        if (InputManager.Instance.GetControls().Player.Interact.WasPressedThisFrame() && isPlayerInteracting)
            EventBroadcaster.Instance.PostEvent(ButtonNames.L1_SWITCH_ROOM);
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
