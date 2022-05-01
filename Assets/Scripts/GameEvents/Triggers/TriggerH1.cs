using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerH1 : MonoBehaviour
{
    /*cant focus atm but you need to do is know when dialog is done (it should fire)

    */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == TagNames.PLAYER /*&& OnH1DialogEnd*/)
        {
            EventBroadcaster.Instance.PostEvent(EventNames.ON_H1_DIALOG_END);
        }
    }
}
