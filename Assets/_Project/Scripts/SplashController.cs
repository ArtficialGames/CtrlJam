using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEditor;

public class SplashController : MonoBehaviour
{
    [SerializeField] VideoPlayer videoPlayer;
    [SerializeField] CanvasGroup cisteto;
    [SerializeField] CanvasGroup ctrl;

    private void Awake()
    {
#if UNITY_WEBGL

        videoPlayer = null;

#endif

        ctrl.alpha = 0f;
        ctrl.DOFade(1f, 0.5f);
        ctrl.DOFade(0f, 0.5f).SetDelay(2f).OnComplete(() => Cisteto());
        Application.targetFrameRate = 60;
    }

    void Cisteto()
    {
        if(videoPlayer != null)
            StartCoroutine(VideoCoroutine());
        else
        {
            cisteto.DOFade(1f, 0.5f);
            cisteto.DOFade(0f, 0.5f).SetDelay(3f).OnComplete(() => SceneManager.LoadScene("StartMenu"));
        }
    }

    IEnumerator VideoCoroutine()
    {
        videoPlayer.Play();
        yield return new WaitForSeconds((float)videoPlayer.clip.length + 0.25f);
        SceneManager.LoadScene("StartMenu");
    }

}
