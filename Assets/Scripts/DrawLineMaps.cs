using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DrawLineMaps : MonoBehaviour {

    public static DrawLineMaps Instance { set; get; }

    public float timeOfDraw = 0.1f;

    [System.Serializable]
    public class LineMap
    {
        public List<Vector2> lineCords;
    }

    public LineMap[] lineMaps;

    [HideInInspector]
    public bool isDrawing = false;
    [HideInInspector]
    public LineRenderer line;

    private void Awake()
    {
        Instance = this;
        line = GetComponent<LineRenderer>();
    }

    public IEnumerator DrawLine()
    {
        isDrawing = true;
        int mapRandom = Random.Range(0, lineMaps.Length - 1);
        int nPos = 0;
        while(line.positionCount < lineMaps[mapRandom].lineCords.Count)
        {
            line.positionCount = nPos + 1;
            line.SetPosition(nPos, lineMaps[mapRandom].lineCords[nPos]);

            nPos++;

            yield return null;
        }
        isDrawing = false;
        StopCoroutine(DrawLine());
    }

    public IEnumerator EraseLine()
    {
        isDrawing = true;
        Spawner.Instance.InitSpawnOfDots();
        while (line.positionCount > 0)
        {
            line.positionCount--;

            yield return null;
        }
        if (GameOverManager.Instance.isPlaying)
            StartCoroutine(DrawLine());
        StopCoroutine(EraseLine());
    }
}
