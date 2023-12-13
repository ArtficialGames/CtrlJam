using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Leader : Survivor
{
    public DynamicJoystick joystick;
    StateMachine stateMachine;
    public HUD hud;

    private void Awake()
    {
        queue = GetComponent<Queue>();
        stateMachine = GetComponent<StateMachine>();

        State moveState = new State("MOVE", null, WhileInMoveState, null);
        State attackState = new State("ATTACK", null, null, null);
        State offState = new State("OFF", null, null, null);

        State[] initialStates = { moveState , attackState, offState };
        stateMachine.Init(initialStates);

        GetComponentInChildren<AttackDetection>().OnDectection += Attack;
    }

    private void Update()
    {
        HandleAnimation();

        if (Input.GetKeyDown(KeyCode.Escape))
            Pause();
    }

    public void Pause()
    {
        if (Time.timeScale == 0)
        {
            SceneManager.UnloadSceneAsync("Pause");
            Time.timeScale = 1;
        }
        else
            SceneManager.LoadScene("Pause", LoadSceneMode.Additive);
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

    void WhileInMoveState()
    {
        MoveTo((Vector2)transform.position + joystick.Direction, joystick.Direction.magnitude);
    }

    void HandleAnimation()
    {
        animator.SetFloat("Speed", rb.velocity.magnitude);
        spriteRenderer.flipX = joystick.Direction.x < 0;

    }
}
