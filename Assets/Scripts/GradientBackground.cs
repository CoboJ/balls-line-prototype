using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GradientBackground : MonoBehaviour {

    public Transform quadBackground;
    public Material gradientMat;
    public Material particleEffectMat;
    public Color endColorBottom;
    public Color endColorTop;
    public float speedToChangeColor = 5f;
    public ParticleSystem topPS;
    public ParticleSystem bottomPS;
    public Texture[] polygonTexturesToParticles;

    [Header("Gradient Colors 1")]
    public Color bottomColor_1;
    public Color topColor_1;

    [Header("Gradient Colors 2")]
    public Color bottomColor_2;
    public Color topColor_2;

    [Header("Gradient Colors 3")]
    public Color bottomColor_3;
    public Color topColor_3;

    [Header("Gradient Colors 4")]
    public Color bottomColor_4;
    public Color topColor_4;

    [Header("Gradient Colors 5")]
    public Color bottomColor_5;
    public Color topColor_5;

    private void Start()
    {
        quadBackground.localScale = new Vector2(Spawner.Instance.stageDimensions.x + .5f, Spawner.Instance.stageDimensions.y) * 2;
        topPS.transform.position = new Vector2(0, Spawner.Instance.stageDimensions.y);
        bottomPS.transform.position = new Vector2(0, -Spawner.Instance.stageDimensions.y);
        SelectColor();
    }

    private void Update()
    {
        if(gradientMat.GetColor("_Color") != endColorBottom && gradientMat.GetColor("_Color2") != endColorTop)
        {
            gradientMat.SetColor("_Color", Color.Lerp(gradientMat.GetColor("_Color"), endColorBottom, Time.deltaTime * speedToChangeColor)); // the bottom color
            gradientMat.SetColor("_Color2", Color.Lerp(gradientMat.GetColor("_Color2"), endColorTop, Time.deltaTime * speedToChangeColor)); // the top color
        }
    }

    public void StageParticles()
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
        SetTexture();
    }

    public void RestartParticles()
    {
        var mainTop = topPS.main;
        var mainBottom = bottomPS.main;
        mainTop.startLifetime = 0f;
        mainBottom.startLifetime = 0f;

        topPS.Stop();
        bottomPS.Stop();
        SelectColor();
    }

    public void SelectColor()
    {
        int nColor = Random.Range(1, 6);
        switch (nColor)
        {
            case 5:
                if (endColorTop != topColor_5 && endColorBottom != bottomColor_5)
                {
                    endColorBottom = bottomColor_5;
                    endColorTop = topColor_5;
                }
                else
                    SelectColor();
                break;
            case 4:
                if (endColorTop != topColor_4 && endColorBottom != bottomColor_4)
                {
                    endColorBottom = bottomColor_4;
                    endColorTop = topColor_4;
                }
                else
                    SelectColor();
                break;
            case 3:
                if (endColorTop != topColor_3 && endColorBottom != bottomColor_3)
                {
                    endColorBottom = bottomColor_3;
                    endColorTop = topColor_3;
                }
                else
                    SelectColor();
                break;
            case 2:
                if (endColorTop != topColor_2 && endColorBottom != bottomColor_2)
                {
                    endColorBottom = bottomColor_2;
                    endColorTop = topColor_2;
                }
                else
                    SelectColor();
                break;
            case 1:
                if (endColorTop != topColor_1 && endColorBottom != bottomColor_1)
                {
                    endColorBottom = bottomColor_1;
                    endColorTop = topColor_1;
                }
                else
                    SelectColor();
                break;
            default:
                print("Incorrect color level.");
                break;
        }
    }

    public void SetTexture()
    {
        particleEffectMat.mainTexture = polygonTexturesToParticles[Random.Range(0, polygonTexturesToParticles.Length)];
    }
}
