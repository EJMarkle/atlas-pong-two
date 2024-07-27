using UnityEngine;
using UnityEngine.UI;

public class Paddle : MonoBehaviour
{
    public float moveSpeed = 10f;
    private bool isFrozen = false;
    private CircleCollider2D circleCollider;
    private float paddleRadius;
    private Camera mainCamera;
    private Rigidbody2D rb;

    private void Start()
    {
        circleCollider = GetComponent<CircleCollider2D>();
        circleCollider.isTrigger = false;  // Ensure the collider is not a trigger
        paddleRadius = circleCollider.radius * transform.localScale.y;
        mainCamera = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        
        // Configure the Rigidbody2D
        rb.gravityScale = 0;
        rb.drag = 10; // Add some drag to make movement smoother
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Prevent rotation
    }

    public void MovePaddleToPosition(Vector3 targetPosition)
    {
        if (isFrozen) return;

        // Calculate the movement within screen bounds
        Vector3 viewportPoint = mainCamera.WorldToViewportPoint(targetPosition);
        viewportPoint.x = Mathf.Clamp(viewportPoint.x, 0.05f, 0.95f);
        viewportPoint.y = Mathf.Clamp(viewportPoint.y, 0.05f, 0.95f);
        Vector3 worldTargetPosition = mainCamera.ViewportToWorldPoint(viewportPoint);
        worldTargetPosition.z = 0;

        // Calculate the direction and distance to the target
        Vector2 direction = (worldTargetPosition - transform.position).normalized;
        float distance = Vector2.Distance(worldTargetPosition, transform.position);

        // Apply force to move towards the target position
        rb.AddForce(direction * moveSpeed * distance);
    }

    private void FixedUpdate()
    {
        // Limit the velocity to prevent excessive speed
        rb.velocity = Vector2.ClampMagnitude(rb.velocity, moveSpeed);
    }

    public void FreezePaddle()
    {
        isFrozen = true;
        rb.velocity = Vector2.zero;
    }
}