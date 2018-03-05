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
    public ParticleSystem topPS;
    public ParticleSystem bottomPS;
    public int currentPoints;

    private Animation textPointsAnim;
    private Animation wordsTextAnim;
    private GradientBackground gradientBackground;

    private void Awake()
    {
        Instance = this;
        textPointsAnim = pointsText.GetComponentInParent<Animation>();
        wordsTextAnim = wordsText.GetComponent<Animation>();
        gradientBackground = GetComponent<GradientBackground>();
        topPS.transform.position = new Vector2(0, Spawner.Instance.stageDimensions.y);
        bottomPS.transform.position = new Vector2(0, -Spawner.Instance.stageDimensions.y);
    }

    public void SetPoints(int points, Color color, Transform pos)
    {
        SoundManager.Instance.PlayBallClip(SoundManager.Instance.impactClip);
        Spawner.Instance.explisonPS.transform.position = pos.position;
        var main = Spawner.Instance.explisonPS.main;
        main.startColor = color;
        Spawner.Instance.explisonPS.Play();

        currentPoints += points;
        textPointsAnim.Play();
        pointsText.text = currentPoints.ToString();

        wordsText.text = wordsToShow[Random.Range(0, wordsToShow.Length)];
        wordsTextAnim.Play();

        if (currentPoints % 5 == 0)
        {
            CameraShaker.Instance.ShakeOnce(3f, 4f, .5f, 1f);
            SoundManager.Instance.PlayEffectClip(SoundManager.Instance.achievementClip);
            gradientBackground.SelectColor();
            StageParticles();
            SoundManager.Instance.IncreasePitch();
            Spawner.Instance.moveSpeed += .5f;
            DrawLineMaps.Instance.StartCoroutine(DrawLineMaps.Instance.EraseLine());
        }
    }

    private void StageParticles()
    {
        var mainTop = topPS.main;
        var mainBottom = bottomPS.main;
        if (mainTop.startLifetime.constant < 1.85f && mainBottom.startLifetime.constant < 1.85f)
        {
            mainTop.startLifetime = mainTop.startLifetime.constant + 0.37f;
            mainBottom.startLifetime = mainBottom.startLifetime.constant + 0.37f;

            if (!topPS.isPlaying && !bottomPS.isPlaying)
            {
                topPS.Play(); 
                bottomPS.Play();
            }
        }
        gradientBackground.SetTexture();
    }
}
