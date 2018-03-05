using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjetiveDots : MonoBehaviour {

    SpriteRenderer spriteRenderer;
    Animator myAnimator;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        myAnimator = GetComponent<Animator>();
    }

    public void DisableDot()
    {
        spriteRenderer.enabled = false;

        StartCoroutine(ReinitializeDot());
    }

    private IEnumerator ReinitializeDot()
    {
        if (DrawLineMaps.Instance.isDrawing)
            yield return new WaitUntil(() => !DrawLineMaps.Instance.isDrawing);

        yield return new WaitForSeconds(1);
        
        if (!spriteRenderer.enabled)
        {
            Spawner.Instance.SpawnDot(transform);
            spriteRenderer.enabled = true;
        }
        myAnimator.Play("GetIn", -1, 0f);

        StopCoroutine(ReinitializeDot());
    }
}
