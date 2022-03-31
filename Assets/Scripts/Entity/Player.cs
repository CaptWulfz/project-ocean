using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity
{
    public bool isFirstFakeMinePassed = false;

    #region STATES
    public enum SpeedStates{
        MIN,MID,MAX
    }
    public enum DirectionStates{ //COMPASS
        N, S, E, W, NE, NW, SE, SW
    }
    private SpeedStates currentSpeedState;
    private DirectionStates currentLookState;
    private DirectionStates currentMoveState;

    #endregion

    [Header("Player Values")]
    [SerializeField] Panic panic;
    [SerializeField] Oxygen oxygen;

    [Header("Controllers")]
    [SerializeField] AudioController audioController;
    [SerializeField] PlayerAnimatorController animController;

    [Header("Player Settings")]
    [SerializeField] private SpriteRenderer playerSprite;
    [SerializeField] private GameObject visionCone;
    [SerializeField] private GameObject smartWatchHud;

    [SerializeField] private CapsuleCollider2D playerCollider;
    [SerializeField] private float smoothInputSpeed = 0.1f;         //SmoothDamp value for movement
    private Vector2 currentInputVector;
    private Vector2 smoothInputVelocity;
    
    [Header("Player Speed")]
    [SerializeField] private float minSpeed = 1.5f;
    [SerializeField] private float midSpeed = 2f;
    [SerializeField] private float maxSpeed = 3.35f;
    private float accelSpeed = 0.3f;
    private float currentSpeed = 0f;
    private float speedMultiplier;
    private bool goMin;
    private bool goMid;
    private bool goMax;
    private bool playerIsFloating;
    private float playerExplode = 0;

    [Header("Mouse Settings")]
    [SerializeField] Transform mouseAngle;
    private float mouseAngleZ;

    private Coroutine switchStateDelay = null;

    private Interactable interactableObj;

    #region GETTERS/SETTERS

    public SpeedStates CurrentSpeedState
    {
        get { return this.currentSpeedState; }
    }

    public float PanicValue
    {
        get { return this.panic.PanicValueRelativeToMax; }
    }

    public float OxygenTimer
    {
        get { return this.oxygen.OxygenTimer; }
    }

    public float CurrentSpeed
    {
        get { return this.currentSpeed; }
    }

    public float SpeedMultiplier
    {
        get { return this.speedMultiplier; }
    }

    public bool PlayerIsFloating
    {
        get { return this.playerIsFloating; }
    }

    public float PlayerExplode
    {
        get { return this.playerExplode; }
        set {this.playerExplode = value;}
    }
    public bool VisionCone
    {
        set {this.visionCone.SetActive(value);}
    }
    #endregion

    #region MACHINE RUNTIME
    private void Start()
    {
        Initialize();
    }

    protected override void Initialize()
    {
        base.Initialize();
        this.EntityControls.Player.Enable();
        this.currentSpeed = 0f;
        this.minSpeed = 1.5f;
        this.midSpeed = 2f;
        this.maxSpeed = 3.35f;
        this.currentSpeedState = SpeedStates.MIN; //Init default value
        this.panic.Initialize();
        this.oxygen.Initialize();
        this.animController.InitializeAnimator();

        // Audio
        this.sourceName = string.Format("Entity@{0}", this.GetInstanceID());
        this.audioController.Initialize(this.audioSource , this.sourceName);
  //      this.heartBeatScript = GameObject.FindGameObjectWithTag("HeartHUD").GetComponent<HeartBeat>();
    }

    private void Update()
    {
        //if (Keyboard.current.pKey.wasReleasedThisFrame)
        //{
        //    Debug.Log("Increased Panic by 10");
        //    this.panic.IncreasePanicValue(10f); // stimuli (collision)
        //}
        //if (Keyboard.current.oKey.wasReleasedThisFrame)
        //{
        //    Debug.Log("Decreased Oxygen by 3.5"); // Bump into something
        //    this.oxygen.DecreaseOxygenTimer(3.5f);
        //}
        //if (Keyboard.current.lKey.wasReleasedThisFrame)
        //{
        //    Debug.Log("Decreased Panic by 10 + Good points");
        //    this.panic.DecreasePanicValue(10f); // Panic reduced when looking at source of sounds
        //}
        EvaluatePanicState();
        HandleInteractables();
        this.animController.UpdateAnimator();
    }

    private void FixedUpdate()
    {
        MovePlayerWASD();
        SwitchLookState();
        SwitchLookAnimation();
        AdjustPlayerCollider();
        currentSpeed = rigidBody.velocity.magnitude;   //Records the current speed of the Player
        GameDirector.Instance.TrackPlayerSpeedState(this.currentSpeedState);
    }
    #endregion

    #region Interactables
    private void HandleInteractables()
    {
        if (this.interactableObj == null)
            return;

        if (Keyboard.current.eKey.wasPressedThisFrame)
        {
            this.interactableObj.TriggerSkillCheck();
        }
    }
    #endregion

    //--------
    #region Movement
    private void MovePlayerWASD()
    {
        //SmoothDamp
        Vector2 input = this.EntityControls.Player.Movement.ReadValue<Vector2>();
        currentInputVector = Vector2.SmoothDamp(currentInputVector, input, ref smoothInputVelocity, smoothInputSpeed);
        Vector2 move = new Vector2(currentInputVector.x, currentInputVector.y);

        //Detects direction of move
        SwitchMoveState(input); 
        speedMultiplier = minSpeed;

        //Speed States
        if(currentSpeedState == SpeedStates.MIN)
        {
            speedMultiplier = minSpeed;

        }
        else if(currentSpeedState == SpeedStates.MID)
        {
            speedMultiplier = midSpeed;
        }
        else if(currentSpeedState == SpeedStates.MAX)
        {
            speedMultiplier = maxSpeed;
        }

        if(input == Vector2.zero)   //if there is NO INPUT - Sets the SpeedState to MIN 
        {
            if(switchStateDelay != null)
            {
                StopCoroutine(switchStateDelay);
                switchStateDelay = null;
            }
            if(currentSpeed <= minSpeed)
                {
                    currentSpeedState = SpeedStates.MIN;
                }
            else if(currentSpeed <= midSpeed && currentSpeed > minSpeed)
                {
                    currentSpeedState = SpeedStates.MID;
                }
        }
        if(input != Vector2.zero)   //If there IS INPUT
        {
            if(switchStateDelay == null)
            {
                switchStateDelay = StartCoroutine(SwitchSpeedState());
            }            
        }
        
        //Move the player
        //this.MovePosition(move * maxSpeed);
        this.MovePosition(move * speedMultiplier);
    }
    #endregion
    //--------
    #region State Switch
    IEnumerator SwitchSpeedState(){

        yield return new WaitForSeconds(accelSpeed);
            if(currentSpeedState == SpeedStates.MIN)
            {
                if(goMid)
                    currentSpeedState = SpeedStates.MID;
            }
            else if(currentSpeedState == SpeedStates.MID)
            {
                if (goMax)
                    currentSpeedState = SpeedStates.MAX;
                else if (goMin && !goMid)
                    currentSpeedState = SpeedStates.MIN;
            }
            else if(currentSpeedState == SpeedStates.MAX)
            {
                if(goMid && !goMax)
                    currentSpeedState = SpeedStates.MID;
                else if(goMin && !goMax)
                    currentSpeedState = SpeedStates.MIN;
            }

        switchStateDelay = null;
    }
    
    private void SwitchLookState(){

        mouseAngleZ = mouseAngle.localRotation.eulerAngles.z;
        //N, W, S, E    60deg each
        if ((mouseAngleZ >= 45.0f) && (mouseAngleZ <= 135.0f))              //NORTH
        {
            currentLookState = DirectionStates.N;
            SetMinSpeedToCeiling();
        }
        else if ((mouseAngleZ >= 150.0f) && (mouseAngleZ <= 210.0f))        //WEST
        {
            currentLookState = DirectionStates.W;
            if (currentMoveState == DirectionStates.E)
                SetMinSpeedToCeiling();
            else if (currentMoveState == DirectionStates.N || currentMoveState == DirectionStates.S)
                SetMinSpeedToCeiling();//Mid
            else
                SetMaxSpeedToCeiling();
        }
        else if ((mouseAngleZ >= 225.0f) && (mouseAngleZ <= 315.0f))        //SOUTH
        {
            currentLookState = DirectionStates.S;

            //playerIsFloating = true;

            SetMinSpeedToCeiling();
        }
        else if (((mouseAngleZ >= 330.0f) && (mouseAngleZ <= 360.0f))       // EAST
                || ((mouseAngleZ >= 0.0f) && (mouseAngleZ <= 30.0f)))
        {
            currentLookState = DirectionStates.E;

            if (currentMoveState == DirectionStates.W)
                SetMinSpeedToCeiling();
            else if (currentMoveState == DirectionStates.N || currentMoveState == DirectionStates.S)
                SetMinSpeedToCeiling();//Mid
            else
                SetMaxSpeedToCeiling();
        }
        //NW, SW, SE, NE
        else if ((mouseAngleZ > 135.0) && (mouseAngleZ < 150.0f))           //NORTH WEST
        {
            currentLookState = DirectionStates.NW;

            if (currentMoveState == DirectionStates.E || currentMoveState == DirectionStates.S) 
                SetMinSpeedToCeiling();
            else
                SetMaxSpeedToCeiling();//Mid
        }
        else if ((mouseAngleZ > 210.0) && (mouseAngleZ < 225.0f))           //SOUTH WEST
        {
            currentLookState = DirectionStates.SW;

            if (currentMoveState == DirectionStates.E|| currentMoveState == DirectionStates.N)
                SetMinSpeedToCeiling();
            else
                SetMaxSpeedToCeiling();//Mid
        }
        else if ((mouseAngleZ > 315.0) && (mouseAngleZ < 330.0f))           //SOUTH EAST
        {
            currentLookState = DirectionStates.SE;

            if (currentMoveState == DirectionStates.W || currentMoveState == DirectionStates.N)
                SetMinSpeedToCeiling();
            else
                SetMaxSpeedToCeiling();//Mid
        }
        else if ((mouseAngleZ > 30.0) && (mouseAngleZ < 45.0f))             //NORTH EAST
        {
            currentLookState = DirectionStates.NE;

            if (currentMoveState == DirectionStates.W || currentMoveState == DirectionStates.S)
                SetMinSpeedToCeiling();
            else
                SetMaxSpeedToCeiling();//Mid
        }
        //Debug.Log("QQQ CURRENT LOOK STATE: " + currentLookState);
        //GameDirector.Instance.TrackPlayerLookState(currentLookState);
        if((goMin && !goMid && !goMax) || currentSpeed <= minSpeed)
            playerIsFloating = true;
        else
            playerIsFloating = false;
    }

    private void SwitchMoveState(Vector2 input){

        //N W S E
        if(input == Vector2.up)                                         //NORTH
        {
            currentMoveState = DirectionStates.N;
        }
        else if (input == Vector2.left)                                 //WEST
        {
            currentMoveState = DirectionStates.W;
        }
        else if (input == Vector2.down)                                 //SOUTH
        {
            currentMoveState = DirectionStates.S;
        }
        else if (input == Vector2.right)                                //EAST
        {
            currentMoveState = DirectionStates.E;
        }
    }
    #endregion

    #region State Listeners


    public void SwitchLookAnimation(){
        if(currentLookState == DirectionStates.N || currentLookState == DirectionStates.S) //Flips player
        {
            if((mouseAngleZ > 90.0f && mouseAngleZ < 120.0f)||(mouseAngleZ > 240.0f && mouseAngleZ < 270.0f))
            {
                //flip sprite to the left
                playerSprite.flipX = true;
            }
            else if((mouseAngleZ < 90.0f && mouseAngleZ > 60.0f)||(mouseAngleZ < 300.0f && mouseAngleZ > 270.0f))
            {
                //flip sprite to the right
                
                playerSprite.flipX = false;
            }
        }
    }

    public void AdjustPlayerCollider()
    {
        if (!playerIsFloating && (currentSpeed > minSpeed))   //Swim Animation
        {
            playerCollider.direction = CapsuleDirection2D.Horizontal;
            playerCollider.size = new Vector2(2.3f, 0.63f);             //x 2.3, y 0.63

            if (playerSprite.flipX == false)                      //Looking Right x -0.9, y 0.1
                playerCollider.offset = new Vector2(-0.9f, -0.1f);
            else if (playerSprite.flipX == true)                 //Looking Left  
                playerCollider.offset = new Vector2(0.9f, -0.1f);
        }
        else if (playerIsFloating || currentSpeed <= minSpeed)                   //Idle/Floating Animation
        {
            playerCollider.direction = CapsuleDirection2D.Vertical;
            playerCollider.size = new Vector2(1f, 2f);                  //x 1, y 2
            if (playerSprite.flipX == false)
                playerCollider.offset = new Vector2(-0.18f, -0.88f);        //x -0.18, y-0.88
            else if (playerSprite.flipX == true)
                playerCollider.offset = new Vector2(0.18f, -0.88f);
        }
    }

    public void EvaluatePanicState()
    {
        if (!this.panic.SwitchingPanicState)
            return;

        switch (this.panic.PanicState)
        {
            case PanicState.CALM:
                OnPanicStateCalm();
                break;
            case PanicState.NORMAL:
                OnPanicStateNormal();
                break;
            case PanicState.DANGER:
                OnPanicStateDanger();
                break;
            case PanicState.DYING:
                OnPanicStateDying();
                break;
            case PanicState.DEAD:
                OnPanicStateDead();
                break;
        }
        this.audioController.SoundPanicState(this.panic.PanicState);
    }
    #endregion

    #region Panic State Evaluators
    private void OnPanicStateCalm()
    {
        this.oxygen.SetOxygenDecreaseMultiplier(0.5f);
    }
    private void OnPanicStateNormal()
    {
        this.oxygen.SetOxygenDecreaseMultiplier(0.75f);
    }
    private void OnPanicStateDanger()
    {
        this.oxygen.SetOxygenDecreaseMultiplier(1f);
    }
    private void OnPanicStateDying()
    {
        this.oxygen.SetOxygenDecreaseMultiplier(1.25f);
    }

    #endregion

    #region Oxygen
    public void AddOxygen(float value)
    {
        this.oxygen.AddOxygen(value);
    }
    #endregion

    #region Deaths
    // POSTEVENT = POST VIDEO
    // ADD OBSERVER = NOTIF TO THE VIDEO
    // GET PARAM = Get specific parameter
    private void OnPanicStateDead()
    {
        // Add Panic Death Animation here
        this.EntityControls.Player.Movement.Disable();
        this.visionCone.SetActive(false);
        smartWatchHud = GameObject.FindGameObjectWithTag("SmartWatch");
        smartWatchHud.SetActive(false);
        StartCoroutine(this.animController.WaitForAnimationToFinish("Player_Death_Panic", () =>
        {
            DeathPopup popup = PopupManager.Instance.ShowPopup<DeathPopup>(PopupNames.DEATH_POPUP);
            this.gameObject.GetComponent<AudioSource>().volume = 0;
            popup.Setup();
            popup.Show();
        }));
    }

    public void OnOxygenStageDead()
    {
        this.audioController.SoundOxygenDeath();
        this.EntityControls.Player.Movement.Disable();
        this.visionCone.SetActive(false);
        smartWatchHud = GameObject.FindGameObjectWithTag("SmartWatch");
        smartWatchHud.SetActive(false);
        StartCoroutine(this.animController.WaitForAnimationToFinish("Player_Death_Oxygen", () =>
        {
            Debug.Log("NO MORE OXYGEN death popup: ");
            this.gameObject.GetComponent<AudioSource>().volume = 0;
            
            DeathPopup popup = PopupManager.Instance.ShowPopup<DeathPopup>(PopupNames.DEATH_POPUP);
            popup.Setup();
            popup.Show();
        }));
    }

    public void OnMineExplosionDead()
    {
        this.EntityControls.Player.Movement.Disable();
        this.gameObject.GetComponent<AudioSource>().volume = 0;
        smartWatchHud = GameObject.FindGameObjectWithTag("SmartWatch");
        smartWatchHud.SetActive(false);
        playerExplode = 1;
        StartCoroutine(this.animController.WaitForAnimationToFinish("Player_Death_Explode", () =>
        {
            Debug.Log("Kaboom, animation dead");
            DeathPopup popup = PopupManager.Instance.ShowPopup<DeathPopup>(PopupNames.DEATH_POPUP);
            popup.Setup();
            popup.Show();
        }));


        
    }
    #endregion

    #region Listeners
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        string tag = collision.gameObject.tag;
        if (tag == TagNames.DAMAGE)
        {
            Damage damage = collision.GetComponent<Damage>();
            this.panic.ApplyPanicPressure(damage.PanicInfliction);
        }

        if (tag == TagNames.INTERACTABLE)
        {
            this.interactableObj = collision.GetComponent<Interactable>();
        }
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == TagNames.DAMAGE)
        {
            Debug.Log("Exit");
            Damage damage = collision.GetComponent<Damage>();
            this.panic.RemovePanicPressure(damage.PanicInfliction);
        }

        if (collision.tag == TagNames.INTERACTABLE)
        {
            this.interactableObj = null;
        }
    }

    protected override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == TagNames.HOSTILE)
        {
            Debug.Log("Collide with Damage");
            SoundSource source = collision.gameObject.GetComponent<SoundSource>();
            this.panic.IncreasePanicValue(source.InflictedPanicValue * 3);
        }
    }
    #endregion

    #region Bool Manipulation

    public void SetMinSpeedToCeiling()
    {
        goMin = true;
        goMid = false;
        goMax = false;
    }
    public void SetMidSpeedToCeiling()
    {
        goMin = true;
        goMid = true;
        goMax = false;
    }
    public void SetMaxSpeedToCeiling()
    {
        goMin = true;
        goMid = true;
        goMax = true;
    }

    #endregion
}
