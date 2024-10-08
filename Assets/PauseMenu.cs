using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false;
    public GameObject pauseMenuUI;

    void Update()
    {
        

        // Ensure Player.Instance is accessible
            if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (gameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        gameIsPaused = false;

        // Lock and hide the cursor when resuming the game
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        SoundManager.Instance.ResumeAllAudio();
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        gameIsPaused = true;

        // Unlock and show the cursor when the game is paused
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        SoundManager.Instance.PauseAllAudio();
    }

    public void LoadMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void Quit()
    {
        Application.Quit();
    }
}
