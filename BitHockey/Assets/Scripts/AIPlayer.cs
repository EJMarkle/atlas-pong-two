using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// AIPlayer class which holds cpu emeny logic
/// </summary>
public class AIPlayer : MonoBehaviour
{
    private Paddle paddle;
    private Player playerScript;
    private Ball ball;
    
    [SerializeField] private float deadzone = 70f;

    private void OnEnable()
    {
        SetupAI();
        deadzone = PlayerPrefs.GetFloat("AIDeadzone", 70f);
    }

    private void Start()
    {
        SetupAI();
    }

    // Get components and disable player scripts
    private void SetupAI()
    {
        paddle = GetComponentInParent<Paddle>();
        if (paddle == null)
        {
            enabled = false;
            return;
        }

        playerScript = GetComponentInParent<Player>();
        if (playerScript != null)
        {
            playerScript.enabled = false;
        }

        ball = FindObjectOfType<Ball>();
        if (ball == null)
        {
            enabled = false;
        }
    }

    // Call MovePaddleTowardsBall() every frame
    private void Update()
    {
        if (paddle != null && ball != null)
        {
            MovePaddleTowardsBall();
        }
    }

    // Paddle paths to ball's y position w deadzone and reaction speed var
    private void MovePaddleTowardsBall()
    {
        float paddleY = paddle.transform.position.y;
        float ballY = ball.transform.position.y;
        
        float difference = ballY - paddleY;

        if (Mathf.Abs(difference) < deadzone)
        {
            paddle.MovePaddle(0);
            return;
        }

        float direction = Mathf.Sign(difference);

        float moveAmount = direction * Time.deltaTime;

        paddle.MovePaddle(direction);
    }
}