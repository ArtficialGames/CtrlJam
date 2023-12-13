using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyLightingAnimated : MonoBehaviour
{
    Animator animator;
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] AnimatorOverrideController normal;
    [SerializeField] AnimatorOverrideController lowLight;

    int lightLevel;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        InvokeRepeating("UpdateLighting", 0f, 0.15f);
    }

    private void UpdateLighting()
    {
        if (PixelLighting.Instance != null)
        {
            lightLevel = PixelLighting.Instance.GetTileLightLevel(Vector3Int.RoundToInt(transform.position));

            if(GetComponent<Follower>().queue == null)
            {
                if (lightLevel == 0)
                    spriteRenderer.enabled = false;
                else
                {
                    spriteRenderer.enabled = true;

                    if (lightLevel < 4 && lightLevel > 0)
                        animator.runtimeAnimatorController = lowLight;
                    else
                        animator.runtimeAnimatorController = normal;
                }
            }
            else
            {
                spriteRenderer.enabled = true;

                if (lightLevel < 4)
                    animator.runtimeAnimatorController = lowLight;
                else
                    animator.runtimeAnimatorController = normal;
            }
        }
    }

}
