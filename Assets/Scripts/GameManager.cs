using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [Header("Colors")]
    public Color[] colorOptions;
    public Color targetColor;

    public Image targetColorImg;

    [Header("Balloon Spawn Numbers")]
    public int maxBalloon = 20;
    public int balloonTargetCount = 5;

    private int countPopped = 0;

    public Action OnGameCompleted;


    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

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
}
