using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SoundSource : Entity
{
    [SerializeField] AudioSource audioSource;
    [SerializeField] SoundSourceRange soundRange;

    private SoundModel soundModel;

    private const float DISTANCE_FROM_TARGET = 0f;

    private float inflictedPanicValue;
    public float InflictedPanicValue
    {
        get { return this.inflictedPanicValue; }
    }

    private float inflictedOxygenValue;
    public float InflictedOxygenValue
    {
        get { return this.inflictedOxygenValue; }
    }

    private Player target;

    public void Setup(SoundModel model)
    {
        this.soundModel = model;
        this.sourceName = string.Format("Sound-Source@{0}", this.GetInstanceID());
        this.target = GameObject.FindGameObjectWithTag(TagNames.PLAYER).GetComponent<Player>();
        this.Speed = 3f;
        InitializeInflictionValues();
        InitializeAudioSource();
        AudioManager.Instance.RegisterAudioSource(AudioKeys.SFX, sourceName, this.audioSource);
        AudioManager.Instance.PlayAudioWithClip(AudioKeys.SFX, this.sourceName, this.soundModel.AudioClip, this.soundModel.MaxVolume);
    }

    public void Despawn()
    {
        AudioManager.Instance.UnregisterAudioSource(AudioKeys.SFX, sourceName);
    }

    private void InitializeInflictionValues()
    {
        this.inflictedPanicValue = this.soundModel.InflictedPanicValue;
        this.inflictedOxygenValue = this.soundModel.InflictedOxygenValue;
        this.soundRange.SetupInflictionValues(this.inflictedPanicValue, this.inflictedOxygenValue);
    }

    private void InitializeAudioSource()
    {
        this.audioSource.loop = this.soundModel.Loop;
        this.audioSource.volume = this.soundModel.MaxVolume;
        this.audioSource.pitch = this.soundModel.Pitch;
        this.audioSource.minDistance = this.soundModel.MinDistance;
        this.audioSource.maxDistance = this.soundModel.MaxDistance;
    }

    private void Update()
    {
        if (Vector2.Distance(this.transform.position, this.target.transform.position) > DISTANCE_FROM_TARGET)
        {
            this.FollowTarget(this.target.transform, this.Speed);
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == TagNames.PLAYER)
        {
            Debug.Log("Player Collide");
            Destroy(this.gameObject);
        }
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == TagNames.PLAYER_VISION)
        {
            Debug.Log("I die");
            Destroy(this.gameObject);
        }
    }
}