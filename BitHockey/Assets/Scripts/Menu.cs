using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using TMPro;

public class Menu : MonoBehaviour
{
    public GameObject mainMenu;
    public GameObject optionsMenu;
    public GameObject tutorialMenu;
    public Button[] allButtons;
    public TMP_Dropdown difficultyDropdown;

    private void Start()
    {
        // Add event listeners to all buttons
        foreach (Button button in allButtons)
        {
            button.onClick.AddListener(() => OnButtonClick(button));
            
            EventTrigger trigger = button.gameObject.AddComponent<EventTrigger>();
            
            EventTrigger.Entry entryEnter = new EventTrigger.Entry();
            entryEnter.eventID = EventTriggerType.PointerEnter;
            entryEnter.callback.AddListener((data) => { OnPointerEnter(button); });
            trigger.triggers.Add(entryEnter);

            EventTrigger.Entry entryExit = new EventTrigger.Entry();
            entryExit.eventID = EventTriggerType.PointerExit;
            entryExit.callback.AddListener((data) => { OnPointerExit(button); });
            trigger.triggers.Add(entryExit);
        }

        difficultyDropdown.onValueChanged.AddListener(OnDifficultyChanged);
        int savedDifficulty = PlayerPrefs.GetInt("Difficulty", 1); // Default to Medium
        difficultyDropdown.value = savedDifficulty;
    }

    private void OnPointerEnter(Button button)
    {
        Color color = button.image.color;
        button.image.color = new Color(color.r, color.g, color.b, 0.5f);
    }

    private void OnPointerExit(Button button)
    {
        Color color = button.image.color;
        button.image.color = new Color(color.r, color.g, color.b, 1f);
    }

    private void OnButtonClick(Button button)
    {
        // Darken the button
        Color color = button.image.color;
        button.image.color = new Color(color.r * 0.8f, color.g * 0.8f, color.b * 0.8f, color.a);

        // Handle button functionality
        switch (button.name)
        {
            case "1PLAYER":
                PlayerPrefs.SetInt("AIEnabled", 1);
                SceneManager.LoadScene("Game");
                break;
            case "2PLAYER":
                PlayerPrefs.SetInt("AIEnabled", 0);
                SceneManager.LoadScene("Game");
                break;
            case "OPTIONS":
                mainMenu.SetActive(false);
                optionsMenu.SetActive(true);
                break;
            case "TUTORIAL":
                mainMenu.SetActive(false);
                tutorialMenu.SetActive(true);
                break;
            case "BACK":
                BackToMainMenu();
                break;
            case "QUIT":
                #if UNITY_EDITOR
                    UnityEditor.EditorApplication.isPlaying = false;
                #else
                    Application.Quit();
                #endif
                break;
        }
    }

    public void BackToMainMenu()
    {
        optionsMenu.SetActive(false);
        tutorialMenu.SetActive(false);
        mainMenu.SetActive(true);
    }

    private void OnDifficultyChanged(int value)
    {
        PlayerPrefs.SetInt("Difficulty", value);
        PlayerPrefs.Save();

        float deadzone;
        switch (value)
        {
            case 0: // EASY
                deadzone = 85f;
                break;
            case 1: // MEDIUM
                deadzone = 75f;
                break;
            case 2: // HARD
                deadzone = 65f;
                break;
            default:
                deadzone = 75f; // Default to MEDIUM
                break;
        }
        PlayerPrefs.SetFloat("AIDeadzone", deadzone);
        PlayerPrefs.Save();
    }
}