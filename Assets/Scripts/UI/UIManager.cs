using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverScreen;
    [SerializeField] private GameObject mainMenuScreen;
    [SerializeField] private AudioClip gameOverSound;
    [SerializeField] private AudioClip mainMenuSound;

    private void Awake()
    {
        if (gameOverScreen != null)
        {
            gameOverScreen.SetActive(false);
        }


    }

    private void Start()
    {
        if (mainMenuScreen != null && mainMenuSound != null && SoundManager.instance != null)
        {
            SoundManager.instance.PlaySound(mainMenuSound);
        }
    }

    public void GameOver()
    {
        gameOverScreen.SetActive(true);
        SoundManager.instance.PlaySound(gameOverSound);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    //Game over functions
    public void Restart()
    {
        //Reload the same level
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
    public void MainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void Quit()
    {
        Application.Quit();
        //Exit play mode
        UnityEditor.EditorApplication.isPlaying = false;
    }

}