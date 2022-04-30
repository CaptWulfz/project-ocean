using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCheckpoint : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == TagNames.PLAYER)
            DataManager.Instance.SaveCheckPoint(this.gameObject.transform);
    }
}
