using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    [SerializeField] float speed;

    Rigidbody2D rb;
    Transform target;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        target = GetTarget();
    }

    void FixedUpdate()
    {
        Follow(target);
    }

    protected void Follow(Transform target)
    {
        target.position = new Vector3(target.position.x, target.position.y, 0);

        Vector3 diff = target.position - transform.position;
        diff.Normalize();

        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0f, 0f, rot_z);

        if (Vector3.Distance(transform.position, target.position) > 1f)
        {
            rb.velocity = transform.right * speed * Time.fixedDeltaTime;
        }
        else
            rb.velocity = Vector2.zero;
    }

    Transform GetTarget()
    {
        return FindObjectOfType<Leader>().queue.survivors[FindObjectOfType<Leader>().queue.survivors.Count - 1].transform;
    }

}
