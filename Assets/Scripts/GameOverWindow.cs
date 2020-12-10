using UnityEngine;
using UnityEngine.UI;

public class GameOverWindow : MonoBehaviour {

    private Text scoreText;
    private Text highscoreText;

    private void Awake() {
        scoreText = transform.Find("ScoreText").GetComponent<Text>();
        highscoreText = transform.Find("HSText").GetComponent<Text>();
        
    }

    private void Start() {
        Bird.GetInstance().OnDied += Bird_OnDied;

        if (Level.GetInstance().GetScore() > Score.GetHighscore())
        {
            // New Highscore
            highscoreText.text = "New Highscore!";
        }
        else
        {
            highscoreText.text = "Highscore: " + Score.GetHighscore();
        }

        Hide();
    }

    private void Bird_OnDied(object sender, System.EventArgs e) {
        scoreText.text = Level.GetInstance().score.ToString();
        Show();
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }

}
