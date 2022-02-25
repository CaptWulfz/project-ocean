using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAudioSource : MonoBehaviour
{
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    private string musicSourceName;
    private string sfxSourceName;

    public void Initialize()
    {
        this.musicSourceName = string.Format("Main-Music-Source@{0}", GetInstanceID());
        this.sfxSourceName = string.Format("Main-SFX-Source@{0}", GetInstanceID());

        AudioManager.Instance.RegisterAudioSource(AudioKeys.MUSIC, this.musicSourceName, this.musicSource);
        AudioManager.Instance.RegisterAudioSource(AudioKeys.SFX, this.sfxSourceName, this.sfxSource);

        EventBroadcaster.Instance.AddObserver(EventNames.ON_SKILL_CHECK_FINISHED, OnSkillCheckFinished);
    }

    #region Event Broacaster
    private void OnSkillCheckFinished(Parameters param = null)
    {
        if (param != null)
        {
            bool success = param.GetParameter<bool>(ParameterNames.SKILLCHECK_RESULT, false);
            string skillCheckClip = success ? SFXKeys.SKILLCHECK_SUCCESS : SFXKeys.SKILLCHECK_FAIL;
            AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sfxSourceName, skillCheckClip);
        }
    }
    #endregion
}
