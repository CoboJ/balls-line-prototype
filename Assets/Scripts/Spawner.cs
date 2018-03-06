using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    
    public static Spawner Instance { set; get; }

    public Transform dotsObjetiveHolder;
    public Transform ballsHolder;
    public ParticleSystem explosionPS;

    public float moveSpeed = 6f;
    public Vector2 stageDimensions;

    public GameObject currentBallToSpawn;
    public List<GameObject> balls;

    private void Awake()
    {
        Instance = this;
        stageDimensions = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));
        foreach (Transform ball in ballsHolder)
        {
            balls.Add(ball.gameObject);
        }
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

    public IEnumerator ReinitializeBall(GameObject ball, GameObject nextBall)
    {
        yield return new WaitUntil(() => currentBallToSpawn == ball);

        yield return new WaitForSeconds(2f);

        if (DrawLineMaps.Instance.isDrawing)
            yield return new WaitUntil(() => !DrawLineMaps.Instance.isDrawing);
        
        if(!GameOverManager.Instance.isPlaying)
            yield return new WaitUntil(() => GameOverManager.Instance.isPlaying);

        if (!ball.activeInHierarchy)
            ball.SetActive(true);

        currentBallToSpawn = nextBall;

        StopCoroutine(ReinitializeBall(ball, nextBall));
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
        StopAllCoroutines();
        currentBallToSpawn = balls[0].gameObject;
        for (int i = 0; i < balls.Count; i++)
        {
            balls[i].transform.SetSiblingIndex(i);

            if (!balls[i].gameObject.activeInHierarchy)
                balls[i].gameObject.SetActive(true);

            balls[i].GetComponent<Animator>().SetTrigger("Disable");
        }
        ShootScript.Instance.SetAiming();
    }

    public void SpawnExplosionPS(Color color, Transform pos)
    {
        explosionPS.transform.position = pos.position;
        var main = explosionPS.main;
        main.startColor = color;
        explosionPS.Play();
    }
}
