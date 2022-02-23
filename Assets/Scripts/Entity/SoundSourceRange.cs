using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundSourceRange : Damage
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == TagNames.PLAYER)
        {

        }
    }
}
