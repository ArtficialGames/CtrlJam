using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Torch : LightSource
{
    [SerializeField] float decaySpeed;
    [SerializeField] float minRange;
    [SerializeField] Light2D torchLight;
    [SerializeField] CircleCollider2D col;
    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField] Animator animator;
    [SerializeField] AnimatorOverrideController withTorch;
    [SerializeField] AnimatorOverrideController noTorch;

    [SerializeField] GameObject lightCircle;

    Leader leader;
    float maxTorchLightRadius;

    private void Awake()
    {
        leader = transform.root.GetComponent<Leader>();
        maxTorchLightRadius = range;
    }

    private void Update()
    {
        if(range > minRange)
        {
            animator.runtimeAnimatorController = withTorch;
            lightCircle.SetActive(true);
            range -= decaySpeed * Time.deltaTime;
            leader.hud.UpdateTorchAmount(Mathf.CeilToInt((range - minRange) / (maxTorchLightRadius - minRange) * 100f));
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

        torchLight.pointLightInnerRadius = range;
        torchLight.pointLightOuterRadius = range;
        spriteRenderer.transform.parent.localScale = new Vector3(range, range, range);
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

    public float GetRadius()
    {
        return torchLight.pointLightInnerRadius;
    }

    public void Refuel(float amount)
    {
        range += amount;

        if (range > maxTorchLightRadius)
            range = maxTorchLightRadius;

        col.radius = range;
    }
}
