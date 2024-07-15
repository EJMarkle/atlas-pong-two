using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource backgroundMusic;
    public AudioSource paddleHitSound;
    public AudioSource edgeHitSound;
    public AudioSource scoreSound;

    private static AudioManager instance;

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

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    public void PlayBackgroundMusic()
    {
        if (backgroundMusic != null && !backgroundMusic.isPlaying)
        {
            backgroundMusic.loop = true;
            backgroundMusic.Play();
        }
    }

    public void PlayPaddleHitSound()
    {
        if (paddleHitSound != null)
        {
            paddleHitSound.Play();
        }
    }

    public void PlayEdgeHitSound()
    {
        if (edgeHitSound != null)
        {
            edgeHitSound.Play();
        }
    }

    public void PlayScoreSound()
    {
        if (scoreSound != null)
        {
            scoreSound.Play();
        }
    }
}