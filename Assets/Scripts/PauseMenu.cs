using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
   
    public static bool GamePaused = false;

    public GameObject PauseMenuUi;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GamePaused)
            {
                Resume();
            } else
            {
                Pause();
            }


        }
    }


    public void Resume()
    {
        PauseMenuUi.SetActive(false);
        GamePaused = false;
        Time.timeScale = 1f;
    }

    void Pause()
    {
        PauseMenuUi.SetActive(true);
        GamePaused = true;
        Time.timeScale = 0f;
    }

    public float MenuLevel = 0;

    // Update is called once per frame
    
    public void LoadMenu()
    {
        Time.timeScale = 1f;
        //SceneManager.LoadScene("Menu");
        SceneManager.LoadScene(0);
        Debug.Log("Loading");
    }

    public void QuitGame()
    {
        Debug.Log("Quiting");
        Application.Quit();
    }



}
