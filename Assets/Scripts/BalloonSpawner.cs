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

    // private List<GameObject> activeballoons = new List<GameObject>();

    private void Start()
    {
        //InvokeRepeating(nameof(Spawnballon), 0.05f, spawnTime);
        Invoke(nameof(Spawnballon),1f);
    }

    void Spawnballon()
    {
        // if (activeballoons.Count >= maxBalloon) return;

        for (int i = 0; i < maxBalloon; i++)
        {
            Vector2 spawnPos = RandomRangeInScreen();
            GameObject balloon = Instantiate(balloonPrefab, spawnPos, Quaternion.Euler(0, 0, Random.Range(-50f, 50f)), transform);
            // activeballoons.Add(balloon);

            Color randomColor = GetColorWithPriority();
            balloon.GetComponent<Balloon>().SetColor(randomColor);
        }
    }

    Color GetColorWithPriority()
    {
        float randomValue = Random.Range(0f, 1f);
        Debug.Log(randomValue);

        if (randomValue < 0.5f)
        {
            return GameManager.instance.targetColor;
        }
        else
        {
            Color otherColor1 = Color.white;
            Color otherColor2 = Color.white;
            int count = 0;

            for (int i = 0; i < GameManager.instance.Size; i++)
            {
                if (GameManager.instance.colorOptions[i] != GameManager.instance.targetColor)
                {
                    if (count == 0)
                        otherColor1 = GameManager.instance.colorOptions[i];
                    else
                        otherColor2 = GameManager.instance.colorOptions[i];

                    count++;
                }
            }

            return Random.Range(0, 2) == 0 ? otherColor1 : otherColor2;
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
