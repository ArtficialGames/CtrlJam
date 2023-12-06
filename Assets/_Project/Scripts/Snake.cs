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
    Vector2 wanderPos;
    Leader leader;
    StateMachine stateMachine;

    Path path;
    int currentWaypoint = 0;
    bool reachedEndOfPath;
    Seeker seeker;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        stateMachine = GetComponent<StateMachine>();

        GetComponentInChildren<AttackDetection>().OnDectection += Attack;
        leader = FindObjectOfType<Leader>();

        State wanderState = new State("WANDER", EnterWanderState, WhileInWanderState, ExitWanderState);
        State chaseState = new State("CHASE", EnterChaseState, WhileInChaseState, ExitChaseState);
        State attackState = new State("ATTACK", null, null, null);

        State[] initialStates = { wanderState, chaseState, attackState };

        stateMachine.Init(initialStates);
    }

    void UpdateWanderingPath()
    {
        seeker.StartPath(rb.position, wanderPos, OnPathComplete);
    }

    void UpdateChasingPath()
    {
        target = GetTarget();

        if (target != null)
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        else
            stateMachine.GoToState("WANDER");
    }

    void OnPathComplete(Path p)
    {
        if (!p.error)
        {
            path = p;
            currentWaypoint = 0;
        }
    }

    void PerformPath()
    {
        if (path == null || isAttacking)
            return;

        if (currentWaypoint >= path.vectorPath.Count)
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

        if (distance < nextWaypointDistance)
        {
            currentWaypoint++;
        }
    }

    Transform GetTarget()
    {
        List<Follower> lostFollowers = new List<Follower>();

        foreach (var follower in GameObject.FindGameObjectsWithTag("Follower"))
        {
            if (follower.GetComponent<StateMachine>().GetCurrentStateName() == "LOST" && !follower.GetComponent<Follower>().isSafe)
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
        else 
        {
            Survivor lastInQueue = leader.queue.survivors[leader.queue.survivors.Count - 1];

            if (lastInQueue.gameObject.CompareTag("Follower") && !lastInQueue.GetComponent<Follower>().isSafe || leader.queue.survivors[leader.queue.survivors.Count - 1].CompareTag("Leader"))
                return leader.queue.survivors[leader.queue.survivors.Count - 1].transform;
        }

        return null;
    }

    Vector2 GetWanderingPos()
    {
        Vector2 pos = new Vector2(Random.Range(-25f, 25f), Random.Range(-25f, 25f));

        while(Vector2.Distance(leader.transform.position, pos) < leader.GetComponentInChildren<Torch>().GetRadius())
            pos = new Vector2(Random.Range(-25f, 25f), Random.Range(-25f, 25f));

        return pos;
    }

    void Attack(GameObject attackTarget)
    {
        if (attackTarget == null)
            return;

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

    void EnterWanderState()
    {
        wanderPos = GetWanderingPos();
        InvokeRepeating("UpdateWanderingPath", 0f, 0.5f);
    }

    void WhileInWanderState()
    {
        PerformPath();

        if (Vector2.Distance(transform.position, wanderPos) < 2f)
            wanderPos = GetWanderingPos();

        if (GetTarget() != null)
            stateMachine.GoToState("CHASE");
    }

    void ExitWanderState()
    {
        CancelInvoke();
    }

    void EnterChaseState()
    {
        InvokeRepeating("UpdateChasingPath", 0f, 0.5f);
    }

    void WhileInChaseState()
    {
        PerformPath();

        if (GetTarget() == null)
            stateMachine.GoToState("WANDER");
    }
    void ExitChaseState()
    {
        CancelInvoke();
    }
}
