using UnityEngine;


/// <summary>
/// Goal class, raises score when puck impacts goal
/// </summary>
public class Goal : MonoBehaviour
{
    public bool isLeftGoal;
    private GameManager gameManager;

    // init
    private void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    // Call OnGoalScored when triggered by Puck
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Puck"))
        {
            gameManager.OnGoalScored(isLeftGoal);
        }
    }
}
