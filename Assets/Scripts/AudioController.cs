using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{

    public void Initialize()
    {
        // Initialize the AudioController, (sorry idk how or what to put here hehe)
    }

    public void SoundOxygenDeath()
    {
        //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.HEART_BEAT);
        //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.BREATHING);
    }

    public void SoundPanicState(PanicState panicState)
    {
        Debug.Log("SOUND PANIC STATE: " + panicState);
        switch (panicState)
        {
            case PanicState.CALM:
                //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.HEART_BEAT);
                //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.BREATHING);
                break;
            case PanicState.NORMAL:
                //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.HEART_BEAT);
                //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.BREATHING);
                break;
            case PanicState.DANGER:
                //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.HEART_BEAT);
                //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.BREATHING);
                break;
            case PanicState.DYING:
                //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.HEART_BEAT);
                //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.BREATHING);
                break;
            case PanicState.DEAD:
                //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.HEART_BEAT);
                //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.BREATHING);
                break;
        }
    }
}
