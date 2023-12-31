using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class Follower : Survivor
{
    [SerializeField] float distanceLimit;
    [SerializeField] AudioClip pickupSFX;
    [SerializeField] AudioClip breakSFX;

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
        if (leader.queue.survivors.Count - 1 >= leader.queue.maxSurvivors)
            return;

        queue = leader.GetComponent<Queue>();
        queue.Add(this);
        target = queue.GetNextInLine(this).transform;
        MoveTo(target.position, leader.joystick.Direction.magnitude);
        stateMachine.GoToState("FOLLOW");
        AudioManager.Instance.PlaySFX(pickupSFX);
    }

    public void QueueOut()
    {
        if (queue.undetachable)
            return;

        List<Survivor> survivorsBehind = queue.GetSurvivorsBehind(this);
        survivorsBehind.Reverse();

        foreach (var survivor in survivorsBehind)
        {
            survivor.GetComponent<StateMachine>().GoToState("LOST");
        }

        stateMachine.GoToState("LOST");
        AudioManager.Instance.PlaySFX(breakSFX);
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
        MoveTo(target.position, queue.GetComponent<Leader>().joystick.Direction.magnitude);

        if (Vector3.Distance(transform.position, target.position) > distanceLimit)
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

        GameObject.FindGameObjectWithTag("HUD").GetComponent<HUD>().PlayHUDAnimation();
        //GameObject.FindGameObjectWithTag("HUD").GetComponent<HUD>().UpdateSurvivorsCount(queue.survivors.Count, GameObject.FindGameObjectsWithTag("Follower").Length);

        stateMachine.GoToState("DEAD");
    }

    private void Update()
    {
        HandAnimation();
    }

    void HandAnimation()
    {
        animator.SetFloat("Speed", rb.velocity.magnitude);
        animator.SetBool("Crying", stateMachine.GetCurrentStateName() == "UNPICKED" || stateMachine.GetCurrentStateName() == "LOST");

        if (queue != null)
        {
            //spriteRenderer.flipX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x < transform.position.x;
            spriteRenderer.flipX = rb.velocity.x < 0;
        }
    }
}
