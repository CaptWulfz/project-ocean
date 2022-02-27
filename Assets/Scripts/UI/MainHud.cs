using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHud : MonoBehaviour
{
    [SerializeField] SkillCheck skillCheck;
    [SerializeField] EventDialogManager dialogManager;
    public void Initialize()
    {
        this.skillCheck.gameObject.SetActive(false);
        this.dialogManager.gameObject.SetActive(false);
        GameDirector.Instance.RegisterSkillCheck(this.skillCheck);
        GameDirector.Instance.RegisterEventDialogManager(this.dialogManager);
    }
}
