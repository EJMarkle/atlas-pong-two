using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;


/// <summary>
/// GameManager class, handles game logic
/// </summary>
public class GameManager : MonoBehaviour
{
    public Score leftScore;
    public Score rightScore;
    public Ball ball;
    public Paddle leftPaddle;
    public Paddle rightPaddle;
    public int winningScore = 11;
    public bool GameEnded { get { return gameEnded; } }

    public Color winnerColor = Color.green;
    public Color loserColor = Color.red;
    public GameObject gameCompleteObject;
    public TextMeshProUGUI gameCompleteText;
    public float flashInterval = 0.5f;
    public GameObject aiPlayer;
    private bool gameEnded = false;
    private bool isListeningForRestart = false;

    // Check if AI should be enabled
    private void Start()
    {
        bool aiEnabled = PlayerPrefs.GetInt("AIEnabled", 0) == 1;
        aiPlayer.SetActive(aiEnabled);

        // AudioManager.Instance.PlayBackgroundMusic();
    }

    private void Update()
    {
        if (isListeningForRestart && Input.GetKeyDown(KeyCode.Escape))
        {
            ReturnToMenu();
        }
    }

    // Score counter and triggers win sequence upon a winning score
    public void ScorePoint(bool leftPlayerScored)
    {
        if (gameEnded) return;

        if (leftPlayerScored)
        {
            leftScore.IncrementScore();
            if (leftScore.GetScore() >= winningScore)
            {
                WinSequence(true);
            }
        }
        else
        {
            rightScore.IncrementScore();
            if (rightScore.GetScore() >= winningScore)
            {
                WinSequence(false);
            }
        }

        if (!gameEnded)
        {
            ResetBall();
        }
    }
 
    // Resets ball
    private void ResetBall()
    {
        ball.ResetPosition();
    }

    // Paints winner score green and loser score red and stops ball and paddle movement
    private void WinSequence(bool leftPlayerWon)
    {
        gameEnded = true;

        if (leftPlayerWon)
        {
            leftScore.SetScoreColor(winnerColor);
            rightScore.SetScoreColor(loserColor);
        }
        else
        {
            rightScore.SetScoreColor(winnerColor);
            leftScore.SetScoreColor(loserColor);
        }

        ball.StopMovement();

        leftPaddle.FreezePaddle();
        rightPaddle.FreezePaddle();

        gameCompleteObject.SetActive(true);
        StartCoroutine(FlashText());

        isListeningForRestart = true;
    }

    private void ReturnToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    private IEnumerator FlashText()
    {
        while (true)
        {
            gameCompleteText.enabled = !gameCompleteText.enabled;
            yield return new WaitForSeconds(flashInterval);
        }
    }
    private void RestartGame()
    {
        StopAllCoroutines();
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}