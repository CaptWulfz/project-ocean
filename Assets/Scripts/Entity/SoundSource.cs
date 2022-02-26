using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SoundSource : Entity
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] SoundSourceRange soundRange;
    [SerializeField] SoundModel soundModel;
    public SoundModel SoundModel
    {
        get { return this.soundModel; }
    }

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

    public void Setup()
    {
        this.sourceName = string.Format("Sound-Source@{0}", this.GetInstanceID());
        this.target = GameObject.FindGameObjectWithTag(TagNames.PLAYER).GetComponent<Player>();
        this.Speed = this.soundModel.BaseSpeed;
        InitializeInflictionValues();
        InitializeAudioSource();
        AudioManager.Instance.RegisterAudioSource(AudioKeys.SFX, sourceName, this.audioSource);
        AudioClip clipToPlay = GetAudioClip();
        AudioManager.Instance.PlayAudioWithClip(AudioKeys.SFX, this.sourceName, clipToPlay, this.soundModel.MaxVolume);
    }

    private AudioClip GetAudioClip()
    {
        AudioClip clip = this.soundModel.AudioClip[0];

        if (this.soundModel.AudioType == AudioType.MULTIPLE)
        {
            int index = Random.Range(0, this.soundModel.AudioClip.Length);
            clip = this.soundModel.AudioClip[index];
        }

        Debug.Log("QQQ Loaded Clip " + clip.name);
        return clip;
    }

    public void Despawn()
    {
        AudioManager.Instance.UnregisterAudioSource(AudioKeys.SFX, sourceName);
        Destroy(this.gameObject);
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
        float dir = (this.target.transform.position.x - this.transform.position.x);
        this.spriteRenderer.flipX = (dir > 0f) ? true : false;

        if (Vector2.Distance(this.transform.position, this.target.transform.position) > DISTANCE_FROM_TARGET)
        {
            this.FollowTarget(this.target.transform, this.Speed);
        }
        if (this.target.CurrentSpeedState != Player.SpeedStates.MIN)
        {
            this.Speed = this.target.CurrentSpeed + this.soundModel.PlayerSpeedOffset;
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
            Despawn();
        }
    }
}
