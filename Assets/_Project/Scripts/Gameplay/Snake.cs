using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using Unity.VisualScripting;

public class Snake : MonoBehaviour
{
    [SerializeField] int health;
    [SerializeField] float wanderSpeed;
    [SerializeField] float chaseSpeed;
    [SerializeField] float afterDamageSpeed;
    [SerializeField] float attackDuration;
    [SerializeField] float nextWaypointDistance = 2f;
    [SerializeField] float distanceFromTargetMin;
    [SerializeField] float distanceFromTargetMax;
    [SerializeField] Bounds mapSize;

    [SerializeField] AudioClip attackSFX;
    [SerializeField] AudioClip gameplayMusic;
    [SerializeField] AudioClip chaseMusic;

    Rigidbody2D rb;
    public bool isAttacking;
    public bool isMouthOpen;
    bool canAttack = true;
    Transform target;
    Vector2 wanderPos;
    Vector2 afterDamagePos;
    Leader leader;
    StateMachine stateMachine;
    float currentSpeed;
    int maxHealth;

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
        State damageState = new State("DAMAGE", EnterDamageState, WhileInDamageState, null);

        State[] initialStates = { wanderState, chaseState, attackState, damageState };

        stateMachine.Init(initialStates);

        maxHealth = health;
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

    void UpdateDamagedPath()
    {
        seeker.StartPath(rb.position, afterDamagePos, OnPathComplete);
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
        Vector2 force = direction * currentSpeed * Time.fixedDeltaTime;

        rb.AddForce(force);
        Debug.DrawRay(transform.position, rb.velocity.normalized);

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

            if (lastInQueue.gameObject.CompareTag("Follower") && !lastInQueue.GetComponent<Follower>().isSafe || lastInQueue.transform.CompareTag("Leader"))
            {
                if(leader.transform.position.y > 90f)
                    return leader.queue.survivors[leader.queue.survivors.Count - 1].transform;
            }
        }

        return null;
    }

    Vector2 GetWanderingPos()
    {
        Vector2 randomPos = new Vector2(Random.Range(-65f, 70f), Random.Range(85f, 180f));
        Vector2 pos = new Vector2(Random.Range(distanceFromTargetMin, distanceFromTargetMax), Random.Range(distanceFromTargetMin, distanceFromTargetMax));

        /*while(Vector2.Distance(leader.transform.position, pos) < leader.GetComponentInChildren<Torch>().GetRadius())
            pos = new Vector2(Random.Range(-70f, 75f), Random.Range(90, 187f));

        print(pos);*/

        if (leader.transform.position.y > 90f)
            return (Vector2)leader.transform.position + pos;
        else
            return randomPos;

    }
    Vector2 GetPosAfterDamage()
    {
        Vector2 pos1 = new Vector2(60f, 105f);
        Vector2 pos2 = new Vector2(-60f, 105f);
        Vector2 pos3 = new Vector2(-60f, 180f);
        Vector2 pos4 = new Vector2(60f, 180f);


        List<Vector2> positions = new List<Vector2>();
        positions.Add(pos1);
        positions.Add(pos2);
        positions.Add(pos3);
        positions.Add(pos4);

        Vector2 farthest = positions[0];

        foreach (var position in positions)
        {
            if (Vector2.Distance(position, (Vector2)leader.transform.position) > Vector2.Distance(farthest, (Vector2)leader.transform.position))
                farthest = position;
        }

        return farthest;

        /*if (leader.transform.position.y < 135f)
            pos = new Vector2(0f, 100f);
        else
            pos = new Vector2(0f, 170f);

        return pos;*/
    }

    void Attack(GameObject attackTarget)
    {
        if (stateMachine.GetCurrentStateName() == "DAMAGE")
            return;

        if (attackTarget == null)
            return;

        if (attackTarget.GetComponent<Follower>() != null && attackTarget.GetComponent<Follower>().GetComponent<StateMachine>().GetCurrentStateName() == "UNPICKED")
            return;

        if (attackTarget.GetComponent<Follower>() != null && attackTarget.GetComponent<Follower>().queue != null && attackTarget.GetComponent<Survivor>() != attackTarget.GetComponent<Follower>().queue.survivors[attackTarget.GetComponent<Follower>().queue.survivors.Count - 1])
            return;

        if (!canAttack || target != null && attackTarget != target.root.gameObject)
            return;

        isAttacking = true;
        isMouthOpen = true;
        attackTarget.GetComponent<Survivor>().Die();
        StartCoroutine(AttackCooldown());
        rb.velocity = Vector2.zero;
        AudioManager.Instance.PlaySFX(attackSFX);
    }

    IEnumerator AttackCooldown()
    {
        canAttack = false;

        yield return new WaitForSeconds(0.15f);
        isMouthOpen = false;

        //SpriteRenderer spriteRenderer = GetComponentInChildren<AttackDetection>().GetComponent<SpriteRenderer>();

        //spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 1f);
        yield return new WaitForSeconds(attackDuration);
        //spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0f);

        canAttack = true;
        isAttacking = false;
    }

    void EnterWanderState()
    {
        wanderPos = GetWanderingPos();
        currentSpeed = wanderSpeed;
        InvokeRepeating("UpdateWanderingPath", 0f, 0.5f);
    }

    void WhileInWanderState()
    {
        PerformPath();

        if (Vector2.Distance(transform.position, wanderPos) < 5f)
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
        currentSpeed = chaseSpeed;
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

    void EnterDamageState()
    {
        rb.velocity = Vector2.zero;
        currentSpeed = afterDamageSpeed;
        afterDamagePos = GetPosAfterDamage();
        StartCoroutine(DamageAnim());
        InvokeRepeating("UpdateDamagedPath", 0f, 0.5f);
        AudioManager.Instance.ChangeMusicClip(gameplayMusic);
    }

    void WhileInDamageState()
    {
        PerformPath();

        if (Vector2.Distance(transform.position, afterDamagePos) < 2f)
            stateMachine.GoToState("WANDER");
    }

    void ExitDamageState()
    {
        CancelInvoke();
    }

    IEnumerator DamageAnim()
    {
        for (int i = 0; i < 1f / 0.3f; i++)
        {
            foreach (Transform item in transform.root)
            {
                item.GetComponent<SpriteRenderer>().color = Color.red;
            }

            transform.root.GetChild(0).Find("Sprite").GetComponent<SpriteRenderer>().color = Color.red;

            yield return new WaitForSeconds(0.15f);

            foreach (Transform item in transform.root)
            {
                item.GetComponent<SpriteRenderer>().color = Color.white;
            }

            transform.root.GetChild(0).Find("Sprite").GetComponent<SpriteRenderer>().color = Color.white;

            yield return new WaitForSeconds(0.15f);
        }

        foreach (Transform item in transform.root)
        {
            item.GetComponent<SpriteRenderer>().color = Color.white;
        }

        transform.root.GetChild(0).Find("Sprite").GetComponent<SpriteRenderer>().color = Color.white;
    }

    public void TakeDamage(int damage)
    {
        health -= damage;

        stateMachine.GoToState("DAMAGE");

        if (health <= 0)
            Die();
    }

    void Die()
    {
        GameController.Instance.Win();
        Destroy(gameObject.transform.root.gameObject);
    }
}
