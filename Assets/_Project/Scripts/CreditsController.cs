using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CreditsController : MonoBehaviour
{
    [SerializeField] Button backBtn;

    private void Awake()
    {
        backBtn.onClick.AddListener(Back);
    }

    public void Back()
    {
        SceneManager.LoadScene(0);
    }
}
