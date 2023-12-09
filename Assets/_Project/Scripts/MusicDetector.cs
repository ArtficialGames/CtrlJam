using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicDetector : MonoBehaviour
{
    Leader leader;
    
    [SerializeField] AudioClip gameplayMusic;
    [SerializeField] AudioClip chaseMusic;

    bool canPlayChase = true;
    bool canPlayGameplay = true;

    private void Awake()
    {
        leader = FindObjectOfType<Leader>();
    }

    private void OnBecameVisible()
    {
        if(!AudioManager.Instance.isChase && canPlayChase && AudioManager.Instance.GetCurrentMusic() != chaseMusic)
        {
            StartCoroutine(ChaseCoroutine());
            AudioManager.Instance.ChangeMusicClip(chaseMusic);
        }
    }

    private void Update()
    {
        if(Vector2.Distance(transform.position, leader.transform.position) > 10f && AudioManager.Instance.isChase && canPlayGameplay && AudioManager.Instance.GetCurrentMusic() != gameplayMusic)
        {
            StartCoroutine(GameplayCoroutine());
            AudioManager.Instance.ChangeMusicClip(gameplayMusic);
        }
    }

    IEnumerator ChaseCoroutine()
    {
        canPlayChase = false;
        yield return new WaitForSeconds(2f);
        canPlayChase = true;
    }

    IEnumerator GameplayCoroutine()
    {
        canPlayGameplay = false;
        yield return new WaitForSeconds(2f);
        canPlayGameplay = true;
    }
}
