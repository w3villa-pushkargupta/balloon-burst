using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Color[] colorOptions;
    public Color targetColor;

    public Image targetColorImg;

    private int size;

    public int Size {  get { return size; } }

    private void Awake()
    {
        if (instance == null) instance = this;

        else if (instance != this) Destroy(gameObject);
    }

    private void Start()
    {
        SelectColor();

        size = colorOptions.Length;
    }

    public void SelectColor()
    {
        targetColor = colorOptions[Random.Range(0, colorOptions.Length)];
        targetColorImg.color = targetColor;
    }
}
