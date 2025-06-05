using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class BalloonSpawner : MonoBehaviour
{
    [SerializeField] private Balloon balloonPrefab;

    private BoxCollider2D boundArea;
    private int targetColorReturnCount = 0;

    public Action OnCoroutineComplete;

    [Header("Collider Screen Space")]
    [SerializeField] float firstSpawnOffsetY = 7f;

    [SerializeField] private float marginX = 1f;
    [SerializeField] private float marginY = 3f;
    [SerializeField] private float yOffset = -1.25f;


    private void Start()
    {
        Camera cam = Camera.main;
        boundArea = GetComponent<BoxCollider2D>();

        Vector2 bottomLeft = cam.ScreenToWorldPoint(new Vector2(0, 0));
        Vector2 topRight = cam.ScreenToWorldPoint(new Vector2(Screen.width, Screen.height));

        Vector2 screenSize = topRight - bottomLeft;
        Vector2 adjustedSize = screenSize - new Vector2(marginX, marginY);

        boundArea.size = adjustedSize;
        boundArea.offset = new Vector2(0, yOffset);
  

        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        float firstSpawnTime = 0.5f;
        yield return new WaitForSeconds(firstSpawnTime);   

        List<Vector2> usedPositions = new List<Vector2>();
        float colliderRadius = balloonPrefab.GetComponentInChildren<CircleCollider2D>().radius * balloonPrefab.transform.localScale.x;

        int i = 0;
        while (i < GameManager.instance.maxBalloon)
        {
            Vector2 newPos = RandomRangeInScreen(usedPositions, colliderRadius);
            usedPositions.Add(newPos);

            float oldPosY = boundArea.bounds.min.y - Random.Range(0f, firstSpawnOffsetY);

            Balloon balloon = Instantiate(balloonPrefab, transform.position - new Vector3(0,firstSpawnOffsetY,0), Quaternion.Euler(0, 0, Random.Range(-50f, 50f)),transform);

            Color randomColor = GetColorWithPriority();
            balloon.SetColor(randomColor, newPos);

            yield return new WaitForSeconds(Random.Range(0.01f, 0.08f));

           balloon. transform.DOMove(newPos, 0.5f).From(new Vector2(newPos.x, oldPosY)).SetEase(Ease.OutBack)
                .OnComplete(() =>
                {
                    balloonPrefab.TimeToAnimate();
                });
            i++;
        }
        boundArea.enabled = false;

        OnCoroutineComplete?.Invoke();
    }

    Color GetColorWithPriority()
    {
        float randomValue = Random.Range(0f, 1f);

        if (randomValue<0.7f && targetColorReturnCount < GameManager.instance.balloonTargetCount)
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
        int maxAttempts = 120;
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

        if (attempts >= maxAttempts)
        {
            Debug.LogWarning("Could not find a valid spawn position." + attempts);
        }

        return spawnPos;
    }

    Vector2 GridSampleInScreen(List<Vector2> usedPositions, float colliderRadius)
    {
        Bounds bounds = boundArea.bounds;
        float spacing = colliderRadius * 3f;

        List<Rect> gridCells = new List<Rect>();

        for (float x = bounds.min.x + colliderRadius; x <= bounds.max.x - colliderRadius; x += spacing)
        {
            for (float y = bounds.min.y + colliderRadius; y <= bounds.max.y - colliderRadius; y += spacing)
            {
                gridCells.Add(new Rect(x - spacing / 2f, y - spacing / 2f, spacing, spacing));
            }
        }

        Shuffle(gridCells);

        foreach (var cell in gridCells)
        {
            Vector2 randomPos = new Vector2(
                Random.Range(cell.xMin + colliderRadius, cell.xMax - colliderRadius),
                Random.Range(cell.yMin + colliderRadius, cell.yMax - colliderRadius)
            );

            if (!IsTooCloseToOthers(randomPos, usedPositions, colliderRadius * 3f))
            {
                return randomPos;
            }
        }

        Debug.LogWarning("Could not find a valid spawn position in grid.");
        return Vector2.zero;
    }

    bool IsTooCloseToOthers(Vector2 position, List<Vector2> others, float minDistance)
    {
        return others.Any(p => Vector2.Distance(p, position) < minDistance);
    }

    void Shuffle<T>(List<T> list)
    {
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[i], list[j]) = (list[j], list[i]);
        }
    }
}
