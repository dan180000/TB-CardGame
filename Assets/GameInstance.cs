using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameInstance : MonoBehaviour
{
    public void RestartScene()
    {
        SceneManager.LoadScene(Application.loadedLevel);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
