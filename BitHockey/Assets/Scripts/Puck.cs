using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puck : MonoBehaviour
{
    [SerializeField] private float initialSpeed = 10f;
    [SerializeField] private float paddleForceMultiplier = 0.2f; // Adjust this value as needed to balance the gameplay
    [SerializeField] public float maxVelocity = 20f; // Maximum velocity for the puck
    [SerializeField] public float slowLimit = 2f; // Velocity limit to trigger the slowdown
    [SerializeField] private float slowdownFactor = 0.99f; // Factor to control the rate of slowdown

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        LaunchPuck();
    }

    private void Update()
    {
        ApplySlowdown();
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.CompareTag("Edges"))
        {
            rb.velocity = new Vector2(rb.velocity.x, -rb.velocity.y);
        }
        else if (collider.CompareTag("Paddle"))
        {
            ReflectOffPaddle(collider);
        }
        else if (collider.CompareTag("Sides"))
        {
            rb.velocity = new Vector2(-rb.velocity.x, rb.velocity.y);
        }
    }

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
        else
        {
            // Fallback to simple reflection if there's no Rigidbody2D
            rb.velocity = -rb.velocity;
        }
    }

    private void ApplySlowdown()
    {
        if (rb.velocity.magnitude > 0.1f) // Apply slowdown only if the puck is moving
        {
            rb.velocity *= slowdownFactor;
        }
        else
        {
            rb.velocity = Vector2.zero; // Stop the puck completely if the velocity is very low
        }
    }

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
}
