using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static GameController Instance;
    [SerializeField] WinBox winBox;

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
        yield return new WaitForSeconds(1f);

        int highscore = PlayerPrefs.GetInt("Highscore", 0);
        int current = FindObjectOfType<Queue>().survivors.Count - 1;


        if (current > highscore)
        {
            highscore = current;
            PlayerPrefs.SetInt("Highscore", highscore);
        }

        winBox.gameObject.SetActive(true);
        winBox.Show(FindObjectOfType<Queue>().survivors.Count, highscore);
    }
}
