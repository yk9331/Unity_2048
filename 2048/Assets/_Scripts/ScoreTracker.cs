using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreTracker : MonoBehaviour {

    private int score = 0;
    public static ScoreTracker _instance;
    public Text highScoreText;
    public Text scoreText;

    public int Score {

        get {
            return score;
        }
        set {
            score = value;
            scoreText.text = score.ToString();

            if(PlayerPrefs.GetInt("HighScore")< score) {
                PlayerPrefs.SetInt("HighScore", score);
                highScoreText.text = score.ToString();
            }
        }
    }

    private void Awake() {
        _instance = this;
        //PlayerPrefs.DeleteAll();
        if (!PlayerPrefs.HasKey("HighScore")) {
            PlayerPrefs.SetInt("HighScore", 0);
        }
        scoreText.text = score.ToString();
        highScoreText.text = PlayerPrefs.GetInt("HighScore").ToString();
    }
}
