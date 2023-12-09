using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Leader : Survivor
{
    [SerializeField] Transform followTarget;
    StateMachine stateMachine;
    public HUD hud;

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

    private void Update()
    {
        HandleAnimation();

        if (Input.GetKeyDown(KeyCode.Escape))
            Pause();
    }

    void Pause()
    {
        SceneManager.LoadScene(0);
    }

    void Attack(GameObject target)
    {
        if (target.transform.root.GetChild(0).GetComponent<Snake>().GetComponent<StateMachine>().GetCurrentStateName() == "DAMAGE")
            return;

        rb.velocity = Vector2.zero;
        rb.AddForce((transform.position - target.transform.position).normalized * 1000f);
        target.transform.root.GetChild(0).GetComponent<Snake>().TakeDamage(1);
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

    void HandleAnimation()
    {
        animator.SetFloat("Speed", rb.velocity.magnitude);
        spriteRenderer.flipX = Camera.main.ScreenToWorldPoint(Input.mousePosition).x < transform.position.x;
    }
}
