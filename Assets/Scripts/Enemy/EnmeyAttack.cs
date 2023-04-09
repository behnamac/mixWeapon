using Elementary.Scripts.LevelManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnmeyAttack : MonoBehaviour
{
    [SerializeField] private float damage = 5;

    private ProtectedWall _protectedWall;
    private EnemyMove _enemyMove;

    private void Awake()
    {
        _enemyMove = GetComponent<EnemyMove>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out _protectedWall)) 
        {
            _enemyMove.CanWalk = false;

            InvokeRepeating(nameof(Attack), 2, 2);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out _protectedWall))
        {
            _enemyMove.CanWalk = true;

            CancelInvoke(nameof(Attack));
        }
    }

    public void Attack() 
    {
        _protectedWall.TakeDamage(damage, transform);
    }
}
