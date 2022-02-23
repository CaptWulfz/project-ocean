using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [SerializeField] protected Rigidbody2D rigidBody;

    private Controls controls;
    public Controls EntityControls
    {
        get { return this.controls; }
        set { this.controls = value; }
    }

    [SerializeField] private float currentSpeed = 0f;
    [SerializeField] private float maxSpeed = 10f;
    [SerializeField] private float minSpeed = 5f;
    [SerializeField] private float midSpeed = 6.5f;

    protected float CurrentSpeed
    {
        get { return this.currentSpeed; }
        set { this.currentSpeed = value; }
    }
    protected float MaxSpeed
    {
        get { return this.maxSpeed; }
        set { this.maxSpeed = value; }
    }
    protected float MidSpeed
    {
        get { return this.midSpeed; }
        set { this.midSpeed = value; }
    }
    protected float MinSpeed
    {
        get { return this.minSpeed; }
        set { this.minSpeed = value; }
    }

    protected virtual void Initialize()
    {
        this.EntityControls = InputManager.Instance.GetControls();
    }

    protected void MovePosition(Vector2 move)
    {
        this.rigidBody.velocity = move;
    }
    #region Collision Events

    protected virtual void OnCollisionEnter2D(Collision2D collision)
    {
        
    }

    protected virtual void OnCollisionExit2D(Collision2D collision)
    {
        
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {

    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {

    }
    #endregion
}
