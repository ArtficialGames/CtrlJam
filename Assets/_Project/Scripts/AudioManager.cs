using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public AudioSource musicSource;
    float musicVolume = 0.5f;
    float sfxVolume = 0.5f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            if(this.musicSource.clip == Instance.musicSource.clip)
                Destroy(gameObject);
            else
            {
                Destroy(Instance.gameObject);
                Instance = this;
            }
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