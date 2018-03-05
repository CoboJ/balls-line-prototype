using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {

    public static SoundManager Instance { set; get; }

    public AudioSource ballsAS, musicAS, effectsAS;

    public AudioClip impactClip, shootClip, achievementClip;

    private void Awake()
    {
        Instance = this;
    }

    public void PlayBallClip(AudioClip clip)
    {
        ballsAS.PlayOneShot(clip);
    }

    public void PlayEffectClip(AudioClip clip)
    {
        effectsAS.PlayOneShot(clip);
    }

    public void IncreasePitch()
    {
        if (PointManager.Instance.currentPoints < 55)
            musicAS.pitch += 0.05f;
    }

    public void RestartPitch()
    {
        musicAS.pitch = 0.8f;
    }
}
