using UnityEngine;
using UnityEngine.UI;
using System.Collections;


/// <summary>
/// Ball class which governs movement and collisions
/// </summary>
public class Ball : MonoBehaviour
{
    public float initialForce = 5f;
    public float resetDelay = 2f;
    private Rigidbody2D rb;
    private Vector2 startPosition;
    public GameManager gameManager;
    private Image ballImage;
    public float ballAcceleration = 0.1f;

    // Init rigidbodies, components, and trigger ball movement
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = transform.position;
        ballImage = GetComponent<Image>();
        InitialMovement();
    }

    void Update()
    {
        // Apply constant acceleration to the ball
        if (rb.velocity != Vector2.zero)
        {
            rb.velocity += rb.velocity.normalized * ballAcceleration * Time.deltaTime;
        }
    }

    // Start moving the ball in 1 of 4 random directions, top-left, bottom-left, top-right, or bottom-right
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
 
    // Collision logic
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (gameManager.GameEnded) return;

        // Increase score of respective player upon goal collision
        if (collision.gameObject.CompareTag("LeftGoal"))
        {
            gameManager.ScorePoint(false);
            AudioManager.Instance.PlayScoreSound();
            StartCoroutine(ResetAfterDelay());
        }
        else if (collision.gameObject.CompareTag("RightGoal"))
        {
            gameManager.ScorePoint(true);
            AudioManager.Instance.PlayScoreSound();
            StartCoroutine(ResetAfterDelay());
        }

        // Bounce ball on off screen edge (marked w tagged invis gameobject)
        if (collision.gameObject.CompareTag("Edges"))
        {
            HandleEdgeCollision();
            AudioManager.Instance.PlayEdgeHitSound();
        }

        // Reflect ball on paddle collision
        else if (collision.gameObject.CompareTag("Paddle"))
        {
            HandlePaddleCollision(collision);
            AudioManager.Instance.PlayPaddleHitSound();
        }
    }

    // Bounce ball off screen edge w maintained horizontal velocity and rerversed angle 
    void HandleEdgeCollision()
    {
        Vector2 currentVelocity = rb.velocity;
        rb.velocity = new Vector2(currentVelocity.x, -currentVelocity.y);
    }

    // Bounce ball off paddles w maintained horizontal velocity and varable angle based on sweet spot (less angle when bounced off paddle center)
    void HandlePaddleCollision(Collider2D paddleCollider)
    {
        RectTransform paddleTransform = paddleCollider.GetComponent<RectTransform>();
        Vector2 paddleSize = paddleTransform.sizeDelta;

        float hitPositionY = transform.position.y - paddleTransform.position.y;
        float normalizedHitPosition = hitPositionY / (paddleSize.y / 2f);

        float maxBounceAngle = 70f;
        float bounceAngle = normalizedHitPosition * maxBounceAngle;

        Vector2 currentVelocity = rb.velocity;
        float speed = currentVelocity.magnitude;

        float reflectionDirection = (currentVelocity.x < 0) ? 1.0f : -1.0f;

        Vector2 newVelocity = Quaternion.Euler(0, 0, bounceAngle) * new Vector2(reflectionDirection, 0);

        rb.velocity = newVelocity.normalized * speed;
    }

    // Ball reset logic w launch
    public void ResetPosition()
    {
        transform.position = Vector3.zero;
        rb.velocity = Vector2.zero;
        InitialMovement();
    }
    
    // Disable ball and reset after 2 second delay
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

    // Freeze ball
    public void StopMovement()
    {
        StopAllCoroutines();
        rb.velocity = Vector2.zero;
    }
}