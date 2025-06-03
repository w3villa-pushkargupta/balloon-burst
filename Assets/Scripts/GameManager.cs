using System;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Color[] colorOptions;
    public Color targetColor;

    public Image targetColorImg;

    public int maxBalloon = 20;
    public int balloonTargetCount = 5;

    private int countPopped = 0;

    public Action OnGameCompleted;

    [Header("Balloon Sounds")]
    public AudioClip[] popSounds;
    public AudioClip shakeSounds;
    private AudioSource audioSource;

    private int volumeMultiplier = 5;


    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>(); 
        }
    }

    private void Start()
    {
        SelectColor();
    }

    public void SelectColor()
    {
        targetColor = colorOptions[UnityEngine.Random.Range(0, colorOptions.Length)];
        targetColorImg.color = targetColor;
    }

    public void RegisterBalloonDestroyed()
    {
        countPopped++;

        if (countPopped >= balloonTargetCount)
        {
            OnGameCompleted?.Invoke();
        }
    }

    public void RestartGame()
    {
        SceneManager.LoadScene(0);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void BalloonPopSound()
    {

        if (audioSource != null && popSounds.Length > 0)
        {
            int index = UnityEngine.Random.Range(0, popSounds.Length);
            audioSource.pitch = UnityEngine.Random.Range(0.95f, 1.05f);
            audioSource.PlayOneShot(popSounds[index]);
        }
    }

    public void BalloonShakeSound()
    {
        audioSource.clip = shakeSounds;
        audioSource.volume = UnityEngine.Random.Range(0.8f, 1f) * volumeMultiplier;
        audioSource.pitch = UnityEngine.Random.Range(0.95f, 1.2f);
        audioSource.Play();
    }
}
