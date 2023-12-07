using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] Image survivors_IMG;
    [SerializeField] Sprite survivorsNormal_Sprite;
    [SerializeField] Sprite survivorsLose_Sprite;
    [SerializeField] Sprite survivorsAdd_Sprite;

    [SerializeField] TMP_Text survivorCountCurrent;
    [SerializeField] TMP_Text survivorCountMax;
    [SerializeField] TMP_Text torchAmount;

    public void UpdateSurvivorsCount(int current, int max)
    {
        if (Int32.Parse(survivorCountCurrent.text) < current)
            StartCoroutine(ChangeSpriteCoroutine(survivorsAdd_Sprite));

        survivorCountCurrent.text = current.ToString("00");
        survivorCountMax.text = max.ToString("00");
    }

    public void UpdateTorchAmount(int amount)
    {
        torchAmount.text = amount + "%";
    }

    public void PlayHUDAnimation()
    {
        StartCoroutine(ChangeSpriteCoroutine(survivorsLose_Sprite));
    }

    IEnumerator ChangeSpriteCoroutine(Sprite newSprite)
    {
        survivors_IMG.sprite = newSprite;
        yield return new WaitForSeconds(0.75f);
        survivors_IMG.sprite = survivorsNormal_Sprite;
    }

}
