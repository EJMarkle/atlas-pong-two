using UnityEngine;
using UnityEngine.UI;
using TMPro;


/// <summary>
/// Score class, handles score incs and reset
/// </summary>
public class Score : MonoBehaviour
{
    private int score = 0;
    public Text scoreText;

    // inc score
    public void IncrementScore()
    {
        score++;
        UpdateScoreText();
    }

    // get score
    public int GetScore()
    {
        return score;
    }
 
    // score reset
    public void ResetScore()
    {
        score = 0;
        UpdateScoreText();
    }

    // set score text color
    public void SetScoreColor(Color color)
    {
        scoreText.color = color;
    }

    // updates score text
    private void UpdateScoreText()
    {
        scoreText.text = score.ToString();
    }
}
