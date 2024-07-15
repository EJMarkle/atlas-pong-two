using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
 

/// <summary>
/// Class for enabling score functionality
/// </summary>
public class Score : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    private int currentScore = 0;

    // Call UpdateScore every frame
    void Start()
    {
        UpdateScore();
    }

    // Score incrementer
    public void IncrementScore()
    {
        currentScore++;
        UpdateScore();
    }

    // Update score display
    private void UpdateScore()
    {
        scoreText.text = currentScore.ToString();
    }

    // Get score method
    public int GetScore()
    {
        return currentScore;
    }

    // Changes text color
    public void SetScoreColor(Color color)
    {
        scoreText.color = color;
    }
}