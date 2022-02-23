using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player : Entity //
{    
    //NEEDS CLEANING... -Gelo
    private Vector2 currentInputVector;
    private Vector2 smoothInputVelocity;
    private Vector2 mouseDelta;
    [SerializeField] private float mouseLookSpeed = 0.02f;
    [SerializeField] private float smoothInputSpeed = .2f;
    [SerializeField] private bool buildMomentum = false;

    private void Start()
    {
        Initialize();
        EntityControls.Player.Movement.started += _ => MinMovement();
        EntityControls.Player.Movement.performed += _ => MaxMovement();
        EntityControls.Player.Movement.canceled += _ => MinMovement();
    }

    protected override void Initialize() //Move vars to this class rather than Init -TODO
    {
        base.Initialize();
        this.CurrentSpeed = 0f;
        this.MaxSpeed = 8.0f;
        this.MidSpeed = 6.5f;
        this.MinSpeed = 5.0f;
        this.EntityControls.Player.Enable();
    }

    private void Update()
    {
        MouseLook();
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    #region MOVEMENT

    private void MovePlayer()
    { 
        Vector2 input = this.EntityControls.Player.Movement.ReadValue<Vector2>();
        currentInputVector = Vector2.SmoothDamp(currentInputVector, input, ref smoothInputVelocity, smoothInputSpeed);
        Vector2 move = new Vector2(currentInputVector.x, currentInputVector.y);

        if(buildMomentum)
            this.MovePosition(move * this.MaxSpeed);
        else
            this.MovePosition(move * this.MinSpeed);
    }

    private void MinMovement(){
        buildMomentum = false;
    }

    private void MaxMovement(){
        buildMomentum = true;
    }

    #endregion

    #region CAMERA LOOK

    private void MouseLook()
    {
        Vector3 direction = Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, rotation, mouseLookSpeed);
    }

    #endregion

    IEnumerator AccelTime(Vector2 move){

        
        yield return new WaitForSeconds(2f);
        this.MovePosition(move * this.MaxSpeed);
        Debug.Log("Waited");
    }

    
}
