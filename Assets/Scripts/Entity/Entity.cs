using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Entity : MonoBehaviour
{
    [Header("Player Settings")]
    [SerializeField] protected Rigidbody2D rigidBody;

    private Controls controls;
    public Controls EntityControls
    {
        get { return this.controls; }
        set { this.controls = value; }
    }

    protected string sourceName = "";
    private float speed = 5f;
    protected float Speed
    {
        get { return this.speed; }
        set { this.speed = value; }
    }

    protected Dictionary<string, object> animatorParameters;

    protected virtual void Initialize()
    {
        this.EntityControls = InputManager.Instance.GetControls();
    }

    protected void MovePosition(Vector2 move)
    {
        this.rigidBody.velocity = move;
    }

    protected void FollowTarget(Transform target, float speed)
    {
        this.rigidBody.MovePosition(Vector2.MoveTowards(this.transform.position, target.position, speed * Time.deltaTime));
    }

    protected void PerformDeath()
    {

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
