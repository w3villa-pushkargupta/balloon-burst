using UnityEngine;
using System.Collections;
using TMPro;

public class BalloonGameTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    private float timer = 15f;
    private bool timerRunning = true;

    public BalloonSpawner balloonSpawner;

    void Start()
    {
        balloonSpawner.OnCoroutineComplete += StartTimer;
        GameManager.instance.OnGameCompleted += StopTimer;
    }

    void StartTimer()
    {
        StartCoroutine(TimeRunner());
    }

    IEnumerator TimeRunner()
    {
        while (timerRunning && timer > 0f)
        {
            timer -= Time.deltaTime;
            if (timer < 0f) timer = 0f;

            UpdateTimerUI();
            yield return null; 
        }
        timerRunning = false;
    }

    int UpdateTimerUI()
    {
        timerText.text = Mathf.CeilToInt(timer).ToString();
        return Mathf.CeilToInt(timer);
    }

    private void StopTimer()
    {
        timerRunning = false;
        int timeValue = UpdateTimerUI();

        if (timeValue >= 10)
        {
            Debug.Log("3 Star");
        }
        else if (timeValue >= 5 && timeValue < 10)
        {
            Debug.Log("2 Star");
        }
        else
        {
            Debug.Log("1 Star");
        }
    }

}
