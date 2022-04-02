using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameDirectorMain
{
    public EventDialogManager eventDialogManager;
    private bool isDialogInSequence = false;
    public void RegisterDialogManager(EventDialogManager eventDialogManager)
    {
        this.eventDialogManager = eventDialogManager;
    }

    public void StartDialogSequence(EventDialog eventDialog)
    {
        if (eventDialog == null && isDialogInSequence)
            return;
        else
        {
            isDialogInSequence = true;
            eventDialogManager.GenerateDialogSequence(eventDialog);
        }
    }
}
