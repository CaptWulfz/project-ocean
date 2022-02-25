using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainHud : MonoBehaviour
{
    [SerializeField] SkillCheck skillCheck;

    public void Initialize()
    {
        this.skillCheck.gameObject.SetActive(false);
        GameDirector.Instance.RegisterSkillCheck(this.skillCheck);
    }
}
