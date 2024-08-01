using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


/// <summary>
/// Menu class, enables UI buttons
/// </summary>
public class Menu : MonoBehaviour
{
    // starts game in player one mode, enabling AIPlayer
    public void Start1PlayerGame()
    {
        GameManager.IsOnePlayerMode = true;
        SceneManager.LoadScene("Game");
    }

    // starts game without player one mode, enabling Player2Controller
    public void Start2PlayerGame()
    {
        GameManager.IsOnePlayerMode = false;
        SceneManager.LoadScene("Game");
    }

    // Loads Menu scene
    public void ExitToMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    // Quits application
    public void QuitGame()
    {
        Application.Quit();
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
    }
}