using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Snake : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float attackDuration;
    [SerializeField] float nextWaypointDistance = 2f;

    Rigidbody2D rb;
    bool isAttacking;
    bool canAttack = true;
    Transform target;
    Leader leader;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath;
    Seeker seeker;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);
        GetComponentInChildren<AttackDetection>().OnDectection += Attack;
        leader = FindObjectOfType<Leader>();
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

        if (path == null || isAttacking)
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
        List<Follower> lostFollowers = new List<Follower>();

        foreach (var follower in GameObject.FindGameObjectsWithTag("Follower"))
        {
            if (follower.GetComponent<StateMachine>().GetCurrentStateName() == "LOST")
                lostFollowers.Add(follower.GetComponent<Follower>());
        }

        if(lostFollowers.Count > 0)
        {
            Follower nearestFollower = null;

            foreach (var follower in lostFollowers)
            {
                if (nearestFollower == null || nearestFollower != null && Vector2.Distance(transform.position, follower.transform.position) < Vector2.Distance(transform.position, nearestFollower.transform.position))
                {
                    nearestFollower = follower;
                }
            }

            return nearestFollower.transform;
        }
        else return leader.queue.survivors[leader.queue.survivors.Count - 1].transform;
    }

    void Attack(GameObject attackTarget)
    {
        if (!canAttack || attackTarget != target.root.gameObject)
            return;

        isAttacking = true;
        attackTarget.GetComponent<Survivor>().Die();
        StartCoroutine(AttackCooldown());
        rb.velocity = Vector2.zero;
    }

    IEnumerator AttackCooldown()
    {
        canAttack = false;

        SpriteRenderer spriteRenderer = GetComponentInChildren<AttackDetection>().GetComponent<SpriteRenderer>();

        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
        yield return new WaitForSeconds(attackDuration);
        spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0f);

        canAttack = true;
        isAttacking = false;
    }
}
