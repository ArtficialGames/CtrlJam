using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class SplashController : MonoBehaviour
{
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] CanvasGroup ctrl;

    private void Awake()
    {
        ctrl.alpha = 0f;
        ctrl.DOFade(1f, 0.5f);
        ctrl.DOFade(0f, 0.5f).SetDelay(2f).OnComplete(() => StartCoroutine(VideoCoroutine()));
    }

    IEnumerator VideoCoroutine()
    {
        videoPlayer.Play();
        yield return new WaitForSeconds((float)videoPlayer.clip.length + 0.25f);
        SceneManager.LoadScene("StartMenu");
    }

}
