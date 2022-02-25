using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventDialogBox : MonoBehaviour
{
    GameObject dialogManagerReference;
    EventDialog nextSequenceOfDialog;

    public void SetDialogBoxProperties(GameObject dialogManagerReference, EventDialog nextSequenceOfDialog)
    {
        this.dialogManagerReference = dialogManagerReference;
        this.nextSequenceOfDialog = nextSequenceOfDialog;
    }
    public void OnEventButtonClick()
    {
        dialogManagerReference.GetComponent<EventDialogManager>().GenerateDialogSequence(nextSequenceOfDialog);
    }
}
