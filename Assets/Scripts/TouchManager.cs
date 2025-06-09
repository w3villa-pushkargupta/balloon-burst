using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TouchManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem touchEffect;

    private void Start()
    {
        var main = touchEffect.main;
        main.startColor = GameManager.instance.targetColor;
    }
    private void FixedUpdate()
    {
        if (Input.GetMouseButton(0))
        {
            Vector2 touchPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(touchPos, Vector2.zero);
            if (hit.collider != null)
            {
                Balloon balloon = hit.collider.GetComponentInParent<Balloon>();
                if (balloon != null)
                {
                    balloon.HandleTouch();
                    if (balloon.GetBalloonColor() == GameManager.instance.targetColor)
                    {
                        Instantiate(touchEffect, touchPos, Quaternion.identity);
                    }
                }
            }
        }
    }
}

