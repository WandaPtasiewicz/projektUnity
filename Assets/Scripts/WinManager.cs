using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class WinManager : MonoBehaviour
{
    public GameObject exitButton;
    public void Awake()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
    exitButton.SetActive(false);
#endif
    }
    public void Credits()
    {
        SceneManager.LoadScene(2);
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(1);
        foreach (var go in FindObjectsOfType<GameObject>())
        {
            if (go.scene.name == null) Destroy(go);
        }
    }

    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
