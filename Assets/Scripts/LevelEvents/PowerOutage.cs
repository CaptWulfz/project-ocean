using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerOutage : MonoBehaviour
{
    [SerializeField] private GameObject lighting;
    [SerializeField] private GameObject flashlight;
    [SerializeField] private GameObject doorSwitchRoom;
    [SerializeField] private bool powerOutage;
    
    void Start()
    {
        //Temp
        lighting = GameObject.Find("PowerOutage");
        flashlight = GameObject.Find("VisionCone");
        doorSwitchRoom = GameObject.Find("DoorSwitchRoom");
        powerOutage = false;    //Implement for EventBroadcasting
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == TagNames.PLAYER)
        {
            if(powerOutage)
            {

            }
            else //Cue power outage events
            {
                //Audio cue

                //Darken room / increase opacity of darkness
                lighting.gameObject.GetComponent<SpriteRenderer>().color = new Color(0f,0f,0f,.5f);

                //Turn on flashlight
                flashlight.gameObject.GetComponent<SpriteMask>().enabled = true;
                flashlight.gameObject.GetComponent<SpriteRenderer>().enabled = true;
                powerOutage = true;

                //Open switch room door 
                doorSwitchRoom.SetActive(false);
            }
        }
    }
}
