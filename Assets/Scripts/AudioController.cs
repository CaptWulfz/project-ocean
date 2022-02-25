using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private AudioSource audioSource;
    private string sourceName;

    public void Initialize(AudioSource source, string sourceName)
    {
        this.audioSource = source;
        this.sourceName = sourceName;
        Debug.Log("AUDIO SOURCE: " + this.audioSource);
        Debug.Log("SOURCE NAME: " + this.sourceName);
        AudioManager.Instance.RegisterAudioSource(AudioKeys.SFX, this.sourceName, this.audioSource);
        AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.BREATHING_CALM);

    }

    public void SoundOxygenDeath()
    {
        //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.HEART_BEAT);
        //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.BREATHING);
    }

    public void SoundPanicState(PanicState panicState)
    {
        Debug.Log("SOUND PANIC STATE: " + panicState);
        Debug.Log("SOUND SOURCE NAME: " + this.sourceName);
        switch (panicState)
        {
            case PanicState.CALM:
                //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.HEART_BEAT);
                AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.BREATHING_CALM);
                break;
            case PanicState.NORMAL:
                //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.HEART_BEAT);
                AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.BREATHING_CALM);
                break;
            case PanicState.DANGER:
                //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.HEART_BEAT);
                AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.BREATHING_DANGER);
                break;
            case PanicState.DYING:
                //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.HEART_BEAT);
                AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.BREATHING_DYING);
                break;
            case PanicState.DEAD:
                //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.HEART_BEAT);
                //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.BREATHING);
                break;
        }
    }
}
