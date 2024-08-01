using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using UnityEngine.SceneManagement;


/// <summary>
/// GameManager class, game logic, one player mode, 
/// </summary>
public class GameManager : MonoBehaviour
{
    public Score leftScore;
    public Score rightScore;
    public Puck puck;
    public Transform centerSpawnPoint;
    public float resetDelay = 3f;
    public int WinScore = 5;
    public GameObject GameComplete;
    public Text leftScoreDisplay;
    public Text rightScoreDisplay;
    private bool canReloadScene = false;
    public static bool IsOnePlayerMode { get; set; } = true;
    public bool isGameEnded { get; private set; } = false;
    private AudioManager audioManager;

    // Start puck coroutine and audio
    private void Start()
    {
        GameComplete.SetActive(false);
        SetupPlayers();
        StartCoroutine(ResetAndLaunchPuck(0f));        
        audioManager = AudioManager.Instance;
    }

    // Check if oneplayer mode and set accordingly
    private void SetupPlayers()
    {
        GameObject paddleTwo = GameObject.Find("HockeyPaddleTwo");
        if (paddleTwo != null)
        {
            Player2Controller player2Controller = paddleTwo.GetComponent<Player2Controller>();
            AIPlayer aiPlayer = paddleTwo.GetComponent<AIPlayer>();

            if (player2Controller != null && aiPlayer != null)
            {
                player2Controller.enabled = !IsOnePlayerMode;
                aiPlayer.enabled = IsOnePlayerMode;
            }
        }
    }

    // play sound and inc score on goal
    public void OnGoalScored(bool isLeftGoal)
    {
        if (isGameEnded) return;

        audioManager.PlayScoreSound();
        ScorePoint(!isLeftGoal);
        
        StartCoroutine(ResetAndLaunchPuck(resetDelay));
    }

    // inc score for appropriate player
    public void ScorePoint(bool isLeftPlayer)
    {
        if (isLeftPlayer)
        {
            leftScore.IncrementScore();
        }
        else
        {
            rightScore.IncrementScore();
        }

        CheckGameEnd();
    }

    // CHeck if any player has reached win score
    private void CheckGameEnd()
    {
        if (leftScore.GetScore() >= WinScore || rightScore.GetScore() >= WinScore)
        {
            GameEnded();
        }
    }

    // Starts winsequence
    public void GameEnded()
    {
        isGameEnded = true;
        Debug.Log("Game Ended!");
        StartCoroutine(WinSequence());
    }

    // reset and laucnh puck w/o score inc
    public void ResetPuckOutOfBounds()
    {
        if (!isGameEnded)
        {
            StopAllCoroutines();
            StartCoroutine(ResetAndLaunchPuck(resetDelay));
        }
    }

    // reset and launch puck w delay
    private IEnumerator ResetAndLaunchPuck(float delay)
    {
        yield return new WaitForSeconds(delay);
        puck.ResetPuckPosition();
        puck.LaunchPuck();
    }

    // Stop coroutines and call WinSequence
    private IEnumerator WinSequence()
    {
        StopAllCoroutines();

        StartCoroutine(ExecuteWinSequence());

        yield break;
    }

    // Stop puck, change score colors,  enable GameComplete object, enable escape to return to menu
    private IEnumerator ExecuteWinSequence()
    {
        puck.ResetPuckPosition();
        puck.gameObject.SetActive(false);

        if (leftScore.GetScore() >= WinScore)
        {
            leftScoreDisplay.color = Color.green;
            rightScoreDisplay.color = Color.red;
        }
        else
        {
            leftScoreDisplay.color = Color.red;
            rightScoreDisplay.color = Color.green;
        }

        yield return new WaitForSeconds(0.5f);

        GameComplete.SetActive(true);
        
        canReloadScene = true;

        while (!Input.GetKeyDown(KeyCode.Escape))
        {
            yield return null;
        }

        SceneManager.LoadScene("Menu");
    }

    // paddle light
    public void TriggerPaddleLight(GameObject paddle)
    {
        StartCoroutine(FlashLight(paddle.transform.Find("PaddleLight").gameObject));
    }

    // puck light
    private IEnumerator FlashLight(GameObject light)
    {
        light.SetActive(true);
        yield return new WaitForSeconds(0.10f);
        light.SetActive(false);
    }

    // listener for escape key press
    private void Update()
    {
        if (canReloadScene && Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadScene("Menu");
        }
    }
}
