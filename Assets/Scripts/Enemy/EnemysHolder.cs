using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemysHolder : MonoBehaviour
{
    public static EnemysHolder Instance;

    [SerializeField] private EnemyHolder[] enemyHolders;
    private Dictionary<string, EnemyHealth> enemies;

    private void Awake()
    {
        Instance = this;

        enemies = new Dictionary<string, EnemyHealth>();
        for (int i = 0; i < enemyHolders.Length; i++)
        {
            enemies.Add(enemyHolders[i].enemyName, enemyHolders[i].enemy);
        }
    }

    public EnemyHealth GetEnemy(string enemyName) 
    {
        return enemies[enemyName];
    }

    [System.Serializable]
    private class EnemyHolder 
    {
        public string enemyName;
        public EnemyHealth enemy;
    }
}
