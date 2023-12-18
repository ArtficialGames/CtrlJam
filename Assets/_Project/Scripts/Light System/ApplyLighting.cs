using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApplyLighting : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public Sprite[] lightSprite;

    private int lightLevel = 0;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        InvokeRepeating("UpdateLighting", 0f, 0.15f);
    }

    private void UpdateLighting()
    {
        spriteRenderer.sprite = lightSprite[lightSprite.Length - 1];

        if (lightLevel < lightSprite.Length)
            spriteRenderer.sprite = lightSprite[lightLevel];
    }

    public void SetLightLevel(int level)
    {
        lightLevel = level;
    }

    public void TurnOff()
    {
        spriteRenderer.sprite = lightSprite[lightSprite.Length - 1];
        CancelInvoke();
    }

    public void TurnOn()
    {
        spriteRenderer.sprite = lightSprite[lightSprite.Length - 1];
        InvokeRepeating("UpdateLighting", 0f, 0.15f);
    }

}
