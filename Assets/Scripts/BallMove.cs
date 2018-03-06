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
    private Animator myAnimator;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void Shoot()
    {
        if(cordsNumber != DrawLineMaps.Instance.line.positionCount - 1 && isShooting != true && gameObject.activeInHierarchy)
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
            gameObject.SetActive(false);
        }
    }

    private void OnBecameInvisible()
    {
        if (isShooting == true)
        {
            if (gameObject.activeInHierarchy)
                GameOverManager.Instance.currentAttempts--;
            
            if(GameOverManager.Instance.currentAttempts <= 0)
                GameOverManager.Instance.GameOver();
            
            myRigidbody.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

    private void OnDisable()
    {
        if (Spawner.Instance != null)
            Spawner.Instance.StartCoroutine(Spawner.Instance.ReinitializeBall(gameObject, nextBall));
    }

    private void OnEnable()
    {
        cordsNumber = 0;
        speedRotation = Random.Range(150f, 300f);
        if (DrawLineMaps.Instance.line.positionCount != 0)
            transform.position = DrawLineMaps.Instance.line.GetPosition(cordsNumber);
        isShooting = false;
        myAnimator.Play("GetIn", -1, 0f);
    }

    public void DisableDot()
    {
        gameObject.SetActive(false);
    }
}