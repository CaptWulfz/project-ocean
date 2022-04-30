using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class GameDirectorMain
{
    private SmartWatch smartWatch;


    public void RegisterSmartWatch(SmartWatch check)
    {
        this.smartWatch = check;
    }

    public void ShowSmartWatch()
    {
        this.smartWatch.ShowSmartWatch();
    }
    public void HideSmartWatch()
    {
        this.smartWatch.HideSmartWatch();

    }

}
