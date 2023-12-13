using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager _instance;
    public static AudioManager Instance { get { return _instance; } }
    public float musicVolume;
    public float sfxVolume;
    public AudioSource musicAudioSource;
    [SerializeField] AudioSource chaseAudioSource;

    public bool isChase;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            if (this.musicAudioSource.clip != _instance.musicAudioSource.clip)
            {
                _instance.musicAudioSource.clip = this.musicAudioSource.clip;
                _instance.musicAudioSource.loop = this.musicAudioSource.loop;
                _instance.musicAudioSource.Play();
            }

            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        DontDestroyOnLoad(gameObject);

        sfxVolume = PlayerPrefs.GetFloat("SFX_Volume", 0.5f);
        musicVolume = PlayerPrefs.GetFloat("Music_Volume", 0.5f);
        musicAudioSource.volume = musicVolume;
    }

    public AudioSource PlaySFX(AudioClip SFX, Vector3 position, bool loop = false)
    {
        GameObject tempGO = new GameObject("TempAudio");
        tempGO.tag = "SFX";
        tempGO.transform.position = position;
        AudioSource aSource = tempGO.AddComponent(typeof(AudioSource)) as AudioSource;
        aSource.clip = SFX;
        aSource.volume = sfxVolume;

        aSource.spatialBlend = 1f;

        aSource.Play();
        Destroy(tempGO, SFX.length);
        return aSource;
    }

    public AudioSource PlaySFX(AudioClip SFX, float delay, bool loop = false)
    {
        GameObject tempGO = new GameObject("TempAudio");
        tempGO.tag = "SFX";
        tempGO.transform.position = Vector3.zero;
        AudioSource aSource = tempGO.AddComponent(typeof(AudioSource)) as AudioSource;
        aSource.clip = SFX;
        aSource.volume = sfxVolume;
        aSource.loop = loop;

        aSource.spatialBlend = 0f;

        aSource.PlayDelayed(delay);
        Destroy(tempGO, SFX.length + delay);
        return aSource;
    }

    public AudioSource PlaySFX(AudioClip SFX, bool loop = false)
    {
        GameObject tempGO = new GameObject("TempAudio");
        tempGO.tag = "SFX";
        tempGO.transform.position = Vector3.zero;
        AudioSource aSource = tempGO.AddComponent(typeof(AudioSource)) as AudioSource;
        aSource.clip = SFX;
        aSource.volume = sfxVolume * 0.65f;
        aSource.loop = loop;

        aSource.spatialBlend = 0f;

        aSource.Play();
        Destroy(tempGO, SFX.length);
        return aSource;
    }

    public void KillSFXs()
    {
        foreach (GameObject sfx in GameObject.FindGameObjectsWithTag("SFX"))
        {
            Destroy(sfx);
        }
    }

    public void ChangeMusicVolume(float music)
    {
        musicVolume = music;
        musicAudioSource.volume = musicVolume;
        PlayerPrefs.SetFloat("Music_Volume", musicVolume);
    }

    public void ChangeSFXVolume(float sfx)
    {
        sfxVolume = sfx;
        PlayerPrefs.SetFloat("SFX_Volume", sfxVolume);
    }

    public void ChangeMusicClip(AudioClip newMusic, bool loop = true)
    {
        if (musicAudioSource.clip == newMusic)
            return;

        musicAudioSource.loop = loop;
        musicAudioSource.Stop();
        musicAudioSource.clip = newMusic;
        musicAudioSource.Play();
    }

    public void Pause()
    {
        musicAudioSource.Pause();
    }

    public void Resume()
    {
        musicAudioSource.Play();
    }

    /*public void PlayChase()
    {
        chaseAudioSource.volume = PlayerPrefs.GetFloat("Music_Volume", 0.5f);
        musicAudioSource.volume = 0;

        //StartCoroutine(Fade(chaseAudioSource, musicAudioSource));

        isChase = true;
    }

    public void PlayGameplay()
    {
        musicAudioSource.volume = PlayerPrefs.GetFloat("Music_Volume", 0.5f);
        chaseAudioSource.volume = 0;

        //StartCoroutine(Fade(musicAudioSource, chaseAudioSource));

        isChase = false;
    }*/

    IEnumerator Fade(AudioSource on, AudioSource off)
    {
        float currentTime = 0;
        float duration = 0.5f;

        float start = on.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            on.volume = Mathf.Lerp(start, PlayerPrefs.GetFloat("Music_Volume"), currentTime / duration);
            off.volume = Mathf.Lerp(start, 0, currentTime / duration);

            yield return null;
        }

        yield break;
    }

    public AudioClip GetCurrentMusic()
    {
        return musicAudioSource.clip;
    }
}
