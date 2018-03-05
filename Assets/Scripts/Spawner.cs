using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    
    public static Spawner Instance { set; get; }

    public Transform dotsObjetiveHolder;
    public Transform ballsHolder;
    public ParticleSystem explosionPS;

    public float moveSpeed = 6f;
    public bool isBallSpawning = false;
    public Vector2 stageDimensions;

    [HideInInspector]
    public GameObject currentBallToSpawn;

    private void Awake()
    {
        Instance = this;
        stageDimensions = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
    }

    public void SpawnDot(Transform currentDot)
    {
        currentDot.position = new Vector2(Random.Range(-stageDimensions.x + 0.30f, stageDimensions.x - 0.30f),
                                          Random.Range(-stageDimensions.y + 0.30f, stageDimensions.y - 0.30f));

        for (int i = 0; i < DrawLineMaps.Instance.line.positionCount - 1; i++)
        {
            if (Vector2.Distance(currentDot.position, DrawLineMaps.Instance.line.GetPosition(i)) < 0.5f)
                SpawnDot(currentDot);
        }

        foreach (Transform dot in dotsObjetiveHolder)
        {
            if (dot.gameObject.name != currentDot.name)
            {
                if (Vector2.Distance(currentDot.position, dot.position) < 2)
                    SpawnDot(currentDot);
            }
        }
    }

    public void InitSpawnOfDots()
    {
        foreach (Transform dot in dotsObjetiveHolder)
        {
            dot.GetComponent<Animator>().SetTrigger("Disable");
        }
    }

    public void InitSpawnOfBalls()
    {
        foreach (Transform ball in ballsHolder)
        {
            ball.GetComponent<Animator>().SetTrigger("Disable");
        }
    }

    public void SpawnExplosionPS(Color color, Transform pos)
    {
        explosionPS.transform.position = pos.position;
        var main = explosionPS.main;
        main.startColor = color;
        explosionPS.Play();
    }
}
