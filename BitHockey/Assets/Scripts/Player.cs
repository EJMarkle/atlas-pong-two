using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public bool isLeftPlayer = true; // Set this in the inspector for each player
    private Paddle paddle;
    private Camera mainCamera;

    void Start()
    {
        paddle = GetComponent<Paddle>();
        mainCamera = Camera.main;
    }
    
    void Update()
    {
        Vector3 mousePosition = Input.mousePosition;
        Vector3 worldPosition = mainCamera.ScreenToWorldPoint(mousePosition);
        worldPosition.z = 0; // Ensure the z-position is 0 for 2D

        // Restrict x-position based on which player this is
        if (isLeftPlayer)
        {
            worldPosition.x = Mathf.Min(worldPosition.x, 0); // Left side of screen
        }
        else
        {
            worldPosition.x = Mathf.Max(worldPosition.x, 0); // Right side of screen
        }

        paddle.MovePaddleToPosition(worldPosition);
    }
}