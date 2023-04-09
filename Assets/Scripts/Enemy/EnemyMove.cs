using Elementary.Scripts.LevelManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMove : MonoBehaviour
{
    [SerializeField] private float speed;

    private EnemyHealth health;
    public bool CanWalk { get; set; }
    private void Awake()
    {
        health = GetComponent<EnemyHealth>();

        LevelManager.OnLevelComplete += OnLevelCompelet;
        LevelManager.OnLevelFail += OnLevelFail;
        health.OnDead += OnDead;

        CanWalk = true;
    }
    
    // Update is called once per frame
    void Update()
    {
        if (!CanWalk) return;
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
    private void OnDestroy()
    {
        LevelManager.OnLevelComplete -= OnLevelCompelet;
        LevelManager.OnLevelFail -= OnLevelFail;
        health.OnDead -= OnDead;
    }

    private void OnLevelCompelet(Level level)
    {
        CanWalk = false;
    }
    private void OnLevelFail(Level level)
    {
        CanWalk = false;
    }
    private void OnDead() 
    {
        CanWalk = false;
    }
}
