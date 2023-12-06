using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerPicker : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Follower") && collision.GetComponent<StateMachine>().GetCurrentStateName() == "UNPICKED" || collision.CompareTag("Follower") && collision.GetComponent<StateMachine>().GetCurrentStateName() == "LOST")
        {
            collision.GetComponent<Follower>().QueueIn(transform.root.GetComponent<Leader>());
        }
    }
}
