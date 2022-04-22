using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerOutage : MonoBehaviour
{
    [SerializeField] private List<GameObject> lighting;
    [SerializeField] private GameObject flashlight;
    [SerializeField] private GameObject doorSwitchRoom;
    //[SerializeField] private bool powerOutage;
    
    private void OnEnable() {
        EventBroadcaster.Instance.AddObserver(EventNames.ON_POWER_OUTAGE, OnPowerOutage);
    }
    private void OnDisable() {
        EventBroadcaster.Instance.RemoveObserver(EventNames.ON_POWER_OUTAGE);
    }
    void Start()
    {
        //Temp
        flashlight = GameObject.Find("VisionCone");
        //powerOutage = false;    //Implement for EventBroadcasting
    }
    private void OnPowerOutage(Parameters param = null)
    { //Cue power outage events

    Debug.Log("Power Outage");
      //Audio cue
            //Darken room / increase opacity of darkness
            foreach(GameObject light in lighting)
            {
                light.GetComponent<SpriteRenderer>().color = new Color(0f,0f,0f,.5f);
            }
            //Turn on flashlight
            flashlight.gameObject.GetComponent<SpriteMask>().enabled = true;
            flashlight.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            //powerOutage = true;

            //Open switch room door 
            doorSwitchRoom.SetActive(false);      
        
        
    }
    
}
