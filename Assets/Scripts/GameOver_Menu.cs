using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver_Menu : MonoBehaviour
{

    public float MenuLevel = 0;

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

    }

    public void LoadMenu()
    {
        Debug.Log("Loading");
        SceneManager.LoadScene(0);
    }

}