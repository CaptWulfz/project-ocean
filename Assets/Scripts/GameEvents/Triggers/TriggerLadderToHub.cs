using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerLadderToHub : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == TagNames.PLAYER)
        {
            EventBroadcaster.Instance.PostEvent(EventNames.ON_LADDER_TO_HUB_ENTRY);
        }
    }
}
