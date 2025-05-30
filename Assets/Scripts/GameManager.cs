using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Color[] colorOptions;
    public Color targetColor;

    public Image targetColorImg;

    public int maxBalloon = 20;
    public int balloonTargetCount = 5;

    private void Awake()
    {
        if (instance == null) instance = this;

        else if (instance != this) Destroy(gameObject);
    }

    private void Start()
    {
        SelectColor();
    }

    public void SelectColor()
    {
        targetColor = colorOptions[Random.Range(0, colorOptions.Length)];
        targetColorImg.color = targetColor;
    }
}
