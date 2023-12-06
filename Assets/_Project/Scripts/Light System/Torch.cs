using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Torch : LightSource
{
    [SerializeField] float decaySpeed;
    [SerializeField] Light2D torchLight;
    [SerializeField] CircleCollider2D col;
    [SerializeField] SpriteRenderer spriteRenderer;

    Leader leader;
    float maxTorchLightRadius;

    private void Awake()
    {
        leader = GetComponent<Leader>();
        maxTorchLightRadius = range;
    }

    private void Update()
    {
        if(range > 0)
        {
            range -= decaySpeed * Time.deltaTime;
        }

        col.radius = range;
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
    }
}
