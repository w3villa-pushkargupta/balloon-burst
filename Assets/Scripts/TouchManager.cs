using UnityEngine;
using UnityEngine.Rendering.Universal;

public class TouchManager : MonoBehaviour
{
    //[SerializeField] private Balloon balloon;
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
                }
            }
        }
    }
}

