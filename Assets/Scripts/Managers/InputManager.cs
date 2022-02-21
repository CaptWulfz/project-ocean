using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    private Controls controls = null;

    private bool isDone = false;
    public bool IsDone
    {
        get { return this.isDone; }
    }

    #region Initialization
    public void Initialize()
    {
        this.controls = new Controls();
        StartCoroutine(WaitForControls());
    }

    public IEnumerator WaitForControls()
    {
        yield return new WaitUntil(() => { return this.controls != null; });

        this.isDone = true;
    }
    #endregion

    public Controls GetControls()
    {
        if (this.controls == null)
            Initialize();

        return this.controls;
    }
}
