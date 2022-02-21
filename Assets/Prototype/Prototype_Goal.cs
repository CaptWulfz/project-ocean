using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prototype_Goal : MonoBehaviour
{
    [SerializeField] Prototype_PlayerMovement player;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            player.UpdatePlayerPrompt("Player: You Won!");
            player.UpdatePlayerOxygen(1.0f);
        }
    }
}
