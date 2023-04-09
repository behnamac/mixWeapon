using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class AlphaImageAnimation : MonoBehaviour
{
    private Image image;
    [Range(0,1)]
    [SerializeField] private float max;
    // Start is called before the first frame update
    void Start()
    {
        image = GetComponent<Image>();
        FadeDown();
    }

    private void FadeUp()
    {
        image.DOFade(max, 0.2f).SetEase(Ease.Linear).OnComplete(FadeDown);
    }
    private void FadeDown()
    {
        image.DOFade(0, 0.2f).SetEase(Ease.Linear).OnComplete(FadeUp);
    }
}
