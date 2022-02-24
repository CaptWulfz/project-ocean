using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity //
{    
    public enum SpeedStates{
        MIN,
        MID,
        MAX
    }

    private SpeedStates currentSpeedState;

    public enum LookStates{ //COMPASS
        N, S, E, W, NE, NW, SE, SW
    }

    private LookStates currentLookState;
    private LookStates currentMoveState;
    
    
    [SerializeField] Panic panic;
    [SerializeField] Oxygen oxygen;

    [Header("Player Settings")]
    //[SerializeField] Rigidbody2D playerRigidbody;
    [SerializeField] private float smoothInputSpeed = 0.1f; //SmoothDamp value
    private Vector2 currentInputVector;
    private Vector2 smoothInputVelocity;
    
    [Header("Player Speed")]
    [SerializeField] private float currentSpeed = 0f;
    [SerializeField] private float minSpeed = 3f;
    [SerializeField] private float midSpeed = 4f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float speedMultiplier;
    [SerializeField] private bool goMin;
    [SerializeField] private bool goMid;
    [SerializeField] private bool goMax;

    [Header("Mouse Settings")]
    [SerializeField]Vector2 direction = Vector2.zero;
    [SerializeField]Transform mouseAngle;
    private float mouseAngleZ;

    private float secondsInMaxSpeedState;

    private Coroutine switchStateDelay = null;

    public float SpeedMultiplier
    {
        get { return this.speedMultiplier; }
    }

    #region MACHINE RUNTIME
    private void Start()
    {
        Initialize();
    }

    protected override void Initialize() //Move vars to this class rather than Init -TODO
    {
        base.Initialize();
        this.EntityControls.Player.Enable();
        this.currentSpeed = 0f;
        this.minSpeed = 0.5f;
        this.midSpeed = 1f;
        this.maxSpeed = 2f;
        this.currentSpeedState = SpeedStates.MIN; //Init default value
        this.panic.Initialize();
        this.oxygen.Initialize();
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
    }

    private void FixedUpdate()
    {
        MovePlayerWASD();                   //USES WASD
        SwitchLookState();
        currentSpeed = rigidBody.velocity.magnitude;   //Just records the current speed
        //Debug.Log(currentSpeed);
        GameDirector.Instance.TrackPlayerSpeedState(this.currentSpeedState);
    }
    #endregion
    //--------
    #region Movement
    private void MovePlayerWASD()
    {
        Vector2 input = this.EntityControls.Player.Movement.ReadValue<Vector2>();
        SwitchMoveState(input); //Detects direction of move
        currentInputVector = Vector2.SmoothDamp(currentInputVector, input, ref smoothInputVelocity, smoothInputSpeed);
        Vector2 move = new Vector2(currentInputVector.x, currentInputVector.y);

        speedMultiplier = minSpeed;

        if(currentSpeedState == SpeedStates.MIN)
        {
            secondsInMaxSpeedState = 0f;
            speedMultiplier = minSpeed;
        }
        else if(currentSpeedState == SpeedStates.MID)
        {
            secondsInMaxSpeedState = 0f;
            speedMultiplier = midSpeed;
        }
        else if(currentSpeedState == SpeedStates.MAX)
        {
            secondsInMaxSpeedState += Time.deltaTime;       //helo
            speedMultiplier = maxSpeed;
        }
        if(input == Vector2.zero)   //Sets the SpeedState to MIN if there is no input
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
        if(input != Vector2.zero)
        {
            if(switchStateDelay == null)
            {
                switchStateDelay = StartCoroutine(SwitchSpeedState());
            }
        }
        //Debug.Log("SpeedMultiplier: "+speedMultiplier+" | SpeedState: "+ currentSpeedState);
        this.MovePosition(move * speedMultiplier);
    }
    #endregion
    //--------
    #region State Switch
    IEnumerator SwitchSpeedState(){
        
        //Debug.Log("SSState");
        yield return new WaitForSeconds(1.5f);
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
        if ((mouseAngleZ >= 60.0f) && (mouseAngleZ <= 120.0f))              //NORTH
        {
            currentLookState = LookStates.N;
            goMin = true;
            goMid = false;
            goMax = false;
        }
        else if ((mouseAngleZ >= 150.0f) && (mouseAngleZ <= 210.0f))        //WEST
        {
            currentLookState = LookStates.W;
            if (currentMoveState == LookStates.E || currentMoveState == LookStates.NE || currentMoveState == LookStates.SE || currentMoveState == LookStates.S || currentMoveState == LookStates.N)
            {
                goMin = true;
                goMid = false;
                goMax = false;
            }
            else
            {
                goMin = true;
                goMid = true;
                goMax = true;
            }
        }
        else if ((mouseAngleZ >= 240.0f) && (mouseAngleZ <= 300.0f))        //SOUTH
        {
            currentLookState = LookStates.S;
            direction = Vector2.down;
            goMin = true;
            goMid = false;
            goMax = false;
        }
        else if (((mouseAngleZ >= 0.0f) && (mouseAngleZ <= 30.0f)||(mouseAngleZ >= 330.0f) && (mouseAngleZ <= 360.0f)))  // EAST
        {
            currentLookState = LookStates.E;
            if (currentMoveState == LookStates.W || currentMoveState == LookStates.NW || currentMoveState == LookStates.SW || currentMoveState == LookStates.S || currentMoveState == LookStates.N)
            {
                goMin = true;
                goMid = false;
                goMax = false;
            }
            else
            {
                goMin = true;
                goMid = true;
                goMax = true;
            }
        }
        //NW, SW, SE, NE
        else if ((mouseAngleZ > 120.0) && (mouseAngleZ < 150.0f))           //NORTH WEST
        {
            currentLookState = LookStates.NW;
            if (currentMoveState == LookStates.E || currentMoveState == LookStates.NE || currentMoveState == LookStates.SE )
            {
                goMin = true;
                goMid = false;
                goMax = false;
            }
            else
            {
                goMin = true;
                goMid = true;
                goMax = false;
            }
        }
        else if ((mouseAngleZ > 210.0) && (mouseAngleZ < 240.0f))           //SOUTH WEST
        {
            currentLookState = LookStates.SW;
            if (currentMoveState == LookStates.E || currentMoveState == LookStates.NE || currentMoveState == LookStates.SE )
            {
                goMin = true;
                goMid = false;
                goMax = false;
            }
            else
            {
                goMin = true;
                goMid = true;
                goMax = false;
            }
        }
        else if ((mouseAngleZ > 300.0) && (mouseAngleZ < 330.0f))           //SOUTH EAST
        {
            currentLookState = LookStates.SE;
            if (currentMoveState == LookStates.W || currentMoveState == LookStates.NW || currentMoveState == LookStates.SW || currentMoveState == LookStates.S || currentMoveState == LookStates.N)
            {
                goMin = true;
                goMid = false;
                goMax = false;
            }
            else
            {
                goMin = true;
                goMid = true;
                goMax = false;
            }
        }
        else if ((mouseAngleZ > 30.0) && (mouseAngleZ < 60.0f))             //NORTH EAST
        {
            currentLookState = LookStates.NE;
            if (currentMoveState == LookStates.W || currentMoveState == LookStates.NW || currentMoveState == LookStates.SW || currentMoveState == LookStates.S || currentMoveState == LookStates.N)
            {
                goMin = true;
                goMid = false;
                goMax = false;
            }
            else
            {
                goMin = true;
                goMid = true;
                goMax = false;
            }
        }
        //Debug.Log("QQQ CURRENT LOOK STATE: " + currentLookState);
        GameDirector.Instance.TrackPlayerLookState(this.currentLookState);
    }

    private void SwitchMoveState(Vector2 input){

        direction = input;
        //N W S E
        if(direction == Vector2.up)                                         //NORTH
        {
            currentMoveState = LookStates.N;
        }
        else if (direction == Vector2.left)                                 //WEST
        {
            currentMoveState = LookStates.W;
        }
        else if (direction == Vector2.down)                                 //SOUTH
        {
            currentMoveState = LookStates.S;
        }
        else if (direction == Vector2.right)                                //EAST
        {
            currentMoveState = LookStates.E;
        }
        //NW, SW, SE, NE     DOESNT WORK THO. BUT UNNECESSARY. :)     -Gelo
        else if (direction == new Vector2(-1,1))                            //NORTH WEST
        {
            currentMoveState = LookStates.NW;
        }
        else if (direction == new Vector2(-1,-1))                            //SOUTH WEST
        {
            currentMoveState = LookStates.SW;
        }
        else if (direction == new Vector2(1,-1))                            //SOUTH EAST
        {
            currentMoveState = LookStates.SE;
        }
        else if (direction == new Vector2(1,1))                            //NORTH EAST
        {
            currentMoveState = LookStates.NE;
        }

        //Debug.Log("Moving: "+currentMoveState);
    }
    #endregion

    #region State Listeners
    public void EvaluatePanicState()
    {
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
    }
    #endregion

    #region Panic State Evaluators
    private void OnPanicStateCalm()
    {
        //this.heartBeat.HeartBeatSpeed(1f);
        this.oxygen.SetOxygenDecreaseMultiplier(0.5f);
    }
    private void OnPanicStateNormal()
    {
        //this.heartBeat.HeartBeatSpeed(30f);
        //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.HEART_BEAT);
        //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.BREATHING);
        this.oxygen.SetOxygenDecreaseMultiplier(0.75f);
    }
    private void OnPanicStateDanger()
    {
        //this.heartBeat.HeartBeatSpeed(60f);
        //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.HEART_BEAT);
        //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.BREATHING);
        this.oxygen.SetOxygenDecreaseMultiplier(1f);
    }
    private void OnPanicStateDying()
    {
        //this.heartBeat.HeartBeatSpeed(90f);
        //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.HEART_BEAT);
        //AudioManager.Instance.PlayAudio(AudioKeys.SFX, this.sourceName, SFXKeys.BREATHING);
        this.oxygen.SetOxygenDecreaseMultiplier(1.25f);
    }
    private void OnPanicStateDead()
    {
        // Add Panic Death Animation here
        this.EntityControls.Player.Movement.Disable();
        Debug.Log("Character is Scared to Death");
    }
    #endregion

    #region Oxygen Death
    // POSTEVENT = POST VIDEO
    // ADD OBSERVER = NOTIF TO THE VIDEO
    // GET PARAM = Get specific parameter
    public void OnOxygenStageDead()
    {
        this.EntityControls.Player.Movement.Disable();
        Debug.Log("No more Oxygen, Character is Dead");
    }
    #endregion

    #region Listeners
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == TagNames.DAMAGE)
        {
            Debug.Log("Enter");
            Damage damage = collision.GetComponent<Damage>();
            this.panic.ApplyPanicPressure(damage.PanicInfliction);
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
}
