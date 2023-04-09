using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlartChecker : MonoBehaviour
{
    [SerializeField] private Vector3 size;
    [SerializeField] private GameObject alartObject;
    
    private void FixedUpdate()
    {
        Collider[] colliders = Physics.OverlapBox(transform.position, size);
        bool active = false;
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].GetComponent<EnemyHealth>()) 
            {
                active = true;
            }
        }

        alartObject.SetActive(active);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position, size);
    }
}
