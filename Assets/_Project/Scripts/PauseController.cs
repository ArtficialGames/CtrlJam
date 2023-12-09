using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PauseController : MonoBehaviour
{
    [SerializeField] Button resumeBtn;
    [SerializeField] Button ExitBtn;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider musicSlider;

    private void Awake()
    {
        resumeBtn.onClick.AddListener(Resume);
        ExitBtn.onClick.AddListener(Exit);
        sfxSlider.onValueChanged.AddListener(ChangeSFXVolume);
        musicSlider.onValueChanged.AddListener(ChangeMusicVolume);

        sfxSlider.value = PlayerPrefs.GetFloat("SFX_Volume", 0.5f);
        musicSlider.value = PlayerPrefs.GetFloat("Music_Volume", 0.5f);
        Time.timeScale = 0;
    }

    public void Resume()
    {
        SceneManager.UnloadSceneAsync("Pause");
        Time.timeScale = 1;
    }

    public void Exit()
    {
        SceneManager.LoadScene("StartMenu");
        Time.timeScale = 1;
    }

    public void ChangeMusicVolume(float value)
    {
        AudioManager.Instance.ChangeMusicVolume(value);
    }

    public void ChangeSFXVolume(float value)
    {
        AudioManager.Instance.ChangeSFXVolume(value);
    }
}

