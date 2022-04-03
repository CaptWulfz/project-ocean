using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameDirectorMain
{
    bool isDialogInSequence = false;

    public Dictionary<EventDialog, bool> eventDialogs;
    public EventDialogManager eventDialogManager;

    public void InitializeDialogList(EventDialog[] eventDialogs)
    {
        this.eventDialogs = new Dictionary<EventDialog, bool>();
        foreach (EventDialog dialog in eventDialogs)
            this.eventDialogs.Add(dialog, false);
    }

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
