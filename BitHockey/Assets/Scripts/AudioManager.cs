using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


/// <summary>
/// AudioManage class, plays music and sound effects
/// </summary>
public class AudioManager : MonoBehaviour
{
    public AudioSource introMusic;
    public AudioSource mainLoopMusic;
    public AudioSource paddleHitSound;
    public AudioSource edgeHitSound;
    public AudioSource scoreSound;

    private static AudioManager instance;
    private bool hasPlayedIntro = false;

    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
            }
            return instance;
        }
    }

    // inits
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    // play background music on scene load
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Menu" && !hasPlayedIntro)
        {
            StartCoroutine(PlayMusicSequence());
        }
    }

    // play intro loop once, then repeat main loop indef
    private IEnumerator PlayMusicSequence()
    {
        if (introMusic != null && !hasPlayedIntro)
        {
            StopAllAudio();

            introMusic.Play();
            hasPlayedIntro = true;

            yield return new WaitForSeconds(introMusic.clip.length);

            PlayMainLoopMusic();
        }
        else
        {
            PlayMainLoopMusic();
        }
    }

    // loops the main audio loop
    private void PlayMainLoopMusic()
    {
        if (mainLoopMusic != null && !mainLoopMusic.isPlaying)
        {
            mainLoopMusic.loop = true;
            mainLoopMusic.Play();
        }
    }

    // stops all audio
    private void StopAllAudio()
    {
        introMusic.Stop();
        mainLoopMusic.Stop();
    }

    // plays paddle hit osund
    public void PlayPaddleHitSound()
    {
        if (paddleHitSound != null)
        {
            paddleHitSound.PlayOneShot(paddleHitSound.clip);
        }
    }

    // plays screen edge ht sound
    public void PlayEdgeHitSound()
    {
        if (edgeHitSound != null)
        {
            edgeHitSound.PlayOneShot(edgeHitSound.clip);
        }
    }

    // plays goal score sound
    public void PlayScoreSound()
    {
        if (scoreSound != null)
        {
            scoreSound.PlayOneShot(scoreSound.clip);
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}