using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class Torch : MonoBehaviour
{
    [SerializeField] float decaySpeed;
    [SerializeField] Light2D torchLight;
    [SerializeField] CircleCollider2D col;

    Leader leader;
    float maxTorchLightRadius;

    private void Awake()
    {
        leader = GetComponent<Leader>();
        maxTorchLightRadius = torchLight.pointLightInnerRadius;
    }

    private void Update()
    {
        if(torchLight.pointLightInnerRadius > 0)
        {
            torchLight.pointLightInnerRadius -= decaySpeed * Time.deltaTime;
            torchLight.pointLightOuterRadius -= decaySpeed * Time.deltaTime;
        }

        col.radius = torchLight.pointLightInnerRadius;

        //ApplySafety();
    }

    void ApplySafety()
    {
        foreach (var survivor in GameObject.FindGameObjectsWithTag("Follower"))
        {
            survivor.GetComponent<Follower>().isSafe = Vector2.Distance(transform.position, survivor.transform.position) < torchLight.pointLightInnerRadius;
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

    public float GetRadius()
    {
        return torchLight.pointLightInnerRadius;
    }

    public void Refuel(float amount)
    {
        torchLight.pointLightInnerRadius += amount;
        torchLight.pointLightOuterRadius += amount;

        if (torchLight.pointLightInnerRadius > maxTorchLightRadius)
            torchLight.pointLightInnerRadius = maxTorchLightRadius;

        if (torchLight.pointLightOuterRadius > maxTorchLightRadius)
            torchLight.pointLightOuterRadius = maxTorchLightRadius;
    }
}
