using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName= "EventDialog.asset", menuName = "Event/Dialog")]
public class EventDialog : ScriptableObject
{
    [field: Header("Dialog Text")]
    [field: SerializeField]
    public string EventDialogText { get; set; }

    [field: Header("Player Responses")]
    [field: SerializeField]
    public EventDialog[] EventDialogPlayerResponses { get; set; }

    [field: Header("Dialog Properties")]
    [field: SerializeField]
    public ConfidenceTypes ConfidenceType { get; set; }

    [field: SerializeField]
    public DialogEffects EventDialogEffect { get; set; }

    [field: SerializeField]
    public DoubtEffects DoubtEffect { get; set; }

    [field: SerializeField]
    public float EventDialogPanicDamage { get; set; }
}

public enum ConfidenceTypes
{
    CONFIDENT,
    UNSURE,
    NONE
}

public enum DialogEffects
{
    PANIC,
    DOUBT,
    NONE
}

public enum DoubtEffects
{
    SOMETHING,
    NONE
}
