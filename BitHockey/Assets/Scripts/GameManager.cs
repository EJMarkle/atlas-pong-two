using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public GameObject hockeyPaddleTwo;
    public Puck puck;
    public Score leftScore;
    public Score rightScore;
    public int winningScore = 5;

    public GameObject gameCompleteObject;
    public TextMeshProUGUI gameCompleteText;
    public Color winnerColor = Color.green;
    public Color loserColor = Color.red;

    private bool gameEnded = false;

    // Public property to check if the game has ended
    public bool GameEnded => gameEnded;

    private void Start()
    {
        SetupGame();
    }

    private void SetupGame()
    {
        // Enable AIPlayer and disable Player2Controller
        AIPlayer aiPlayer = hockeyPaddleTwo.GetComponent<AIPlayer>();
        Player2Controller player2Controller = hockeyPaddleTwo.GetComponent<Player2Controller>();

        if (aiPlayer != null) aiPlayer.enabled = true;
        if (player2Controller != null) player2Controller.enabled = false;

        // Reset scores
        leftScore.ResetScore();
        rightScore.ResetScore();

        // Hide game complete UI
        gameCompleteObject.SetActive(false);

        // Reset puck
        // ResetPuck();

        // Reset game ended flag
        gameEnded = false;
    }

    // private void ResetPuck()
    //{
    //    puck.transform.position = Vector2.zero;
    //    Debug.Log("ResetPuck: Puck position set to center: " + puck.transform.position);
    //    puck.LaunchPuck();
    //}

    // Method to handle scoring points
    public void ScorePoint(bool isLeftGoal)
    {
        if (gameEnded) return;

        if (isLeftGoal)
        {
            rightScore.IncrementScore();
            if (rightScore.GetScore() >= winningScore)
            {
                EndGame(false);
            }
        }
        else
        {
            leftScore.IncrementScore();
            if (leftScore.GetScore() >= winningScore)
            {
                EndGame(true);
            }
        }

        if (!gameEnded)
        {
        //    ResetPuck();
        }
    }

    private void EndGame(bool leftPlayerWon)
    {
        gameEnded = true;

        if (leftPlayerWon)
        {
            leftScore.SetScoreColor(winnerColor);
            rightScore.SetScoreColor(loserColor);
            gameCompleteText.text = "Left Player Wins!";
        }
        else
        {
            rightScore.SetScoreColor(winnerColor);
            leftScore.SetScoreColor(loserColor);
            gameCompleteText.text = "Right Player Wins!";
        }

        gameCompleteObject.SetActive(true);
        puck.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
    }

    // Method to handle when a goal is scored
    public void OnGoalScored(bool isLeftGoal)
    {
        ScorePoint(isLeftGoal);
    }

    // Method to restart the game
    public void RestartGame()
    {
        SetupGame();
    }

    // Method to toggle between single-player and multiplayer mode
    public void SetGameMode(bool isSinglePlayer)
    {
        AIPlayer aiPlayer = hockeyPaddleTwo.GetComponent<AIPlayer>();
        Player2Controller player2Controller = hockeyPaddleTwo.GetComponent<Player2Controller>();

        if (isSinglePlayer)
        {
            if (aiPlayer != null) aiPlayer.enabled = true;
            if (player2Controller != null) player2Controller.enabled = false;
        }
        else
        {
            if (aiPlayer != null) aiPlayer.enabled = false;
            if (player2Controller != null) player2Controller.enabled = true;
        }
    }
}
