using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonSpawner : MonoBehaviour
{
    public GameObject balloonPrefab;
    private float spawnTime = 0.5f;

    private Collider2D boundArea;
    private int targetColorReturnCount = 0; 

    void Start()
    {
        boundArea = GetComponent<Collider2D>();
        StartCoroutine(Spawn());

        Invoke(nameof(DeleteCollider), 3f);
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(spawnTime);
 
        List<Vector2> usedPositions = new List<Vector2>();
        float colliderRadius = balloonPrefab.GetComponent<CircleCollider2D>().radius * balloonPrefab.transform.localScale.x;

        int i = 0;
        while (i < GameManager.instance.maxBalloon)
        {
            Vector2 newPos = RandomRangeInScreen(usedPositions, colliderRadius);
            usedPositions.Add(newPos);

            float oldPosY = RandomRangeOutScreen(); 

            GameObject balloon = Instantiate(balloonPrefab, transform.position - new Vector3(0,6f,0), Quaternion.Euler(0, 0, Random.Range(-50f, 50f)), transform);

            Color randomColor = GetColorWithPriority();
            balloon.GetComponent<Balloon>().SetColor(randomColor);

            yield return new WaitForSeconds(Random.Range(0.01f, 0.08f));
            balloon.transform.DOMove(newPos, 0.5f).From(new Vector2(newPos.x, oldPosY)).SetEase(Ease.OutBack);
            i++;
        }
    }

    Color GetColorWithPriority()
    {
        float randomValue = Random.Range(0f, 1f);

        if (randomValue<0.8f && targetColorReturnCount < GameManager.instance.balloonTargetCount)
        {
            targetColorReturnCount++;
            return GameManager.instance.targetColor;
        }
        else
        {
            return SetOtherColor();
        }
    }

    private Color SetOtherColor()
    {
        Color otherColor1 = Color.white;
        Color otherColor2 = Color.white;
        int count = 0;

        for (int i = 0; i < GameManager.instance.colorOptions.Length; i++)
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

    Vector2 RandomRangeInScreen(List<Vector2> usedPositions, float colliderRadius)
    {
        Bounds bounds = boundArea.bounds;
        Vector2 spawnPos;
        int maxAttempts = 100;
        int attempts = 0;

        do
        {
            float x = Random.Range(bounds.min.x + colliderRadius, bounds.max.x - colliderRadius);
            float y = Random.Range(bounds.min.y + colliderRadius, bounds.max.y - colliderRadius);
            spawnPos = new Vector2(x, y);

            bool tooClose = false;
            foreach (var pos in usedPositions)
            {
                if (Vector2.Distance(pos, spawnPos) < colliderRadius * 3f)
                {
                    tooClose = true;
                    break;
                }
            }

            if (!tooClose)
                break;

            attempts++;
        }
        while (attempts < maxAttempts);

        return spawnPos;
    }

    float RandomRangeOutScreen()
    {
        Bounds bounds = boundArea.bounds;
        float y = bounds.min.y - Random.Range(0f,2f);
        return y;
    }

    void DeleteCollider()
    {
        boundArea.enabled = false;
    }
}
