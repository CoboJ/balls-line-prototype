using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootScript : MonoBehaviour
{

    public static ShootScript Instance { set; get; }

    public Transform ballsHolder;
    public GameObject aiming;

    private Animation aimingAnim;

    private void Awake()
    {
        Instance = this;
        aimingAnim = aiming.GetComponent<Animation>();
    }

    public void ShootBall()
    {
        if (!DrawLineMaps.Instance.isDrawing && GameOverManager.Instance.isPlaying)
        {
            SoundManager.Instance.PlayBallClip(SoundManager.Instance.shootClip);
            ballsHolder.GetChild(0).GetComponent<BallMove>().Shoot();

            ChangeBall();
        }
    }

    public void ChangeBall()
    {
        ballsHolder.GetChild(0).SetAsLastSibling();

        aiming.transform.position = ballsHolder.GetChild(0).position;
        aiming.transform.rotation = ballsHolder.GetChild(0).rotation;
        aiming.transform.SetParent(ballsHolder.GetChild(0).transform);
        aimingAnim.Play();

    }
}
