using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {

    public GameObject mainMenu;
    public Text recordText;

    private void Start()
    {
        recordText.text = PlayerPrefs.GetInt("Record").ToString();
    }

    public void StartToPlay()
    {
        GameOverManager.Instance.isPlaying = true;
        DrawLineMaps.Instance.StartCoroutine(DrawLineMaps.Instance.DrawLine());
        Spawner.Instance.InitSpawnOfDots();
        Spawner.Instance.InitSpawnOfBalls();
        mainMenu.SetActive(false);
    }

    public void MainMenu()
    {
        ResetGameValues();
        DrawLineMaps.Instance.line.positionCount = 0;
        recordText.text = PlayerPrefs.GetInt("Record").ToString();
        GameOverManager.Instance.gameOverMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    public void ContinuePlaying()
    {
        GameOverManager.Instance.isPlaying = true;
        GameOverManager.Instance.gameOverMenu.SetActive(false);
    }

    public void RestartGame()
    {
        ResetGameValues();
        GameOverManager.Instance.isPlaying = true;
        DrawLineMaps.Instance.line.positionCount = 0;
        DrawLineMaps.Instance.StartCoroutine(DrawLineMaps.Instance.DrawLine());
        Spawner.Instance.InitSpawnOfDots();
        Spawner.Instance.InitSpawnOfBalls();
        GameOverManager.Instance.gameOverMenu.SetActive(false);
    }

    private void ResetGameValues()
    {
        SoundManager.Instance.RestartPitch();
        PointManager.Instance.currentPoints = 0;
        PointManager.Instance.PlayAnimPoints();
        PointManager.Instance.gradientBackground.RestartParticles();
        Spawner.Instance.moveSpeed = 6;
    }
}
