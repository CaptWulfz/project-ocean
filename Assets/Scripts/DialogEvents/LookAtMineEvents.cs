using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtMineEvents : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == TagNames.PLAYER)
        {
            GameDirector.Instance.StartDialogSequence(TopicList.INTRO);
        }
    }
}
