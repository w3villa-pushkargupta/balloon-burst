using UnityEngine;
using System.Collections;
using TMPro;

public class BalloonGameTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private BalloonSpawner balloonSpawner;

    [Header("Gameplay Time")]
    [SerializeField] private float gameTimer = 15f;
    private bool timerRunning = true;

    [Header("Get Stars/Points")]
    [SerializeField] private int minThreeStarTime = 10;
    [SerializeField] private int minTwoStarTime = 5;

    void Start()
    {
        balloonSpawner.OnCoroutineComplete += StartTimer;
        GameManager.instance.OnGameCompleted += StopTimer;
        timerText.color = GameManager.instance.targetColor; 
    }

    void StartTimer()
    {
        StartCoroutine(TimeRunner());
    }

    IEnumerator TimeRunner()
    {
        while (timerRunning && gameTimer > 0f)
        {
            gameTimer -= Time.deltaTime;
            if (gameTimer < 0f) gameTimer = 0f;

            UpdateTimerUI();
            yield return null; 
        }
        timerRunning = false;
    }

    int UpdateTimerUI()
    {
        timerText.text = Mathf.CeilToInt(gameTimer).ToString();
        return Mathf.CeilToInt(gameTimer);
    }

    private void StopTimer()
    {
        timerRunning = false;
        int timeValue = UpdateTimerUI();

        if (timeValue >= minThreeStarTime)
        {
            Debug.Log("3 Star");
        }
        else if (timeValue >= minTwoStarTime && timeValue < minThreeStarTime)
        {
            Debug.Log("2 Star");
        }
        else
        {
            Debug.Log("1 Star");
        }
    }

}
