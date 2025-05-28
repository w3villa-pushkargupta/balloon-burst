using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BalloonSpawner : MonoBehaviour
{
    public GameObject balloonPrefab;
    public float spawnTime = 1f;
    public int maxBalloon = 10;

    public Collider2D boundArea;


 // private List<GameObject> activeballoons = new List<GameObject>();

    private void Start()
    {
        //InvokeRepeating(nameof(Spawnballon), 0.05f, spawnTime);
        //Invoke(nameof(Spawnballon), 1f);
        StartCoroutine(Spawn());

        Invoke(nameof(DeleteCollider), 2f);
    }

    /*
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
    */

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(spawnTime);
        int i = 0;
        while (i < maxBalloon)
        {
            Vector2 spawnPos = RandomRangeOutScreen();
            GameObject balloon = Instantiate(balloonPrefab,spawnPos, Quaternion.Euler(0, 0, Random.Range(-50f, 50f)),transform);

            Color randomColor = GetColorWithPriority();
            balloon.GetComponent<Balloon>().SetColor(randomColor);

            i++;
            yield return null; 
        }
        Vector3 targetPos = CenterboundAreaOnCamera();
        yield return boundArea.transform.DOMove(targetPos, 1f).SetEase(Ease.InOutQuad).WaitForCompletion();
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

    Vector2 RandomRangeOutScreen()
    {
        Bounds bounds = boundArea.bounds;
        float x = Random.Range(bounds.min.x, bounds.max.x);
        float y = Random.Range(bounds.min.y, bounds.max.y);
        return new Vector2(x, y);
    }

 Vector3 CenterboundAreaOnCamera()
    {
        if (boundArea == null || Camera.main == null)
            return Vector3.zero;

        return new Vector3(
            Camera.main.transform.position.x,
            Camera.main.transform.position.y -1f,
            boundArea.transform.position.z 
        );
    }

    void DeleteCollider()
    {
        boundArea.enabled = false;
    }
}
