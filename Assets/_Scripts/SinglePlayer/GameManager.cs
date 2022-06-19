using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public GameObject finishPanelWin;
    public GameObject finishPanelLose;

    private void Awake()
    {
        instance = this;
    }

    public void Restart()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void GameOver(bool win)
    {
        if (win)
        {
            finishPanelWin.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            finishPanelLose.SetActive(true);
            Time.timeScale = 0f;
        }
    }
    public void Back()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }
}
