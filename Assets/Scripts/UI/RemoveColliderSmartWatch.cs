using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveColliderSmartWatch : MonoBehaviour
{
    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == TagNames.PLAYER)
        {
            this.gameObject.SetActive(false);
        }
    }
}
