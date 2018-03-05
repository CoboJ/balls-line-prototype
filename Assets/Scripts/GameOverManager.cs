using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour {

    public static GameOverManager Instance { set; get; }

    public int totalAttempts = 3;
    public int currentAttempts;
    public GameObject gameOverMenu;
    public Text scoreText;
    public Text scoreBoardText;

    [HideInInspector]
    public bool isPlaying = false;

    private void Awake()
    {
        Instance = this;
        currentAttempts = totalAttempts;
    }

    public void GameOver()
    {
        isPlaying = false;
        gameOverMenu.SetActive(true);
        currentAttempts = totalAttempts;
        scoreText.text = PointManager.Instance.currentPoints.ToString();
        if (PointManager.Instance.currentPoints > PlayerPrefs.GetInt("Record"))
        {
            PlayerPrefs.SetInt("Record", PointManager.Instance.currentPoints);
            scoreBoardText.text = "New Record!";
        }
        scoreBoardText.text = "Score";
    }
}
