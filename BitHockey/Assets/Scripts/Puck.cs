using UnityEngine;
using UnityEngine.UI;
using System.Collections;


/// <summary>
/// Puck class, handles puck reflections and triggers
/// </summary>
public class Puck : MonoBehaviour
{
    [SerializeField] private float initialSpeed = 10f;
    [SerializeField] private float paddleForceMultiplier = 0.2f;
    [SerializeField] public float maxVelocity = 20f;
    [SerializeField] public float slowLimit = 2f;
    [SerializeField] private float slowdownFactor = 0.99f;
    private GameManager gameManager;
    private GameObject puckLight;

    private Rigidbody2D rb;
    private Camera mainCamera;
    private Collider2D puckCollider;
    private AudioManager audioManager;

    // inits
    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        puckCollider = GetComponent<Collider2D>();
        gameManager = FindObjectOfType<GameManager>();
        mainCamera = Camera.main;
        puckLight = transform.Find("PuckLight").gameObject;
        puckLight.SetActive(false);
    }

    // launch puck and start audio
    private void Start()
    {
        LaunchPuck();
        audioManager = AudioManager.Instance;
    }

    // apply constyant slowdown to puck
    private void Update()
    {
        ApplySlowdown();
    }

    // triggers
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (!gameManager.isGameEnded)
        {   
            // reset puck w out of bounds method
            if (collider.CompareTag("Catch"))
            {
                Debug.Log("Puck caught, resetting...");
                gameManager.ResetPuckOutOfBounds();
            }
            // screen edge puck collisions
            else if (collider.CompareTag("Edges") || collider.CompareTag("Sides"))
            {
                if (collider.CompareTag("Edges"))
                {
                    rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);
                }
                else
                {
                    rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
                }
                audioManager.PlayEdgeHitSound();
                StartCoroutine(FlashPuckLight());
            }
            // paddle and puck collisions
            else if (collider.CompareTag("Paddle"))
            {
                ReflectOffPaddle(collider);
                audioManager.PlayPaddleHitSound();
                gameManager.TriggerPaddleLight(collider.gameObject);
                StartCoroutine(FlashPuckLight());
            }
        }
    }

    // enable "light" gameobject
    private IEnumerator FlashPuckLight()
    {
        puckLight.SetActive(true);
        yield return new WaitForSeconds(0.10f);
        puckLight.SetActive(false);
    }

    // Paddle and puck reflection logic
    private void ReflectOffPaddle(Collider2D paddleCollider)
    {
        HPaddle paddleScript = paddleCollider.GetComponent<HPaddle>();

        if (paddleScript != null)
        {
            Vector2 paddleVelocity = paddleScript.GetCurrentVelocity();
            Vector2 puckVelocity = rb.velocity;

            // Reflect the puck's direction based on the collision normal
            Vector2 normal = ((Vector2)transform.position - (Vector2)paddleCollider.bounds.center).normalized;
            Vector2 reflection = Vector2.Reflect(puckVelocity, normal);

            // Check if the paddle's speed is at or below the slowLimit
            if (paddleVelocity.magnitude <= slowLimit)
            {
                // Quarter the puck's velocity
                rb.velocity = reflection.normalized * (puckVelocity.magnitude / 4f);
            }
            else
            {
                // Calculate new speed based on the paddle's speed
                float speedMultiplier = 1 + (paddleVelocity.magnitude * paddleForceMultiplier);
                Vector2 newVelocity = reflection.normalized * (puckVelocity.magnitude * speedMultiplier);

                // Clamp the new velocity to the maxVelocity
                if (newVelocity.magnitude > maxVelocity)
                {
                    newVelocity = newVelocity.normalized * maxVelocity;
                }

                rb.velocity = newVelocity;
            }
        }
        //else
        //{
        //    rb.velocity = -rb.velocity;
        //}
    }

    // slowdown logic, supposed to feel like an air hockey table
    private void ApplySlowdown()
    {
        if (rb.velocity.magnitude > 0.1f)
        {
            rb.velocity *= slowdownFactor;
        }
        else
        {
            rb.velocity = Vector2.zero;
        }
    }

    // launch puck slowly in one of four random directions
    public void LaunchPuck()
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

        rb.velocity = initialDirection * initialSpeed;
    }

    // reset puck
    public void ResetPuckPosition()
    {
        transform.position = gameManager.centerSpawnPoint.position;
        rb.velocity = Vector2.zero;
    }
}
