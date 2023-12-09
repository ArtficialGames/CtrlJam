using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuel : MonoBehaviour
{
    [SerializeField] float recoverAmount;
    [SerializeReference] AudioClip sfx;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Leader"))
        {
            collision.GetComponentInChildren<Torch>().Refuel(recoverAmount);
            AudioManager.Instance.PlaySFX(sfx);
            Destroy(gameObject);
        }
    }
}
