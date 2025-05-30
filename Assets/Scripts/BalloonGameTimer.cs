using UnityEngine;
using System.Collections;
using TMPro;

public class BalloonGameTimer : MonoBehaviour
{
    public TextMeshProUGUI timerText;

    private float timer = 15f;
    private bool timerRunning = true;


    void Update()
    {
        TimeRunner();
    }

    void TimeRunner()
    {
        if (timerRunning)
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                timer = 0f;
                timerRunning = false;
            }

            UpdateTimerUI();
        }
    }

    void UpdateTimerUI()
    {
        timerText.text = Mathf.CeilToInt(timer).ToString();
    }
}
