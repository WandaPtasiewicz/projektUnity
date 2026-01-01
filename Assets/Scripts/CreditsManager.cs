using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class CreditsManager : MonoBehaviour
{
    public void Update()
    {
       
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
