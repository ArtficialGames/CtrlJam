using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fuel : MonoBehaviour
{
    [SerializeField] float recoverAmount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Leader"))
        {
            collision.GetComponentInChildren<Torch>().Refuel(recoverAmount);
            Destroy(gameObject);
        }
    }
}
