using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingEvent : MonoBehaviour
{
    bool hasEnteredOnce = false;
    bool hasExitedOnce = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == TagNames.PLAYER)
        {
            hasEnteredOnce = true;
            GameDirector.Instance.StartDialogSequence(TopicList.END_ENTER_ALTAR);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == TagNames.PLAYER)
        {
            hasExitedOnce = true;
            GameDirector.Instance.StartDialogSequence(TopicList.END_EXIT_ALTAR);
        }
    }
}
