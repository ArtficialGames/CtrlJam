using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AttackDetection : MonoBehaviour
{
    public Action<GameObject> OnDectection;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Follower") || collision.CompareTag("Leader"))
        {
            OnDectection.Invoke(collision.gameObject);
        }
    }
}
