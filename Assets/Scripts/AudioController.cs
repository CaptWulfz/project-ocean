using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    private const float BREATHING_VOLUME = 0.03f;

    private AudioSource audioSource;
    private string sourceName;

    public void Initialize(AudioSource source, string sourceName)
    {
        this.audioSource = source;
        this.sourceName = sourceName;
        Debug.Log("AUDIO SOURCE: " + this.audioSource);
        Debug.Log("SOURCE NAME: " + this.sourceName);
        AudioManager.Instance.RegisterAudioSource(AudioKeys.SFX, this.sourceName, this.audioSource);
        AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.BREATHING_CALM, BREATHING_VOLUME);
    }

    public void SoundOxygenDeath()
    {
        //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.HEART_BEAT);
        //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.BREATHING);
    }

    public void SoundPanicState(PanicState panicState)
    {
        switch (panicState)
        {
            case PanicState.CALM:
                //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.HEART_BEAT);
                AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.BREATHING_CALM, BREATHING_VOLUME);
                break;
            case PanicState.NORMAL:
                //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.HEART_BEAT);
                AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.BREATHING_CALM, BREATHING_VOLUME);
                break;
            case PanicState.DANGER:
                //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.HEART_BEAT);
                AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.BREATHING_DANGER, BREATHING_VOLUME);
                break;
            case PanicState.DYING:
                //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.HEART_BEAT);
                AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.BREATHING_DYING, BREATHING_VOLUME);
                break;
            case PanicState.DEAD:
                //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.HEART_BEAT);
                //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.BREATHING);
                break;
        }
    }
}
