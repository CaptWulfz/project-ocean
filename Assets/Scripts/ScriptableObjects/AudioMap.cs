using UnityEngine;

[CreateAssetMenu(fileName = "AudioMap.asset", menuName = "Database/AudioMap")]
public class AudioMap : ScriptableObject
{
    [System.Serializable]
    public class AudioEntry
    {
        public string key;
        public AudioClip source;
    }

    public AudioEntry[] SFX;
    public AudioEntry[] Music;
}
