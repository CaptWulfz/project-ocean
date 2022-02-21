using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prototype_Obstacle : MonoBehaviour
{
    [SerializeField] Prototype_PlayerMovement player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.IncreasePlayerPanic(0.1f);
        }
    }
}
