using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class PauseMenuController : MonoBehaviour
{
    private static bool isGamePause = false;

    public GameObject pauseMenuUI;

    private int menuBuildIndex = 0;

    public static bool IsGamePause { get => isGamePause; set => isGamePause = value; }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) 
        {
            if (IsGamePause) 
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
        Debug.Log("Resuming game...");
        // hide the pause menu
        pauseMenuUI.SetActive(false);

        // set the time back to normal
        Time.timeScale = 1f;

        IsGamePause = false;
    }

    void Pause() 
    {
        Debug.Log("Pausing game...");
        // display the pause menu
        pauseMenuUI.SetActive(true);
        
        // freeze the game time
        Time.timeScale = 0f;

        IsGamePause = true;
    }

    public void LoadMenu() 
    {
        Debug.Log("Loading Menu...");
        // restart time
        Time.timeScale = 1f;
        PauseMenuController.IsGamePause = false;
        SceneManager.LoadScene(menuBuildIndex);
    }

    public void QuitGame() 
    {
        Debug.Log("Quitting game...");
        Application.Quit();
    }
}
