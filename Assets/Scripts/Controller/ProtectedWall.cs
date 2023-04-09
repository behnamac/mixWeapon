using Elementary.Scripts.LevelManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProtectedWall : MonoBehaviour
{
    [SerializeField] private float maxHealth;
    [SerializeField] private GameObject[] inactiveObjects;
    [SerializeField] private GameObject[] activeObjects;
    [SerializeField] private List<GameObject> cracks;
    [SerializeField] private Transform hitParticle;

    private float _currentHealth;
    private bool _break;
    private void Awake()
    {
        _currentHealth = maxHealth;
    }
    public void TakeDamage(float damageValue, Transform damageObject = null) 
    {
        if (_break) return;

        for (int i = 0; i < damageValue; i++)
        {
            ActiveRandomCrack();
        }
        if (damageObject != null)
        {
            _currentHealth -= damageValue;
            Vector3 spawnPoint = transform.position;
            spawnPoint.x = damageObject.position.x;
            spawnPoint.y = 9;

            var particle = Instantiate(hitParticle, spawnPoint, transform.rotation);
            Destroy(particle.gameObject, 6f);
        }


        if(_currentHealth <= 0) 
        {
            LevelManager.Instance.LevelFail();
            _break = true;

            for (int i = 0; i < inactiveObjects.Length; i++)
            {
                inactiveObjects[i].SetActive(false);
            }
            for (int i = 0; i < activeObjects.Length; i++)
            {
                activeObjects[i].SetActive(true);
            }
        }
    }

    private void ActiveRandomCrack() 
    {
        if (cracks.Count < 1) return;
        int randomIndex = Random.Range(0, cracks.Count);
        cracks[randomIndex].SetActive(true);
        cracks.RemoveAt(randomIndex);
    }
}
