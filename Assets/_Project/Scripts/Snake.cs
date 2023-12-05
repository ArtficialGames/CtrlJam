using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Snake : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float nextWaypointDistance = 2f;
    [SerializeField] Transform target;

    Rigidbody2D rb;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath;
    Seeker seeker;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void UpdatePath()
    {
        target = GetTarget();
        seeker.StartPath(rb.position, target.position, OnPathComplete);
    }

    void FixedUpdate()
    {
        //Follow(target);

        if (path == null)
            return;

        if(currentWaypoint >= path.vectorPath.Count)
        {
            reachedEndOfPath = true;
            return;
        }
        else
        {
            reachedEndOfPath = false;
        }

        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.fixedDeltaTime;

        rb.AddForce(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        if(distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    Transform GetTarget()
    {
        return FindObjectOfType<Leader>().queue.survivors[FindObjectOfType<Leader>().queue.survivors.Count - 1].transform;
    }

}
