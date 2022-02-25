using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class AudioManager : Singleton<AudioManager>
{
    // File path of the Audio Map Asset File
    private const string AUDIO_MAP_PATH = "AssetFiles/AudioMap";

    [SerializeField] AudioMap audioMap;

    private float globalMusicVolume = 1f;
    private float globalSfxVolume = 1f;

    private Dictionary<string, AudioSource> sfxSources;
    private Dictionary<string, AudioSource> musicSources;

    private Dictionary<string, AudioClip> sfx;
    private Dictionary<string, AudioClip> music;

    private bool isMute = false;
    public bool IsMute
    {
        get { return this.isMute; }
    }

    private bool isDone = false;
    public bool IsDone
    {
        get { return this.isDone; }
    }

    #region Initialization
    public void Initialize()
    {
        this.audioMap = Resources.Load<AudioMap>(AUDIO_MAP_PATH);

        if (sfxSources == null)
            sfxSources = new Dictionary<string, AudioSource>();

        if (musicSources == null)
            musicSources = new Dictionary<string, AudioSource>();

        if (sfx == null)
            sfx = new Dictionary<string, AudioClip>();

        if (music == null)
            music = new Dictionary<string, AudioClip>();

        StartCoroutine(WaitForAudioMap());
    }

    private IEnumerator WaitForAudioMap()
    {
        yield return new WaitUntil(() => { return this.audioMap != null; });

        sfx.Clear();
        foreach (AudioMap.AudioEntry entry in this.audioMap.SFX)
        {
            this.sfx.Add(entry.key, entry.source);
        }

        music.Clear();
        foreach (AudioMap.AudioEntry entry in this.audioMap.Music)
        {
            this.music.Add(entry.key, entry.source);
        }

        this.isDone = true;
    }
    #endregion

    #region Functions
    /// <summary>
    /// Registers a specific Audio Source to an Audio Group given their respective keys
    /// </summary>
    /// <param name="audioGroupKey">Name of the Audio Group</param>
    /// <param name="sourceKey">Name of the Audio Source. Must be nique</param>
    /// <param name="source">The Audio Source File to be registered to an Audio Group</param>
    public void RegisterAudioSource(string audioGroupKey, string sourceKey, AudioSource source)
    {
        Dictionary<string, AudioSource> audioGroup = GetAudioGroup(audioGroupKey);

        if (audioGroup.ContainsKey(sourceKey))
            audioGroup[sourceKey] = source;
        else
            audioGroup.Add(sourceKey, source);
    }

    /// <summary>
    /// Unregisters a specific Audio Source from an Audio Group given their respective keys
    /// </summary>
    /// <param name="audioGroupKey">Name of the Audio Group</param>
    /// <param name="sourceKey">Name of the Audio Source. Must be unique</param>
    public void UnregisterAudioSource(string audioGroupKey, string sourceKey)
    {
        Dictionary<string, AudioSource> audioGroup = GetAudioGroup(audioGroupKey);

        if (audioGroup.ContainsKey(sourceKey))
            audioGroup.Remove(sourceKey);
    }

    /// <summary>
    /// Plays an Audio File on a specific Audio Source from an Audio Group given their respective keys
    /// </summary>
    /// <param name="audioGroupKey">Name of the Audio Group</param>
    /// <param name="sourceKey">Name of the Audio Source</param>
    /// <param name="clipName">Name of the Audio Clip to be played</param>
    public void PlayAudio(string audioGroupKey, string sourceKey, string clipName)
    {
        AudioSource source = GetAudioSource(audioGroupKey, sourceKey);
        Dictionary<string, AudioClip> dict = GetAudioDict(audioGroupKey);
        float volume = GetAudioGroupGlobalVolume(audioGroupKey);

        source.clip = dict[clipName];
        source.volume = volume;
        source.Play();
    }

    public void PlayAudioWithClip(string audioGroupKey, string sourceKey, AudioClip clip, float localMaxVolume = 1)
    {
        AudioSource source = GetAudioSource(audioGroupKey, sourceKey);
        
        float globalVolume = GetAudioGroupGlobalVolume(audioGroupKey);
        float volume = localMaxVolume * globalVolume;

        source.clip = clip;
        source.volume = volume;
        source.Play();
    }

    public void SetGlobalMute(bool mute)
    {
        this.isMute = mute;

        float musicVolume = GetAudioGroupGlobalVolume(AudioKeys.MUSIC);
        float sfxVolume = GetAudioGroupGlobalVolume(AudioKeys.SFX);

        SetAudioGroupVolume(AudioKeys.MUSIC, musicVolume, true);
        SetAudioGroupVolume(AudioKeys.SFX, sfxVolume, true);
    }

    /// <summary>
    /// Sets the Volume of a Certain Audio Group based on the given key
    /// </summary>
    /// <param name="audioGroupKey">Name of the Audio Group</param>
    /// <param name="value">New volume value to be set. From 0.0 to 1.0</param>
    /// <param name="fromMute">If this function is called from a Mute Toggle. Default: false</param>
    public void SetAudioGroupVolume(string audioGroupKey, float value, bool fromMute = false)
    {
        Dictionary<string, AudioSource> audioGroup = GetAudioGroup(audioGroupKey);
        if (!fromMute)
            SetAudioGroupGlobalVolume(audioGroupKey, value);

        foreach (KeyValuePair<string, AudioSource> kvp in audioGroup)
        {
            kvp.Value.volume = value;
        }
    }


    /// <summary>
    /// Gets the Volume of a Specifc Audio Group based on the given key. Non-helper version.
    /// </summary>
    /// <param name="audioGroupKey">Name of the Audio Group</param>
    /// <param name="forUI">If the Value to be retrieved is for UI purposes</param>
    /// <returns></returns>
    public float GetAudioGroupVolume(string audioGroupKey, bool forUI = false)
    {
        return GetAudioGroupGlobalVolume(audioGroupKey, forUI);
    }

    /// <summary>
    /// Sets the Volume of a specific Audio Source from an Audio Group given their respective keys
    /// </summary>
    /// <param name="audioGroupKey">Name of the Audio Group</param>
    /// <param name="sourceKey">Name of the Audio Source</param>
    /// <param name="value">New Volume value to be set. From 0.0 to 1.0</param>
    public void SetAudioSourceVolume(string audioGroupKey, string sourceKey, float value)
    {
        AudioSource source = GetAudioSource(audioGroupKey, sourceKey);
        source.volume = value;
    }

    /// <summary>
    /// Sets the loop state of an Audio Group based on the given key
    /// </summary>
    /// <param name="audioGroupKey">Name of the Audio Group</param>
    /// <param name="value">New Loop state value to be set. true or false</param>
    public void ToggleAudioGroupLoop(string audioGroupKey, bool value)
    {
        Dictionary<string, AudioSource> audioGroup = GetAudioGroup(audioGroupKey);

        foreach (KeyValuePair<string, AudioSource> kvp in audioGroup)
        {
            kvp.Value.loop = value;
        }
    }

    /// <summary>
    /// Sets the loop state of a specific Audio Source from an Audio Group given their respective keys
    /// </summary>
    /// <param name="audioGroupKey">Name of the Audio Group</param>
    /// <param name="sourceKey">Name of the Audio Source</param>
    /// <param name="value">New Loop state value to be set. true or false</param>
    public void ToggleAudioSourceLoop(string audioGroupKey, string sourceKey, bool value)
    {
        AudioSource source = GetAudioSource(audioGroupKey, sourceKey);
        source.loop = value;
    }
    #endregion

    #region Helpers
    /// <summary>
    /// Gets an Audio Group based on the given key
    /// </summary>
    /// <param name="audioGroupKey">Name of the Audio Group</param>
    /// <returns></returns>
    private Dictionary<string, AudioSource> GetAudioGroup(string audioGroupKey)
    {
        Dictionary<string, AudioSource> audioGroup = null;

        switch (audioGroupKey)
        {
            case AudioKeys.MUSIC:
                audioGroup = this.musicSources;
                break;
            case AudioKeys.SFX:
                audioGroup = this.sfxSources;
                break;
        }

        return audioGroup;
    }

    /// <summary>
    /// Gets a specific Audio Source from an Audio Group given their respective keys
    /// </summary>
    /// <param name="audioGroupKey">Name of the Audio Group</param>
    /// <param name="sourceKey">Name of the Audio Source</param>
    /// <returns></returns>
    private AudioSource GetAudioSource(string audioGroupKey, string sourceKey)
    {
        AudioSource source = null;

        Dictionary<string, AudioSource> audioGroup = GetAudioGroup(audioGroupKey);

        if (audioGroup.ContainsKey(sourceKey))
            source = audioGroup[sourceKey];

        return source;
    }

    /// <summary>
    /// Gets the specific Audio Clip Group based on the given key
    /// </summary>
    /// <param name="audioClipKey">Name of the Audio Clip Group</param>
    /// <returns></returns>
    private Dictionary<string, AudioClip> GetAudioDict(string audioClipKey)
    {
        Dictionary<string, AudioClip> dict = null;

        switch (audioClipKey)
        {
            case AudioKeys.SFX:
                dict = this.sfx;
                break;
            case AudioKeys.MUSIC:
                dict = this.music;
                break;
        }

        return dict;
    }

    /// <summary>
    /// Gets the Global Audio Group Volume
    /// </summary>
    /// <param name="audioGroupKey">Name of the Audio Group</param>
    /// <param name="forUI">If the value to be retrieved is for UI purposes</param>
    /// <returns></returns>
    private float GetAudioGroupGlobalVolume(string audioGroupKey, bool forUI = false)
    {
        if (this.isMute && !forUI)
            return 0f;

        float volume = 1f;

        switch (audioGroupKey)
        {
            case AudioKeys.SFX:
                volume = this.globalSfxVolume;
                break;
            case AudioKeys.MUSIC:
                volume = this.globalMusicVolume;
                break;
        }

        return volume;
    }

    /// <summary>
    /// Sets a new volume value of an Audio Group based on the given key
    /// </summary>
    /// <param name="audioGroupKey">Name of the Audio Group</param>
    /// <param name="volume">New volume value to be set</param>
    private void SetAudioGroupGlobalVolume(string audioGroupKey, float volume)
    {
        switch (audioGroupKey)
        {
            case AudioKeys.SFX:
                this.globalSfxVolume = volume;
                break;
            case AudioKeys.MUSIC:
                this.globalMusicVolume = volume;
                break;
        }
    }
    #endregion
}

public class AudioKeys
{
    public const string SFX = "SFX";
    public const string MUSIC = "MUSIC";
}

public class SFXKeys
{
    public const string TOM = "tom";
    public const string SKILLCHECK_SUCCESS = "Skillcheck_Success";
    public const string SKILLCHECK_FAIL = "Skillcheck_Fail";
}

public class MusicKeys
{
   
}
