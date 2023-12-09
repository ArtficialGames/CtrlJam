using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinBox : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI currentTMP;
    [SerializeField] TextMeshProUGUI highscoreTMP;

    public void Show(int current, int highscore)
    {
        currentTMP.text += current;
        highscoreTMP.text += highscore;
        Time.timeScale = 0f;
    }

    public void Back()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("StartMenu");
    }
}
