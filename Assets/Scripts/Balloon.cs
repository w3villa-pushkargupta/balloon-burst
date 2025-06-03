using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

public class Balloon : MonoBehaviour
{
    public Color balloonColor;
    private SpriteRenderer sr;

    public ParticleSystem popEffect;

    private float newOffset;

    float timer = 0f;
    float interval = 1f;

    Tween waveYTween;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        sr.color = balloonColor;
    }

    public void HandleTouch()
    {
        if (balloonColor == GameManager.instance.targetColor)
        {
            Pop();
            
        }
        else
        {
            Shake();
        }

    }

    private void Shake()
    {
        transform.DOShakePosition(0.3f, new Vector3(0.05f, 0.01f, 0f));
        GameManager.instance.BalloonShakeSound();
    }

    private void Pop()
    {
        Destroy(gameObject);

        GameManager.instance.RegisterBalloonDestroyed();
        GameManager.instance.BalloonPopSound();
        Instantiate(popEffect, transform.position, Quaternion.identity);

    }

    public void SetColor(Color color)
    {
        balloonColor = color;
        if (sr != null) sr.color = color;

        if (popEffect != null && GameManager.instance != null)
        {
            var main = popEffect.main;
            main.startColor = GameManager.instance.targetColor;
        }
    }

    public void AnimateBalloon()
    {
        waveYTween?.Kill();
        if (newOffset < 0.6f)
        {
            float yoyoOffset = Random.Range(0.01f, 0.06f);
            float yoyoDuration = Random.Range(0.2f, 0.5f);

            waveYTween = transform.DOMoveY(transform.position.y + yoyoOffset, yoyoDuration)
                .SetLoops(-1, LoopType.Yoyo)
                .SetEase(Ease.InOutSine);

            //waveYTween = transform.DOShakePosition(duration: yoyoDuration, strength: new Vector3(0f, yoyoOffset, 0f), vibrato: 10, randomness: 90, fadeOut: true)
            //    .SetLoops(-1, LoopType.Restart);
        }
        else {  return; }
    }

    private void Update()
    {
        TimeToAnimate();    
    }

    public void TimeToAnimate()
    { 
        timer += Time.deltaTime;
        if (timer >= interval)
        {
            timer = 0f;

            newOffset = Random.Range(0.1f, 1f);
            AnimateBalloon();
        }
    }
}

