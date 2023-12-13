using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AttackDetection : MonoBehaviour
{
    public Action<GameObject> OnDectection;
    [SerializeField] bool isPlayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isPlayer)
        {
            if (collision.CompareTag("WeakSpot"))
            {
                OnDectection?.Invoke(collision.gameObject);
            }
        }
        else
        {
            if (collision.CompareTag("Follower") || collision.CompareTag("Leader"))
            {
                OnDectection?.Invoke(collision.gameObject);
            }
        }
    }
}
