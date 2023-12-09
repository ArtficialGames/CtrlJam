using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AttackDetection : MonoBehaviour
{
    public Action<GameObject> OnDectection;
    [SerializeField] bool isPlayer;
    [SerializeField] AudioClip sfx;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isPlayer)
        {
            if (collision.CompareTag("WeakSpot"))
            {
                OnDectection?.Invoke(collision.gameObject);
                AudioManager.Instance.PlaySFX(sfx);
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
