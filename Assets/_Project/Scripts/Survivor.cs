using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Survivor : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float maxSpeed;
    [SerializeField] protected float distanceFromTarget;
    [SerializeField] protected Rigidbody2D rb;
    [HideInInspector] public Queue queue;
    protected Transform target;

    protected void Follow(Transform target)
    {
        target.position = new Vector3(target.position.x, target.position.y, 0);

        Vector3 diff = target.position - transform.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);

        float currentSpeed = speed * GetDistanceFromCursor() < maxSpeed ? speed * GetDistanceFromCursor() : maxSpeed;

        if (Vector3.Distance(transform.position, target.position) > distanceFromTarget)
        {
            //rb.velocity = transform.up * currentSpeed * Time.fixedDeltaTime;
            rb.AddForce(transform.up * currentSpeed * Time.fixedDeltaTime);
        }
        else
            rb.velocity = Vector2.zero;
    }

    float GetDistanceFromCursor()
    {
        return Vector2.Distance(transform.position, Camera.main.ScreenToWorldPoint(Input.mousePosition));
    }

    public virtual void Die()
    {

    }
}
