using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundModel.asset", menuName = "Database/Sound Model")]
public class SoundModel : ScriptableObject
{
    [field: Header("General Info")]
    [field: SerializeField]
    public string Name { get; set; }

    [field: SerializeField]
    public AudioType AudioType { get; set; }

    [field: SerializeField]
    public AudioClip[] AudioClip { get; set; }

    [field: Header("Fade Options")]
    [field: SerializeField]
    public FadeType FadeType { get; set; }

    [field: SerializeField]
    public float FarRange { get; set; }

    [field: SerializeField]
    public float MidRange { get; set; }

    [field: SerializeField]
    public float CloseRange { get; set; }

    [field: SerializeField]
    [field: Range(0, 1)]
    public float StartOpacityValue { get; set; }

    [field: SerializeField]
    [field: Range(0, 1)]
    public float MidOpacityValue { get; set; }

    [field: SerializeField]
    [field: Range(0, 1)]
    public float EndOpacityValue { get; set; }

    [field: Header("Entity Values")]
    [field: SerializeField]
    public float BaseSpeed { get; set; }

    [field: SerializeField]
    public float PlayerSpeedOffset { get; set; }

    [field: SerializeField]
    public float InflictedPanicValue { get; set; }

    [field: SerializeField]
    public float InflictedOxygenValue { get; set; }

    [field: SerializeField]
    public float DelayBeforeSpawn { get; set; }

    [field: Header("Audio Source Settings")]
    [field: SerializeField]
    public bool Loop { get; set; }

    [field: SerializeField]
    [field: Range(0, 1)]
    public float MaxVolume { get; set; }

    [field: SerializeField]
    [field: Range(-3, 3)]
    public float Pitch { get; set; }

    [field: SerializeField]
    [field: Range(0, 100)]
    public float MinDistance { get; set; }

    [field: SerializeField]
    [field: Range(0, 100)]
    public float MaxDistance { get; set; }
}

public enum AudioType
{
    SINGLE,
    MULTIPLE
}

public enum FadeType
{
    SLOW_FADE,
    BLINK
}
