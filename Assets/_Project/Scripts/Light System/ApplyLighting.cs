using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.FilePathAttribute;

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
        if (PixelLighting.Instance != null)
        {
            lightLevel = PixelLighting.Instance.GetTileLightLevel(Vector3Int.RoundToInt(transform.position));
            spriteRenderer.sprite = lightSprite[lightLevel];
        }
    }
}
