using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HandBounce : MonoBehaviour
{
    public float animationTime = 0.2f;
    private Vector3 _firstScale;
    // Start is called before the first frame update
    void Start()
    {
        _firstScale = transform.localScale;

        ScaleDown();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ScaleDown()
    {
        transform.DOScale(_firstScale / 2, animationTime).OnComplete(ScaleUp);
    }
    private void ScaleUp()
    {
        transform.DOScale(_firstScale, animationTime).OnComplete(ScaleDown);
    }
}
