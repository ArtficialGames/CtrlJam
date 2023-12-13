using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Torch : LightSource
{
    [SerializeField] float decaySpeed;
    [SerializeField] float minRange;
    [SerializeField] CircleCollider2D col;

    [SerializeField] Animator animator;
    [SerializeField] AnimatorOverrideController withTorch;
    [SerializeField] AnimatorOverrideController noTorch;

    [SerializeField] GameObject lightCircle;

    Leader leader;
    float maxRadius;

    private void Awake()
    {
        leader = transform.root.GetComponent<Leader>();
        maxRadius = range;
    }

    private void Update()
    {
        if(range > minRange)
        {
            animator.runtimeAnimatorController = withTorch;
            lightCircle.SetActive(true);
            range -= decaySpeed * Time.deltaTime;
            leader.hud.UpdateTorchAmount(Mathf.CeilToInt((range - minRange) / (maxRadius - minRange) * 100f));
        }
        else
        {
            lightCircle.SetActive(false);
            animator.runtimeAnimatorController = noTorch;
        }

        if (col.radius > 0f)
        {
            col.radius -= decaySpeed * Time.deltaTime;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Follower"))
            collision.GetComponent<Follower>().isSafe = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Follower"))
            collision.GetComponent<Follower>().isSafe = false;
    }

    public void Refuel(float amount)
    {
        range += amount;

        if (range > maxRadius)
            range = maxRadius;

        col.radius = range;
    }
}
