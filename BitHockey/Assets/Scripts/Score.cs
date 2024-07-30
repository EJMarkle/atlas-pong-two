using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    private int score = 0;
    public TextMeshProUGUI scoreText;

    public void IncrementScore()
    {
        score++;
        UpdateScoreText();
    }

    public int GetScore()
    {
        return score;
    }
 
    public void ResetScore()
    {
        score = 0;
        UpdateScoreText();
    }

    public void SetScoreColor(Color color)
    {
        scoreText.color = color;
    }

    private void UpdateScoreText()
    {
        scoreText.text = score.ToString();
    }
}
