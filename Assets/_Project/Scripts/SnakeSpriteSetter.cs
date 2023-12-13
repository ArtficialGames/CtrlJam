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

    [SerializeField] SnakeHead upHead = new SnakeHead();
    [SerializeField] SnakeHead upRightHead = new SnakeHead();
    [SerializeField] SnakeHead rightHead = new SnakeHead();
    [SerializeField] SnakeHead downRightHead = new SnakeHead();
    [SerializeField] SnakeHead downHead = new SnakeHead();
    [SerializeField] SnakeHead downLeftHead = new SnakeHead();
    [SerializeField] SnakeHead leftHead = new SnakeHead();
    [SerializeField] SnakeHead upLeftHead = new SnakeHead();

    Snake snake;

    int state;
    int correctArray;

    private int lightLevel = 0;

    private void Awake()
    {
        snake = GetComponent<Snake>();
        //InvokeRepeating("UpdateLighting", 0f, 0.15f);

        /*upHead.main[0] = up;
        upRightHead.main[0] = upRight;
        rightHead.main[0] = right;
        downRightHead.main[0] = downRight;
        downHead.main[0] = down;
        downLeftHead.main[0] = downLeft;
        leftHead.main[0] = left;
        upLeftHead.main[0] = upLeft;*/

    }

    private void Update()
    {
        state = snake.isMouthOpen ? 1 : 0;

        if (transform.eulerAngles.z < 25f || transform.eulerAngles.z > 340f)
            spriteRenderer.sprite = correctArray == 0 ? upHead.main[state] : upHead.lowLight[state];

        else if (transform.eulerAngles.z > 295f && transform.eulerAngles.z < 340f)
            spriteRenderer.sprite = correctArray == 0 ? upRightHead.main[state] : upRightHead.lowLight[state];

        else if (transform.eulerAngles.z > 250f && transform.eulerAngles.z < 295f)
            spriteRenderer.sprite = correctArray == 0 ? upRightHead.main[state]: upRightHead.lowLight[state];

        else if (transform.eulerAngles.z > 205f && transform.eulerAngles.z < 250f)
            spriteRenderer.sprite = correctArray == 0 ? rightHead.main[state]: rightHead.lowLight[state];

        else if (transform.eulerAngles.z > 160f && transform.eulerAngles.z < 205f)
            spriteRenderer.sprite = correctArray == 0 ? downRightHead.main[state]: downRightHead.lowLight[state];

        else if (transform.eulerAngles.z > 115f && transform.eulerAngles.z < 160f)
            spriteRenderer.sprite = correctArray == 0 ? downHead.main[state] : downHead.lowLight[state];

        else if (transform.eulerAngles.z > 70f && transform.eulerAngles.z < 115f)
            spriteRenderer.sprite = correctArray == 0 ? downLeftHead.main[state] : downLeftHead.lowLight[state];

        else if (transform.eulerAngles.z > 25f && transform.eulerAngles.z < 70f)
            spriteRenderer.sprite = correctArray == 0 ? leftHead.main[state] : leftHead.lowLight[state];

        else
            spriteRenderer.sprite = correctArray == 0 ? upLeftHead.main[state] : upLeftHead.lowLight[state];
    }

    private void UpdateLighting()
    {
        if (PixelLighting.Instance != null)
        {
            lightLevel = PixelLighting.Instance.GetTileLightLevel(Vector3Int.RoundToInt(transform.position));

            if (lightLevel == 0)
                spriteRenderer.enabled = false;
            else
            {
                spriteRenderer.enabled = true;

                if (lightLevel < 4 && lightLevel > 0)
                    correctArray = 1;
                else
                    correctArray = 0;
            }
        }
    }

    public void TurnOffLight()
    {
        correctArray = 0;
        CancelInvoke();
    }

    public void TurnOnLight()
    {
        correctArray = 0;
        InvokeRepeating("UpdateLighting", 0f, 0.15f);
    }

}
