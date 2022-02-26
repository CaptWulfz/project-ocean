using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DirectorEntitiesMap.asset", menuName = "Database/Director Entities Map")]
public class DirectorEntitiesMap : ScriptableObject
{
    //[field: SerializeField]
    //public SoundSource SoundSourceReference { get; set; }

    //[field: SerializeField]
    //public SoundModel[] SoundModels { get; set; }
    [field: SerializeField]
    public SoundSource[] Entities { get; set; }
}
