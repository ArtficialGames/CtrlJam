using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    [SerializeField] AudioSource musicSource;
    float musicVolume = 0.5f;
    float sfxVolume = 0.5f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        DontDestroyOnLoad(gameObject);
    }

    public void ChangeMusicVolume(float value)
    {
        musicVolume = value;
        musicSource.volume = musicVolume;
    }
    public void ChangeSFXVolume(float value)
    {
        sfxVolume = value;
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }

    public float GetSFXVolume()
    {
        return sfxVolume;
    }
}