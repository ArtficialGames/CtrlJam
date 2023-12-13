using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Survivor : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float maxMultiplier = 5;
    [SerializeField] float minMultiplier = 1;
    [SerializeField] protected float distanceFromEndPos;
    [SerializeField] protected Rigidbody2D rb;
    [SerializeField] protected Animator animator;
    [SerializeField] protected SpriteRenderer spriteRenderer;
    [HideInInspector] public Queue queue;
    protected Transform target;

    protected void MoveTo(Vector2 pos, float speedMultiplier)
    {

        Vector2 diff = pos - (Vector2)transform.position;
        diff.Normalize();
        

        float currentSpeed;
        currentSpeed = speedMultiplier * maxMultiplier < minMultiplier ? speed * minMultiplier : speed * speedMultiplier * maxMultiplier;

        if (Vector3.Distance(transform.position, pos) > distanceFromEndPos)
        {
            rb.AddForce(diff * currentSpeed * Time.fixedDeltaTime);
        }
        else
            rb.velocity = Vector2.zero;
    }

    public virtual void Die()
    {
    }
}
