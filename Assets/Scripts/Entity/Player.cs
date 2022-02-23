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

    

    [Header("Player Settings")]
    //[SerializeField] Rigidbody2D playerRigidbody;
    [SerializeField] Transform lookRotation;
    [SerializeField] private float smoothInputSpeed = 0.2f; //SmoothDamp value
    private bool followMouse = false;
    private Vector2 currentInputVector;
    private Vector2 smoothInputVelocity;
    
    [Header("Player Speed")]
    [SerializeField] private float currentSpeed = 0f;
    [SerializeField] private float minSpeed = 3f;
    [SerializeField] private float midSpeed = 4f;
    [SerializeField] private float maxSpeed = 5f;
    [SerializeField] private float speedMultiplier;

    [Header("Mouse Settings")]
    Vector2 mousePosition;
    private float moveSpeed = 0.01f;
    Vector2 position = new Vector2(0f, 0f);

    private Coroutine switchStateDelay = null;

    public float SpeedMultiplier
    {
        get { return this.speedMultiplier; }
    }

    #region MACHINE RUNTIME
    private void Start()
    {
        Initialize();
        this.EntityControls.Player.MouseMove.performed += _ => FollowMouse();
        this.EntityControls.Player.MouseMove.canceled += _ => StopFollowMouse();
    }

    protected override void Initialize() //Move vars to this class rather than Init -TODO
    {
        base.Initialize();
        this.EntityControls.Player.Enable();
        this.currentSpeed = 0f;
        this.minSpeed = 0.1f;
        this.midSpeed = 0.2f;
        this.maxSpeed = 0.3f;
        this.currentSpeedState = SpeedStates.MIN; //Init default value
    }

    private void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //MovePlayerWASD();
        MovePlayerMouse();
        currentSpeed = this.rigidBody.velocity.magnitude;
        //Debug.Log(currentSpeed);
    }
    #endregion
    //--------
    #region MOVEMENT
    private void MovePlayerWASD()
    {
        Vector2 input = this.EntityControls.Player.Movement.ReadValue<Vector2>();
        currentInputVector = Vector2.SmoothDamp(currentInputVector, input, ref smoothInputVelocity, smoothInputSpeed);
        Vector2 move = new Vector2(currentInputVector.x, currentInputVector.y);

        speedMultiplier = minSpeed;

        if(currentSpeedState == SpeedStates.MIN )
        {
            Debug.Log("CurrentSpeedState MIN");
            speedMultiplier = minSpeed;
        }
        else if(currentSpeedState == SpeedStates.MID)
        {
            Debug.Log("CurrentSpeedState MID");
            speedMultiplier = midSpeed;
        }
        else if(currentSpeedState == SpeedStates.MAX)
        {
            Debug.Log("CurrentSpeedState MAX");
            speedMultiplier = maxSpeed;
        }
        if(input == Vector2.zero)   //Sets the SpeedState to MIN if there is no input
        {
            if(switchStateDelay != null)
            {
                StopCoroutine(switchStateDelay);
                switchStateDelay = null;
            }
            currentSpeedState = SpeedStates.MIN;
        }
        if(currentSpeedState != SpeedStates.MAX)
        {
            if(switchStateDelay == null)
            {
                switchStateDelay = StartCoroutine(SwitchSpeedState());
            }
        }
        
        this.MovePosition(move * speedMultiplier);
    }
    
    private void FollowMouse(){
        Debug.Log("Following");
        followMouse = true;
    }

    private void StopFollowMouse(){
        Debug.Log("Stopping");
        followMouse = false;
    }
    private void MovePlayerMouse()
    {
        
        //speedMultiplier = minSpeed;
        mousePosition = Mouse.current.position.ReadValue();
        mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
        //position = Vector2.Lerp(transform.position, mousePosition, speedMultiplier); 
        //position = Vector2.MoveTowards(transform.position, mousePosition, speedMultiplier);
        position = Vector2.SmoothDamp(transform.position, mousePosition, ref smoothInputVelocity, smoothInputSpeed);


        if(currentSpeedState == SpeedStates.MIN)
        {
            Debug.Log("CurrentSpeedState MIN");
            speedMultiplier = minSpeed;
        }
        else if(currentSpeedState == SpeedStates.MID)
        {
            Debug.Log("CurrentSpeedState MID");
            speedMultiplier = midSpeed;
        }
        else if(currentSpeedState == SpeedStates.MAX)
        {
            Debug.Log("CurrentSpeedState MAX");
            speedMultiplier = maxSpeed;
        }
        if(!followMouse)   //Sets the SpeedState to MIN if there is no input
        {
            if(switchStateDelay != null)
            {
                StopCoroutine(switchStateDelay);
                switchStateDelay = null;
            }
            currentSpeedState = SpeedStates.MIN;
        }
        if(currentSpeedState != SpeedStates.MAX)
        {
            if(switchStateDelay == null)
            {
                switchStateDelay = StartCoroutine(SwitchSpeedState());
            }
        }
        
        if(followMouse)
            this.rigidBody.MovePosition(position * speedMultiplier);
    }
    #endregion
    //--------
    #region STATE SWITCH
    IEnumerator SwitchSpeedState(){
        
        Debug.Log("Waiting for 2 Secs");
        yield return new WaitForSeconds(2f);
        if(currentSpeedState == SpeedStates.MIN)
        {
            currentSpeedState = SpeedStates.MID;
        }
        else if(currentSpeedState == SpeedStates.MID)
        {
            currentSpeedState = SpeedStates.MAX;
        }
        /*
        else if (currentSpeedState == SpeedStates.MAX)
        {
            currentSpeedState = SpeedStates.MIN;
        }
        */
        Debug.Log(currentSpeedState);
        switchStateDelay = null;
    }
    #endregion

    
}
