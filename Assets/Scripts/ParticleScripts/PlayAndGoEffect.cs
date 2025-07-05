using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEditor;

public class PlayAndGoEffect : MonoBehaviour
{
    [SerializeField] private GameObject starPrefab;

    [SerializeField] private Button initialButton;
    [SerializeField] private Button targetButton;

    public void PlayEffect()
    {
        var star = Instantiate(starPrefab, initialButton.transform.position, Quaternion.identity);

        RectTransform targetRect = targetButton.GetComponent<RectTransform>();
        star.transform.DOMove(targetRect.position, 1f).From(initialButton.transform.position).SetDelay(0.5f).SetEase(Ease.InOutQuad);
    }



}
 