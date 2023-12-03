using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Leader : Survivor
{
    [SerializeField] Transform followTarget;

    private void Awake()
    {
        queue = GetComponent<Queue>();
    }

    private void FixedUpdate()
    {
        Follow(followTarget);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Follower") && collision.GetComponent<StateMachine>().GetCurrentStateName() == "LOST")
        {
            collision.GetComponent<Follower>().QueueIn(this);
        }
    }
}
