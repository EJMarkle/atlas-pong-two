using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Ball : MonoBehaviour
{
    public float initialForce = 5f;
    public float resetDelay = 2f;
    private Rigidbody2D rb;
    private Vector2 startPosition;
    public GameManager gameManager;
    private Image ballImage;
    public float ballAcceleration = 0.1f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        ballImage = GetComponent<Image>();
        InitialMovement();
    }

    void Update()
    {
        if (rb.velocity != Vector2.zero)
        {
            rb.velocity += rb.velocity.normalized * ballAcceleration * Time.deltaTime;
        }
    }

    void InitialMovement()
    {
        Vector2[] directions = new Vector2[]
        {
            new Vector2(1, 1).normalized,
            new Vector2(1, -1).normalized,
            new Vector2(-1, 1).normalized,
            new Vector2(-1, -1).normalized
        };

        int randomIndex = Random.Range(0, directions.Length);
        Vector2 initialDirection = directions[randomIndex];

        rb.AddForce(initialDirection * initialForce, ForceMode2D.Impulse);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameManager == null || gameManager.GameEnded) return;

        if (collision.gameObject.CompareTag("Paddle"))
        {
            HandlePaddleCollision(collision);
            Debug.Log("Paddle collision detected");
        }
        else if (collision.gameObject.CompareTag("LeftGoal"))
        {
            gameManager.ScorePoint(false);
            StartCoroutine(ResetAfterDelay());
        }
        else if (collision.gameObject.CompareTag("RightGoal"))
        {
            gameManager.ScorePoint(true);
            StartCoroutine(ResetAfterDelay());
        }
        else if (collision.gameObject.CompareTag("Edges"))
        {
            HandleEdgeCollision();
        }
    }

    void HandleEdgeCollision()
    {
        Vector2 currentVelocity = rb.velocity;
        rb.velocity = new Vector2(currentVelocity.x, -currentVelocity.y);
    }

    void HandlePaddleCollision(Collider2D paddleCollider)
    {
        // Get the current velocity
        Vector2 currentVelocity = rb.velocity;

        // Calculate the normal of the paddle at the point of collision
        Vector2 paddleNormal = (transform.position - paddleCollider.transform.position).normalized;

        // Calculate the reflection
        Vector2 reflectedVelocity = Vector2.Reflect(currentVelocity, paddleNormal);

        // Invert the horizontal velocity to reflect properly
        reflectedVelocity.x = -reflectedVelocity.x;

        // Set the new velocity
        rb.velocity = reflectedVelocity;

        // Optionally, add a small speed increase on each hit
        // rb.velocity *= 1.05f;
    }

    public void ResetPosition()
    {
        transform.position = Vector3.zero;
        rb.velocity = Vector2.zero;
        InitialMovement();
    }

    IEnumerator ResetAfterDelay()
    {
        if (gameManager.GameEnded) yield break;

        ballImage.enabled = false;
        GetComponent<Collider2D>().enabled = false;

        yield return new WaitForSeconds(resetDelay);

        if (gameManager.GameEnded) yield break;

        transform.position = startPosition;
        rb.velocity = Vector2.zero;

        ballImage.enabled = true;
        GetComponent<Collider2D>().enabled = true;

        InitialMovement();
    }

    public void StopMovement()
    {
        StopAllCoroutines();
        rb.velocity = Vector2.zero;
    }
}
