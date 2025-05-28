using DG.Tweening;
using UnityEngine;

public class Balloon : MonoBehaviour
{
    public Color balloonColor;
    private SpriteRenderer sr;

    public ParticleSystem popEffect;

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
    }

    private void Pop()
    {
        Destroy(gameObject);
        Instantiate(popEffect, transform.position, Quaternion.identity);
    }

    public void SetColor(Color color)
    {
        balloonColor = color;
        if (sr != null) sr.color = color;

        var main = popEffect.main;
        main.startColor = GameManager.instance.targetColor;
    }


}

