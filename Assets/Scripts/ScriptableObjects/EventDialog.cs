using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName= "EventDialog.asset", menuName = "Event/Dialog")]
public class EventDialog : ScriptableObject
{
    public string EventDialogText; 
    public ConfidenceTypes ConfidenceType;
    public bool WillPromptPlayerResponse;
}

//effects
//player panic
//doubt

public enum ConfidenceTypes
{
    CONFIDENT,
    UNSURE,
    STATIC
}
