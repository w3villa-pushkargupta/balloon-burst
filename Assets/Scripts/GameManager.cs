using System;
using System.Collections.Generic;
using System.Linq;
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


    [Header("Sprites")]
    public Sprite[] upperLetterSpriteOptions;
    public Sprite upperLetterTargetSprite;
    public Image upperLetterTargetSpriteImg;

    [Header("Balloon Spawn Numbers")]
    public int maxBalloon = 20;
    public int balloonTargetCount = 5;

    private int countPopped = 0;

    public Action OnGameCompleted;

    public RectTransform sliderStarTarget;

    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != this) Destroy(gameObject);

    }

    private void Start()
    {
        SelectColor();
        SelectSprite();
    }

    public void SelectColor()
    {
        targetColor = colorOptions[UnityEngine.Random.Range(0, colorOptions.Length)];
      //  targetColorImg.color = targetColor;
    }

    public void SelectSprite()
    {
        upperLetterTargetSprite = upperLetterSpriteOptions[UnityEngine.Random.Range(0, upperLetterSpriteOptions.Length)];
        upperLetterTargetSpriteImg.sprite = upperLetterTargetSprite;
        upperLetterTargetSpriteImg.color = targetColor;
    }

    public void RegisterBalloonDestroyed()
    {
        countPopped++;

        if (countPopped >= balloonTargetCount)
        {
            OnGameCompleted?.Invoke();
        }
    }

    public float GetBalloonProgress()
    {
        float progress = (float)countPopped / balloonTargetCount;
        return Mathf.Clamp01(progress);
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
