using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMove : MonoBehaviour {

    public float speedRotation = 200f;
    public float jumpForce = 10f;
    public GameObject nextBall;

    private int cordsNumber = 0;
    private bool isShooting = false;
    private Rigidbody2D myRigidbody;
    private SpriteRenderer spriteRenderer;
    private Animator myAnimator;
    private CircleCollider2D myCollider;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        myAnimator = GetComponent<Animator>();
        myCollider = GetComponent<CircleCollider2D>();
        speedRotation = Random.Range(150f, 300f);
    }

    public void Shoot()
    {
        if(cordsNumber != DrawLineMaps.Instance.line.positionCount - 1 && isShooting != true && spriteRenderer.enabled == true)
        {
            isShooting = true;
            myRigidbody.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
        }
    }

    private void Update()
    {
        if (DrawLineMaps.Instance.isDrawing == false && isShooting == false && cordsNumber < DrawLineMaps.Instance.line.positionCount - 1 && GameOverManager.Instance.isPlaying)
        {
            if (Vector2.Distance(transform.position, DrawLineMaps.Instance.line.GetPosition(cordsNumber)) < 0.2f)
                cordsNumber++;
            
            transform.position = Vector2.Lerp(transform.position, DrawLineMaps.Instance.line.GetPosition(cordsNumber), Time.deltaTime * Spawner.Instance.moveSpeed);

            if (transform.childCount > 0)
                transform.Rotate(Vector3.forward * 1 * speedRotation * Time.deltaTime);
        }

        if(cordsNumber == DrawLineMaps.Instance.line.positionCount - 1 && myAnimator.GetCurrentAnimatorStateInfo(0).IsName("GetIn") && DrawLineMaps.Instance.isDrawing == false)
        {
            ShootScript.Instance.ChangeBall();
            myAnimator.SetTrigger("Disable");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("DotObjetive") && isShooting == true && collision.transform.GetComponent<SpriteRenderer>().color == spriteRenderer.color)
        {
            PointManager.Instance.SetPoints(1);
            Spawner.Instance.SpawnExplosionPS(spriteRenderer.color, collision.transform);
            collision.GetComponent<ObjetiveDots>().DisableDot();
            spriteRenderer.enabled = false;
        }
    }

    private void OnBecameInvisible()
    {
        if (isShooting == true)
        {
            if (spriteRenderer.enabled == true)
                GameOverManager.Instance.currentAttempts--;
            
            if(GameOverManager.Instance.currentAttempts <= 0)
                GameOverManager.Instance.GameOver();
            
            myRigidbody.velocity = Vector2.zero;
            DisableDot();
        }
    }

    public void DisableDot()
    {
        spriteRenderer.enabled = false;
        myCollider.enabled = false;

        StartCoroutine(ReinitializeBall());
    }

    private IEnumerator ReinitializeBall()
    {
        if (DrawLineMaps.Instance.isDrawing)
            yield return new WaitUntil(() => !DrawLineMaps.Instance.isDrawing);

        yield return new WaitUntil(() => !Spawner.Instance.isBallSpawning && Spawner.Instance.currentBallToSpawn == gameObject);

        Spawner.Instance.currentBallToSpawn = nextBall;
        Spawner.Instance.isBallSpawning = true;

        if (!spriteRenderer.enabled)
        {
            cordsNumber = 0;
            speedRotation = Random.Range(150f, 300f);
            transform.position = DrawLineMaps.Instance.line.GetPosition(cordsNumber);
            spriteRenderer.enabled = true;
            myCollider.enabled = true;
            isShooting = false;
        }
        myAnimator.Play("GetIn", -1, 0f);

        if (!GameOverManager.Instance.isPlaying)
            yield return new WaitUntil(() => GameOverManager.Instance.isPlaying);

        yield return new WaitForSeconds(Random.Range(1f, 3f));

        Spawner.Instance.isBallSpawning = false;
        StopCoroutine(ReinitializeBall());
    }
}
