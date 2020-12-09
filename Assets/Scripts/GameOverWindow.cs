using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverWindow : MonoBehaviour {

    private Text scoreText;

    private void Awake() {
        scoreText = transform.Find("scoreText").GetComponent<Text>();
        Debug.Log("GameOverWindow.Start");
        
    }

    private void Start() {
        Bird.GetInstance().OnDied += Bird_OnDied;
        Hide();
    }

    private void Bird_OnDied(object sender, System.EventArgs e) {
        scoreText.text = Level.GetInstance().score.ToString();
        Show();
    }

    private void Hide()
    {
        Debug.Log("Escondi");
        gameObject.SetActive(false);
    }

    private void Show()
    {
        Debug.Log("Mostrei");
        gameObject.SetActive(true);
    }

}
