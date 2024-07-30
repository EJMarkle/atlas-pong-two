using UnityEngine;

public class Goal : MonoBehaviour
{
    public bool isLeftGoal;
    private GameManager gameManager;

    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Puck"))
        {
            gameManager.OnGoalScored(isLeftGoal);
        }
    }
}