using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StartMenuController : MonoBehaviour
{
    [SerializeField] Button playBtn;
    [SerializeField] Button CreditsBtn;
    [SerializeField] Button ExitBtn;
    [SerializeField] Slider sfxSlider;
    [SerializeField] Slider musicSlider;

    private void Awake()
    {
        playBtn.onClick.AddListener(Play);
        ExitBtn.onClick.AddListener(Exit);
        CreditsBtn.onClick.AddListener(Credits);
        sfxSlider.onValueChanged.AddListener(ChangeSFXVolume);
        musicSlider.onValueChanged.AddListener(ChangeMusicVolume);

        sfxSlider.value = PlayerPrefs.GetFloat("SFX_Volume", 0.5f);
        musicSlider.value = PlayerPrefs.GetFloat("Music_Volume", 0.5f);
    }

    public void Play()
    {
        SceneManager.LoadScene("Game");
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void Exit()
    {
        Application.Quit();
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
