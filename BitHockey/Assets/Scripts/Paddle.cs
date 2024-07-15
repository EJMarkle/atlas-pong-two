using UnityEngine;


/// <summary>
/// Paddle class which handles paddle movement
/// </summary>
public class Paddle : MonoBehaviour
{ 
    public float moveSpeed = 10f;
    string edgesTag = "Edges";
    private bool isFrozen = false;

    // Enables paddle movement and prevents from moving off screen
    public void MovePaddle(float direction)
    {
        if (isFrozen) return;
        
        float moveAmount = direction * moveSpeed * Time.deltaTime;

        Vector3 targetPosition = transform.position + new Vector3(0, moveAmount, 0);

        transform.Translate(0, moveAmount, 0);

        if (direction > 0)
        {
            Collider2D[] hitColliders = Physics2D.OverlapBoxAll(transform.position, GetComponent<BoxCollider2D>().size, 0f);

            foreach (Collider2D collider in hitColliders)
            {
                if (collider.gameObject.CompareTag(edgesTag) && collider.gameObject.name == "TopEdge")
                {
                    float edgeY = collider.transform.position.y;
                    float paddleHeight = GetComponent<BoxCollider2D>().size.y;

                    float maxYPosition = edgeY - paddleHeight / 2;
                    transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, transform.position.y, maxYPosition), transform.position.z);
                }
            }
        }
        else if (direction < 0)
        {
            Collider2D[] hitColliders = Physics2D.OverlapBoxAll(transform.position, GetComponent<BoxCollider2D>().size, 0f);

            foreach (Collider2D collider in hitColliders)
            {
                if (collider.gameObject.CompareTag(edgesTag) && collider.gameObject.name == "BottomEdge")
                {
                    float edgeY = collider.transform.position.y;
                    float paddleHeight = GetComponent<BoxCollider2D>().size.y;

                    float minYPosition = edgeY + paddleHeight / 2;
                    transform.position = new Vector3(transform.position.x, Mathf.Clamp(transform.position.y, minYPosition, transform.position.y), transform.position.z);
                }
            }
        }
    }

    // Stop paddle movement
    public void FreezePaddle()
    {
        isFrozen = true;
    }
}