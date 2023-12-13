using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    [SerializeField] WinBox winBox;
    [SerializeField] AudioClip snakeDestroySFX;
    [SerializeField] AudioClip winSFX;

    private void Awake()
    {
        Instance = this;
    }

    public void Win()
    {
        StartCoroutine(WinCorroutine());
    }

    public void GameOver()
    {
        StartCoroutine(GameOverCorroutine());
    }

    IEnumerator GameOverCorroutine()
    {
        yield return new WaitForSeconds(1f);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    IEnumerator WinCorroutine()
    {
        AudioManager.Instance.ChangeMusicClip(null);

        GameObject.FindGameObjectWithTag("SnakeHead").GetComponent<SnakeSpriteSetter>().TurnOffLight();

        GameObject.FindGameObjectWithTag("WeakSpot").GetComponent<ApplyLighting>().TurnOff();

        foreach (var item in GameObject.FindGameObjectsWithTag("SnakeBody"))
        {
            item.GetComponent<ApplyLighting>().TurnOff();
        }

        Destroy(GameObject.FindGameObjectWithTag("WeakSpot"));
        AudioManager.Instance.PlaySFX(snakeDestroySFX);
        yield return new WaitForSecondsRealtime(0.1f);

        foreach (var item in GameObject.FindGameObjectsWithTag("SnakeBody"))
        {
            Destroy(item);
            AudioManager.Instance.PlaySFX(snakeDestroySFX);
            yield return new WaitForSecondsRealtime(0.1f);
        }

        Destroy(GameObject.FindGameObjectWithTag("SnakeHead"));
        AudioManager.Instance.PlaySFX(snakeDestroySFX);

        yield return new WaitForSeconds(0.5f);

        int highscore = PlayerPrefs.GetInt("Highscore", 0);
        int current = FindObjectOfType<Queue>().survivors.Count - 1;


        if (current > highscore)
        {
            highscore = current;
            PlayerPrefs.SetInt("Highscore", highscore);
        }

        winBox.gameObject.SetActive(true);
        winBox.Show(FindObjectOfType<Queue>().survivors.Count, highscore);

        AudioManager.Instance.ChangeMusicClip(winSFX, false);
    }
}
