using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class WinManager : MonoBehaviour
{
    public void Update()
    {
       
    }

    public void Credits()
    {
        Debug.Log("credits");
        SceneManager.LoadScene(2);
    }

    public void RestartGame()
    {
        Debug.Log("start");
        SceneManager.LoadScene(0);
    }

    public void MainMenu()
    {
        Debug.Log("menu");
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Debug.Log("exit");
        Application.Quit();
    }
}
