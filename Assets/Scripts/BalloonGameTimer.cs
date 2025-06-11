using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BalloonGameTimer : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private BalloonSpawner balloonSpawner;
    public Slider timerSlider;

    [SerializeField] private float sliderSpeed = 5f;
    private float targetValue;

    [Header("Gameplay Time")]
    [SerializeField] private float maxTime = 15f;
    private float currentTime;
    private bool timerRunning = false;

    [Header("Stars/Points")]
    [SerializeField] private int minThreeStarTime = 10;
    [SerializeField] private int minTwoStarTime = 5;

    void Start()
    {
        balloonSpawner.OnCoroutineComplete += StartTimer;
        GameManager.instance.OnGameCompleted += StopTimer;

        timerSlider.fillRect.GetComponentInChildren<Image>().color = GameManager.instance.targetColor;
    }

    void StartTimer()
    {
        ResetTimer();
        StartCoroutine(TimeRunner());
    }

    void ResetTimer()
    {
        currentTime = maxTime;
        timerRunning = true;

        timerSlider.minValue = 0f;
        timerSlider.maxValue = 1f;
        timerSlider.value = 0f;

        UpdateSliderProgress();
        UpdateTimerUI();
    }

    IEnumerator TimeRunner()
    {
        while (timerRunning && currentTime > 0f)
        {
            currentTime -= Time.deltaTime;
            if (currentTime < 0f) currentTime = 0f;

            UpdateTimerUI();
           UpdateSliderProgress();

            yield return null;
        }

        //if (currentTime <= 0f && timerRunning)
        //{
        //    StopTimer();
        //}
    }

    //void Update()
    //{
    //    timerSlider.value = Mathf.Lerp(timerSlider.value, targetValue, Time.deltaTime * sliderSpeed);
    //}

    public void UpdateSliderProgress()
    {
        if(!timerRunning) return;   
        targetValue = GameManager.instance.GetBalloonProgress();

        timerSlider.DOKill();
        timerSlider.DOValue(targetValue, 0.3f).SetEase(Ease.OutFlash);
    }

    int UpdateTimerUI()
    {
        int displayTime = Mathf.CeilToInt(currentTime);
        timerText.text = displayTime.ToString();
        return displayTime;
    }

    private void StopTimer()
    {
        timerRunning = false;
        ShowStarBasedOnTimer();
    }

    private void ShowStarBasedOnTimer()
    {
        float remainingTime = currentTime;

        if (remainingTime >= minThreeStarTime)
        {
            Debug.Log("3 Stars");
        }
        else if (remainingTime >= minTwoStarTime)
        {
            Debug.Log("2 Stars");
        }
        else
        {
            Debug.Log("1 Star");
        }
    }
}
