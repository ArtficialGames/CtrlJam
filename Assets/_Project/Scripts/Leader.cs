using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leader : Survivor
{
    [SerializeField] Transform followTarget;
    StateMachine stateMachine;

    private void Awake()
    {
        queue = GetComponent<Queue>();
        stateMachine = GetComponent<StateMachine>();

        State moveState = new State("MOVE", null, null, null);
        State attackState = new State("ATTACK", null, null, null);

        State[] initialStates = { moveState , attackState};
        stateMachine.Init(initialStates);

        GetComponentInChildren<AttackDetection>().OnDectection += Attack;
    }

    private void FixedUpdate()
    {
        if(stateMachine.GetCurrentStateName() == "MOVE")
            Follow(followTarget);
    }

    void Attack(GameObject target)
    {
        if (target.GetComponentInChildren<Snake>().GetComponent<StateMachine>().GetCurrentStateName() == "DAMAGE")
            return;

        rb.velocity = Vector2.zero;
        rb.AddForce((transform.position - target.transform.position).normalized * 1000f);
        target.GetComponentInChildren<Snake>().TakeDamage(1);
        stateMachine.GoToState("ATTACK");
        StartCoroutine(AttackCoroutine());
    }

    IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(0.5f);
        stateMachine.GoToState("MOVE");
    }

    public override void Die()
    {
        base.Die();
        GameController.Instance.GameOver();
        Destroy(gameObject);
    }
}
