using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follower : Survivor
{
    [SerializeField] float distanceLimit;

    StateMachine stateMachine;
    Collider2D col;
    public bool isSafe;

    private void Awake()
    {
        stateMachine = GetComponent<StateMachine>();
        col = GetComponent<Collider2D>();

        State unpickedState = new State("UNPICKED", EnterLostState, null, null);
        State followState = new State("FOLLOW", EnterFollowState, WhileInFollowState, null);
        State lostState = new State("LOST", EnterLostState, null, null);
        State deadState = new State("DEAD", EnterDeadState, null, null);

        State[] initialStates = { unpickedState, lostState, followState, deadState };

        stateMachine.Init(initialStates);

    }

    public void QueueIn(Leader leader)
    {
        queue = leader.GetComponent<Queue>();
        queue.Add(this);
        target = queue.GetNextInLine(this).transform;
        Follow(target);
        stateMachine.GoToState("FOLLOW");
    }

    public void QueueOut()
    {
        List<Survivor> survivorsBehind = queue.GetSurvivorsBehind(this);
        survivorsBehind.Reverse();

        foreach (var survivor in survivorsBehind)
        {
            survivor.GetComponent<StateMachine>().GoToState("LOST");
        }

        stateMachine.GoToState("LOST");
    }

    void EnterLostState()
    {
        GetComponent<SpriteRenderer>().color = Color.gray;
        queue?.Remove(this);
        queue = null;
        target = null;
        rb.velocity = Vector2.zero;
        col.isTrigger = true;
    }

    void EnterFollowState()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
        transform.position = target.position - target.up;
        col.isTrigger = false;
        rb.velocity = queue.survivors[0].GetComponent<Rigidbody2D>().velocity;
    }

    void WhileInFollowState()
    {
        Follow(target);

        if (Vector3.Distance(transform.position, target.position) > distanceFromTarget * distanceLimit)
            QueueOut();

        GetComponent<SpriteRenderer>().color = isSafe ? Color.green : Color.red;
    }

    void EnterDeadState()
    {
        queue?.Remove(this);
        Destroy(gameObject);
    }

    public override void Die()
    {
        base.Die();
        stateMachine.GoToState("DEAD");
    }
}
