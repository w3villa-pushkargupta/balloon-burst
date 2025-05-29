using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject balloonPrefab;
    public float spawnTime = 1f;
    public int maxBalloon = 10;
    public Transform Parent;

    public Collider2D boundArea;


    private void Start()
    {
        StartCoroutine(Spawn());

        Invoke(nameof(DeleteCollider), 2f);
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(spawnTime);
        int i = 0;

        List<Vector2> usedPositions = new List<Vector2>();
        float colliderRadius = balloonPrefab.GetComponent<CircleCollider2D>().radius * balloonPrefab.transform.localScale.x;

        while (i < maxBalloon)
        {
            Vector2 pos = new Vector2(Random.RandomRange(-8, 8/*Screen.width*/), Random.RandomRange(-5, 4/*Screen.height*/));//RandomRangeOutScreen(usedPositions, colliderRadius);
            usedPositions.Add(pos);

            GameObject balloon = Instantiate(balloonPrefab, Parent.position, Quaternion.Euler(0, 0, Random.Range(-50f, 50f)), transform);

            Color randomColor = GetColorWithPriority();
            balloon.GetComponent<Balloon>().SetColor(randomColor);

            i++;
            yield return new WaitForSeconds(Random.Range(0.05f, 0.15f));

            balloon.transform.DOMove(pos, 0.5f).From(new Vector2(pos.x, Parent.position.y)).SetEase(Ease.OutBack);
        }
        //Vector3 targetPos = CenterboundAreaOnCamera();
        //yield return boundArea.transform.DOMove(targetPos, 1f).SetEase(Ease.InOutQuad).WaitForCompletion();
    }

    Color GetColorWithPriority()
    {
        float randomValue = Random.Range(0f, 1f);

        if (randomValue < 0.7f)
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

    Vector2 RandomRangeOutScreen(List<Vector2> usedPositions, float colliderRadius)
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
                if (Vector2.Distance(pos, spawnPos) < colliderRadius * 2f)
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

    Vector3 CenterboundAreaOnCamera()
    {
        if (boundArea == null || Camera.main == null)
            return Vector3.zero;

        return new Vector3(
            Camera.main.transform.position.x,
            Camera.main.transform.position.y - 1f,
            boundArea.transform.position.z
        );
    }

    void DeleteCollider()
    {
        boundArea.enabled = false;
    }
}


