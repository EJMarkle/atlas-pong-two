using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puck : MonoBehaviour
{
    [SerializeField] private float initialSpeed = 10f;
    [SerializeField] private float paddleForceMultiplier = 0.2f; // Adjust this value as needed to balance the gameplay

    private Rigidbody2D rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Start()
    {
        LaunchPuck();
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
        Rigidbody2D paddleRb = paddleCollider.GetComponent<Rigidbody2D>();

        if (paddleRb != null)
        {
            Vector2 hitPoint = paddleCollider.ClosestPoint(transform.position);
            Vector2 paddleCenter = paddleCollider.bounds.center;

            // Calculate hit direction
            Vector2 hitDirection = ((Vector2)transform.position - paddleCenter).normalized;

            // Get paddle velocity
            Vector2 paddleVelocity = paddleRb.velocity;

            // Reflect the puck's direction based on the hit point
            Vector2 newVelocity = hitDirection * initialSpeed;

            // Add paddle's velocity influence to the puck
            newVelocity += paddleVelocity * paddleForceMultiplier;

            // Ensure the puck's speed does not drop below a minimum threshold
            if (newVelocity.magnitude < initialSpeed)
            {
                newVelocity = newVelocity.normalized * initialSpeed;
            }

            rb.velocity = newVelocity;
        }
        else
        {
            // Fallback to simple reflection if there's no Rigidbody2D
            rb.velocity = -rb.velocity;
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
