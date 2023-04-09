using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class HandMove : MonoBehaviour
{
    public Transform targetMove;
    public float timeMove = 1.5f;

    private Vector3 _firstPos;
    // Start is called before the first frame update
    void Start()
    {
        _firstPos = transform.position;

        Move();
    }

    private void Move()
    {
        transform.DOMove(targetMove.position, timeMove).OnComplete(() => 
        {
            transform.position = _firstPos;
            Move();
        });
    }
}
