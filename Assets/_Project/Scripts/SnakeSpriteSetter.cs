using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeSpriteSetter : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;

    [SerializeField] Sprite up;
    [SerializeField] Sprite upRight;
    [SerializeField] Sprite right;
    [SerializeField] Sprite downRight;
    [SerializeField] Sprite down;
    [SerializeField] Sprite downLeft;
    [SerializeField] Sprite left;
    [SerializeField] Sprite upLeft;

    private void Update()
    {
        print(transform.eulerAngles);

        if (transform.eulerAngles.z < 25f || transform.eulerAngles.z > 340f)
            spriteRenderer.sprite = up;

        else if (transform.eulerAngles.z > 295f && transform.eulerAngles.z < 340f)
            spriteRenderer.sprite = upRight;

        else if (transform.eulerAngles.z > 250f && transform.eulerAngles.z < 295f)
            spriteRenderer.sprite = upRight;

        else if (transform.eulerAngles.z > 205f && transform.eulerAngles.z < 250f)
            spriteRenderer.sprite = right;

        else if (transform.eulerAngles.z > 160f && transform.eulerAngles.z < 205f)
            spriteRenderer.sprite = downRight;

        else if (transform.eulerAngles.z > 115f && transform.eulerAngles.z < 160f)
            spriteRenderer.sprite = down;

        else if (transform.eulerAngles.z > 70f && transform.eulerAngles.z < 115f)
            spriteRenderer.sprite = downLeft;

        else if (transform.eulerAngles.z > 25f && transform.eulerAngles.z < 70f)
            spriteRenderer.sprite = left;

        else
            spriteRenderer.sprite = upLeft;
    }

}
