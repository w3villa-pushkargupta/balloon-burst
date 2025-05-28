using DG.Tweening.Plugins.Options;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BalloonSpawner : MonoBehaviour
{
    public GameObject balloonPrefab;
    public float spawnTime = 1f;
    public int maxBalloon = 10;

    private float magnitudeX = 1.1f;

    public float minX = -7f, maxX = 7f, minY = -2.75f, maxY = 2.25f;

    private List<GameObject> activeballoons = new List<GameObject>();

    private void Start()
    {
        InvokeRepeating(nameof(Spawnballon), 0.05f, spawnTime);
    }

    void Spawnballon()
    {
        if (activeballoons.Count >= maxBalloon) return;

        Vector2 spawnPos = RandomRangeInScreen();
        GameObject balloon = Instantiate(balloonPrefab, spawnPos, Quaternion.Euler(0, 0, Random.Range(-50f, 50f)), transform);
        activeballoons.Add(balloon);

        // Color randomColor = GameManager.instance.colorOptions[Random.Range(0, GameManager.instance.colorOptions.Length)];

        Color randomColor = GetColorWithPriority();
        balloon.GetComponent<Balloon>().SetColor(randomColor);
    }

    Color GetColorWithPriority()
    {
        float randomValue = Random.Range(0f, 1f);
        Debug.Log(randomValue);

        if (randomValue < 0.6f)
        {
            return GameManager.instance.targetColor;
        }
        else
        {
            List<Color> otherColors = GameManager.instance.colorOptions.Where(c => c != GameManager.instance.targetColor).ToList();
            return otherColors[Random.Range(0, otherColors.Count)];
        }
    }

    Vector2 RandomRangeInScreen()
    {
        Vector3 pos = transform.position;

        pos.x = Random.Range(minX, maxX) * magnitudeX;
        pos.y = Random.Range(minY, maxY);

        return new Vector2(pos.x, pos.y);
    }
}
