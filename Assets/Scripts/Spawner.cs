using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {
    
    public static Spawner Instance { set; get; }

    public Transform dotsObjetiveHolder;
    public Transform ballsHolder;
    public ParticleSystem explisonPS;

    public float moveSpeed = 5f;
    public Vector2 stageDimensions;

    private void Awake()
    {
        Instance = this;
        stageDimensions = Camera.main.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height)); 
    }

    private void Start()
    {
        InitRespawnOfDots();
        StartCoroutine(InitSpanwBalls());
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

    public void InitRespawnOfDots()
    {
        foreach (Transform dot in dotsObjetiveHolder)
        {
            dot.GetComponent<Animator>().SetTrigger("Disable");
        }
    }

    IEnumerator InitSpanwBalls()
    {
        yield return new WaitUntil(() => !DrawLineMaps.Instance.isDrawing);

        foreach (Transform ball in ballsHolder)
        {
            ball.gameObject.SetActive(true);

            yield return new WaitForSeconds(1f);
        }

        StopCoroutine(InitSpanwBalls());
    }
}
