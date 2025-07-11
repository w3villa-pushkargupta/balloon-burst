﻿using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Balloon : MonoBehaviour
{
   [SerializeField] private Color balloonColor;

    [SerializeField] private SpriteRenderer graphicsSprite;
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private ParticleSystem popEffect;

    [SerializeField] private float percentageAnimateAtTime = 0.3f;

    private float newRandomValue;
    private float timer = 0f;
    private float intervalToAnimateBalloon = 1f;

    private Tween waveYTween;
    private Vector2 originalPosition;

    private void Start()
    {
        sr.color = balloonColor;
       
    }

    public void HandleTouch()
    {
        //if (balloonColor == GameManager.instance.targetColor)
        if(graphicsSprite.sprite == GameManager.instance.upperLetterTargetSprite)
        {
            Pop();
            
        }
        else
        {
            Shake();
        }

    }

    public Color GetBalloonColor()
    {
        return balloonColor;
    }

    private void Shake()
    {
        transform.DOShakePosition(0.3f, new Vector3(0.05f, 0.01f, 0f)).OnComplete(() =>
        {
            AudioManager.Instance.PlayShakeSound();
        });
    }

    private void Pop()
    {
        Destroy(gameObject);

        GameManager.instance.RegisterBalloonDestroyed();
        AudioManager.Instance.PlayPopSound();

        GameObject effect = Instantiate(popEffect.gameObject, transform.position, Quaternion.identity);

      //  effect.transform.DOMove(targetPosition, 0.5f).SetEase(Ease.OutQuad);
    }

    public void SetColor(Color color, Vector2 position)
    {
        originalPosition = position;
        balloonColor = color;

        if (popEffect != null && GameManager.instance != null)
        {
            var main = popEffect.main;
            main.startColor = GameManager.instance.targetColor;
        }
    }

    public void SetSprite(Vector2 position, Sprite sr, Color color)
    {
        originalPosition = position;
        balloonColor = color;
        if (graphicsSprite != null) graphicsSprite.sprite = sr;

        //if (popEffect != null && GameManager.instance != null)
        //{
        //    var main = popEffect.main;
        //    main.startColor = GameManager.instance.targetColor;
        //}
    }

    public void AnimateBalloon()
    {
        waveYTween?.Kill();
        if (newRandomValue < percentageAnimateAtTime)
        {
            StartFloatingEffect();
        }

        else {  return; }
    }

    private void StartFloatingEffect()
    {
        float moveRange = 0.09f;     
        float segmentDuration = 1f;  
        int segments = 3;
        Sequence floatSequence = DOTween.Sequence();

        Vector2 currentPos = originalPosition;

        for (int i = 0; i < segments; i++)
        {
            Vector2 randomOffset = new Vector2(
                Random.Range(-moveRange, moveRange),
                Random.Range(-moveRange, moveRange)
            );

            Vector2 nextPoint = currentPos + randomOffset;
            floatSequence.Append(transform.DOMove(nextPoint, segmentDuration).SetEase(Ease.InOutSine));
            currentPos = nextPoint;
        }

        for (int i = 0; i < segments; i++)
        {
            Vector2 randomOffset = new Vector2(
                Random.Range(-moveRange, moveRange),
                Random.Range(-moveRange, moveRange)
            );

            Vector2 nextPoint = originalPosition ;// + randomOffset * (1f - (i / (float)segments)); 
            floatSequence.Append(transform.DOMove(nextPoint, segmentDuration).SetEase(Ease.InOutSine));
        }

        floatSequence.OnComplete(StartFloatingEffect);

        //float yoyoOffset = Random.Range(0.01f, 0.06f);
        //float yoyoDuration = Random.Range(0.2f, 0.6f);

        //waveYTween = transform.DOMoveY(transform.position.y + yoyoOffset, yoyoDuration)
        //    .SetLoops(-1, LoopType.Yoyo)
        //    .SetEase(Ease.InOutSine);

    }

    private void Update()
    {
        TimeToAnimate();    
    }

    public void TimeToAnimate()
    { 
        timer += Time.deltaTime;
        if (timer >= intervalToAnimateBalloon)
        {
            timer = 0f;

            newRandomValue = Random.Range(0.1f, 1f);
            AnimateBalloon();
        }
    }
}

