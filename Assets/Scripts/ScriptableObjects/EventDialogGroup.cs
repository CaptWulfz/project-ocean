using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EventDialogGroup.asset", menuName = "Event/DialogGroup")]
public class EventDialogGroup : ScriptableObject
{
    [field:SerializeField]
    public string EventDialogGroupName { get; set; }

    [field:SerializeField]
    public EventDialog[] EventDialogs { get; set; }
}
