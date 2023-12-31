using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boost : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float duration;
    [SerializeReference] AudioClip sfx;

    Leader target;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Leader"))
        {
            target = collision.GetComponent<Leader>();
            StartCoroutine(BoostCoroutine());
            AudioManager.Instance.PlaySFX(sfx);
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
        target.queue.undetachable = true;
        yield return new WaitForSeconds(duration);

        if(target)
            target.queue.undetachable = false;

        target = null;
    }

}
