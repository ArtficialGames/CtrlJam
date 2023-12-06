using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float duration;

    Leader target;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Leader"))
        {
            target = collision.GetComponent<Leader>();
            StartCoroutine(BoostCoroutine());
        }
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            target.GetComponent<Rigidbody2D>().AddForce(target.GetComponent<Rigidbody2D>().velocity.normalized * speed);

            foreach (Survivor s in target.queue.survivors)
            {
                Rigidbody2D rb = s.GetComponent<Rigidbody2D>();
                rb.velocity = target.GetComponent<Rigidbody2D>().velocity;
            }
        }
    }

    IEnumerator BoostCoroutine()
    {
        yield return new WaitForSeconds(duration);
        target = null;
    }

}
