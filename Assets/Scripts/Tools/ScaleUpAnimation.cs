using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class ScaleUpAnimation : MonoBehaviour
{
    [SerializeField] private Ease ease = Ease.OutBounce;
    [SerializeField] private float timeScale = 0.5f;

    private void Start()
    {
        Vector3 firstScale = transform.localScale;
        transform.localScale = Vector3.zero;
        transform.DOScale(firstScale, timeScale).SetEase(ease);
    }
}
