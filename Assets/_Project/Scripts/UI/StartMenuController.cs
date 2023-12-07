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
        sfxSlider.onValueChanged.AddListener(ChangeSFXVolume);
        musicSlider.onValueChanged.AddListener(ChangeMusicVolume);
        sfxSlider.value = AudioManager.Instance.GetSFXVolume();
        musicSlider.value = AudioManager.Instance.GetMusicVolume();
    }

    public void Play()
    {
        SceneManager.LoadScene(1);
    }

    public void Credits()
    {
        //SceneManager.LoadScene("Credits");
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
