using System.Collections;
using System.Collections.Generic;
using Elementary.Scripts.LevelManagement;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyWaveController : MonoBehaviour
{
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private Transform enemyTargetPoint;
    [SerializeField] private WaveHolder[] waveHolders;

    private int _waveIndex;
    private int _allThisWaveEnmeysNumber; 
    private int _enemyDeadNumber;
    public int AllEnemyNumber { get; set; }
    private void Awake()
    {
        for (int i = 0; i < waveHolders.Length; i++)
        {
            waveHolders[i].waveController = this;
            waveHolders[i].SpawnEnemys(spawnPoint);
            waveHolders[i].InactiveAllEnemys();
        }
        StartCoroutine(waveHolders[_waveIndex].ActiveAllEnemys());
        _allThisWaveEnmeysNumber = waveHolders[_waveIndex].AllEnemys.Count + waveHolders[_waveIndex].AllGaints.Count;
    }
    public void OnEnemyDead() 
    {
        _enemyDeadNumber++;
        if (_enemyDeadNumber >= AllEnemyNumber)
        {
            LevelManager.Instance.LevelComplete();
            return;
        }

        if (_waveIndex >= waveHolders.Length)
            return;

        _allThisWaveEnmeysNumber--;
        float allEnmeyNumberWave = (float)waveHolders[_waveIndex].AllEnemys.Count + (float)waveHolders[_waveIndex].AllGaints.Count;
        float present = ((float)_allThisWaveEnmeysNumber / allEnmeyNumberWave) * 100;
        if (present <= 35)
            NextWave();
    }
    private void NextWave() 
    {
        _waveIndex++;
        if (_waveIndex >= waveHolders.Length) 
            return;

        _allThisWaveEnmeysNumber += waveHolders[_waveIndex].AllEnemys.Count + waveHolders[_waveIndex].AllGaints.Count;
        StartCoroutine(waveHolders[_waveIndex].ActiveAllEnemys());
    }

    [System.Serializable]
    private class WaveHolder 
    {
        public string[] Enemys;
        public string[] GaintEnemys;
        public float EnemySpacing;
        public float GaintSpacing;
        //public float EnemyHealth = 100;
        public float GaintHealth = 300;
        public Vector2 grid;

        [HideInInspector] public List<GameObject> AllEnemys;
        [HideInInspector] public List<GameObject> AllGaints;
        [HideInInspector] public EnemyWaveController waveController;
        public void SpawnEnemys(Transform spawnPoint) 
        {
            //Spawn Normal Enemy
            float offsetX = ((grid.x - 1) * EnemySpacing) / 2;
            float offsetY = ((grid.y - 1) * EnemySpacing) / 2;
            GameObject waveParent = new GameObject("Wave");
            waveParent.transform.parent = spawnPoint;
            for (int i = 0; i < grid.x; i++)
            {
                for (int j = 0; j < grid.y; j++)
                {
                    Vector3 pos = spawnPoint.position;
                    pos.x += (EnemySpacing * i) - offsetX;
                    pos.z += (EnemySpacing * j)/* - offsetY*/;
                    pos.x += Random.Range(-(EnemySpacing / 2), EnemySpacing / 2);
                    //pos.z += Random.Range(-(EnemySpacing / 2), EnemySpacing / 2);
                    var enemy = Instantiate(GetRandomEnemy(), pos, spawnPoint.rotation, waveParent.transform);
                    enemy.transform.LookAt(waveController.enemyTargetPoint.position);
                    //enemy.ChangeMaxHealth(EnemyHealth);
                    enemy.waveController = waveController;
                    AllEnemys.Add(enemy.gameObject);
                    waveController.AllEnemyNumber++;
                }
            }

            //Spawn Gaint Enemy
            float offset = ((GaintEnemys.Length - 1) * EnemySpacing) / 2;
            for (int i = 0; i < GaintEnemys.Length; i++)
            {
                Vector3 pos;
                if (grid == Vector2.zero)
                    pos = spawnPoint.position;
                else
                    pos = new Vector3(spawnPoint.position.x,spawnPoint.position.y,
                        AllEnemys[AllEnemys.Count - 1].transform.position.z + GaintSpacing);
                pos.x += (GaintSpacing * i) - offset;
                var enemy = Instantiate(EnemysHolder.Instance.GetEnemy(GaintEnemys[i]), pos, spawnPoint.rotation, waveParent.transform);
                enemy.transform.LookAt(waveController.enemyTargetPoint.position);
                enemy.ChangeMaxHealth(GaintHealth);
                enemy.waveController = waveController;
                AllGaints.Add(enemy.gameObject);
                waveController.AllEnemyNumber++;
            }
        }
        public IEnumerator ActiveAllEnemys() 
        {
            List<GameObject> enemysActiver = new List<GameObject>();
            List<GameObject> gaintsActiver = new List<GameObject>();
            enemysActiver.AddRange(AllEnemys);
            gaintsActiver.AddRange(AllGaints);
            for (int i = 0; i < AllEnemys.Count; i++)
            {
                yield return new WaitForSeconds(0.1f);
                int randomIndex = Random.Range(0, enemysActiver.Count);
                enemysActiver[randomIndex].SetActive(true);
                enemysActiver.RemoveAt(randomIndex);
            }
            for (int i = 0; i < AllGaints.Count; i++)
            {
                yield return new WaitForSeconds(0.1f);
                int randomIndex = Random.Range(0, gaintsActiver.Count);
                gaintsActiver[randomIndex].SetActive(true);
                gaintsActiver.RemoveAt(randomIndex);
            }
        }
        public void InactiveAllEnemys()
        {
            for (int i = 0; i < AllEnemys.Count; i++)
            {
                AllEnemys[i].SetActive(false);
            }
            for (int i = 0; i < AllGaints.Count; i++)
            {
                AllGaints[i].SetActive(false);
            }
        }
        
        private EnemyHealth GetRandomEnemy() 
        {
            int randomIndex = Random.Range(0, Enemys.Length);
            return EnemysHolder.Instance.GetEnemy(Enemys[randomIndex]);
        }
    }
}
