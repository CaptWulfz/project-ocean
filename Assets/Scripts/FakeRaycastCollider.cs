using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FakeRaycastCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collider){
        if(collider.tag == "DirectionTrigger")
        {
            //Debug.Log(collider.name);
            switch (collider.name)
            {
                case "RightLookTrigger":
                    //Character faces right
                    //Disable opposite direction
                break;

                case "LeftLookTrigger":
                    //Character faces left
                    //Disable opposite direction
                break;
            }
        }
        else
        {
            //Debug.Log(collider.name);
        }
    }
}
