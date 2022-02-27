using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeMineEvent : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == TagNames.PLAYER)
        {
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null && player.isFirstFakeMinePassed)
            {
                player.isFirstFakeMinePassed = true;
                GameDirector.Instance.StartDialogSequence(TopicList.ABYSS_PASS_THROUGH_FIRST_MIRAGE_MINE);
            }
        }
    }
}
