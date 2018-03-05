using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using EZCameraShake;

public class PointManager : MonoBehaviour {

    public static PointManager Instance { set; get; }

    public Text pointsText;
    public Text wordsText;
    public string[] wordsToShow;
    public int currentPoints;

    private Animation textPointsAnim;
    private Animation wordsTextAnim;
    [HideInInspector]
    public GradientBackground gradientBackground;

    private void Awake()
    {
        Instance = this;
        textPointsAnim = pointsText.GetComponentInParent<Animation>();
        wordsTextAnim = wordsText.GetComponent<Animation>();
        gradientBackground = GetComponent<GradientBackground>();
    }

    public void SetPoints(int points)
    {
        SoundManager.Instance.PlayBallClip(SoundManager.Instance.impactClip);

        currentPoints += points;
        PlayAnimPoints();

        wordsText.text = wordsToShow[Random.Range(0, wordsToShow.Length)];
        wordsTextAnim.Play();

        if (currentPoints % 5 == 0)
        {
            CameraShaker.Instance.ShakeOnce(3f, 4f, .5f, 1f);
            SoundManager.Instance.PlayEffectClip(SoundManager.Instance.achievementClip);
            gradientBackground.SelectColor();
            gradientBackground.StageParticles();
            SoundManager.Instance.IncreasePitch();
            Spawner.Instance.moveSpeed += .5f;
            DrawLineMaps.Instance.StartCoroutine(DrawLineMaps.Instance.EraseLine());
        }
    }

    public void PlayAnimPoints()
    {
        textPointsAnim.Play();
        pointsText.text = currentPoints.ToString();
    }
}
