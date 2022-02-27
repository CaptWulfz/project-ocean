using System;
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

    private const float CLOSE_DISTANCE_FROM_TARGET = 2.5f;
    private const float FAR_DISTANCE_FROM_TARGET = 10f;
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
    private Action onDespawn;
    private Color srColor;

    private bool isFrenzy;

    private bool reachedMidOpacity = false;

    public void Setup(Action onDespawn = null, bool isFrenzy = false)
    {
        this.onDespawn = onDespawn;
        this.isFrenzy = isFrenzy;
        this.sourceName = string.Format("Sound-Source@{0}", this.GetInstanceID());
        this.target = GameObject.FindGameObjectWithTag(TagNames.PLAYER).GetComponent<Player>();
        this.Speed = this.soundModel.BaseSpeed;
        this.srColor = this.spriteRenderer.color;
        this.reachedMidOpacity = false;
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
            int index = UnityEngine.Random.Range(0, this.soundModel.AudioClip.Length);
            clip = this.soundModel.AudioClip[index];
        }

        //Debug.Log("QQQ Loaded Clip " + clip.name);
        return clip;
    }

    public void Despawn()
    {
        AudioManager.Instance.UnregisterAudioSource(AudioKeys.SFX, sourceName);
        this.onDespawn?.Invoke();
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

        float distance = Vector2.Distance(this.transform.position, this.target.transform.position);
        //Debug.Log("Distance: " + distance);

        HandleOpacityBehavior(distance);
        if (distance > this.SoundModel.FarRange)
        {
            this.Speed = this.SoundModel.ChaseSpeed;
        } else if (this.isFrenzy)
        {
            this.Speed = this.soundModel.FrenzySpeed;
        } else
        {
            this.Speed = this.soundModel.BaseSpeed;
        }

        if (this.target.CurrentSpeedState != Player.SpeedStates.MIN)
        {
            this.Speed = this.target.CurrentSpeed + this.soundModel.PlayerSpeedOffset + (this.Speed / 2);
        }

        this.FollowTarget(this.target.transform, this.Speed);
    }

    private void HandleOpacityBehavior(float distance)
    {
        float opacity = 0f;
        if (this.soundModel.FadeType == FadeType.SLOW_FADE)
        {
            if (distance > this.soundModel.FarRange)
                opacity = 0;
            else
            {
                float multiplier = Mathf.Abs(this.soundModel.FarRange - distance);
                if (multiplier > 1)
                {
                    multiplier = 1;
                }
                opacity = this.soundModel.CloseRange / distance * multiplier;
            }

            Color newColor = new Color(this.srColor.r, this.srColor.g, this.srColor.b, opacity);
            this.spriteRenderer.color = newColor;
        } else if (this.soundModel.FadeType == FadeType.BLINK)
        {
            opacity = this.spriteRenderer.color.a;
            if (distance > this.soundModel.FarRange)
            {
                opacity = 0f;
                this.reachedMidOpacity = false;
            } else
            {
                if (distance < this.soundModel.FarRange && distance > this.soundModel.MidRange)
                {
                    if (opacity >= 1)
                        opacity = 0.38f;

                    if (!this.reachedMidOpacity)
                    {
                        opacity += Time.deltaTime * 0.15f;
                        if (opacity >= this.soundModel.MidOpacityValue)
                            this.reachedMidOpacity = true;
                    } else if (this.reachedMidOpacity)
                    {
                        opacity -= Time.deltaTime * 0.15f;
                        if (opacity <= 0)
                            this.reachedMidOpacity = false;
                    }
                } else if (distance < this.soundModel.MidRange)
                {
                    float multiplier = Mathf.Abs(this.soundModel.MidRange - distance);
                    if (multiplier > 1)
                    {
                        multiplier = 1;
                    }
                    opacity += this.soundModel.CloseRange / distance * multiplier;
                    this.reachedMidOpacity = false;
                }
            }

            Color newColor = new Color(this.srColor.r, this.srColor.g, this.srColor.b, opacity);
            this.spriteRenderer.color = newColor;
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == TagNames.PLAYER)
        {
            Despawn();
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
