using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity //
{    

    enum SpeedStates{
        MIN,
        MID,
        MAX
    }
    private SpeedStates currentSpeedState;

    enum LookStates{ //COMPASS
        N, S, E, W, NE, NW, SE, SW
    }
    private LookStates currentLookState;
    private LookStates currentMoveState;
    

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
    }

    private void Update()
    {
    }

    private void FixedUpdate()
    {
        MovePlayerWASD();                   //USES WASD
        SwitchLookState();
        currentSpeed = rigidBody.velocity.magnitude;   //Just records the current speed
        //Debug.Log(currentSpeed);
    }
    #endregion
    //--------
    #region MOVEMENT
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
    #region STATE SWITCH
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
        //Debug.Log("Crout State: "+currentSpeedState);
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
            if(currentMoveState == LookStates.E || currentMoveState == LookStates.NE || currentMoveState == LookStates.SE || currentMoveState == LookStates.S || currentMoveState == LookStates.N)
            {
                Debug.Log("Moving OPPOSITE of W");
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
        else if (((mouseAngleZ >= 0.0f) && (mouseAngleZ <= 30.0f)||(mouseAngleZ >= 330.0f) && (mouseAngleZ <= 360.0f)))
        {
            currentLookState = LookStates.E;                                //EAST
            if(currentMoveState == LookStates.W || currentMoveState == LookStates.NW || currentMoveState == LookStates.SW || currentMoveState == LookStates.S || currentMoveState == LookStates.N)
            {
                Debug.Log("Moving OPPOSITE of E");
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
            if(currentMoveState == LookStates.E || currentMoveState == LookStates.NE || currentMoveState == LookStates.SE )
            {
                Debug.Log("Moving OPPOSITE of W");
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
            if(currentMoveState == LookStates.E || currentMoveState == LookStates.NE || currentMoveState == LookStates.SE )
            {
                Debug.Log("Moving OPPOSITE of W");
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
            if(currentMoveState == LookStates.W || currentMoveState == LookStates.NW || currentMoveState == LookStates.SW || currentMoveState == LookStates.S || currentMoveState == LookStates.N)
            {
                Debug.Log("Moving OPPOSITE of SE");
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
            if(currentMoveState == LookStates.W || currentMoveState == LookStates.NW || currentMoveState == LookStates.SW || currentMoveState == LookStates.S || currentMoveState == LookStates.N)
            {
                Debug.Log("Moving OPPOSITE of NE");
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
        //Debug.Log(currentLookState);
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

        Debug.Log("Moving: "+currentMoveState);
    }
    #endregion

    
}
