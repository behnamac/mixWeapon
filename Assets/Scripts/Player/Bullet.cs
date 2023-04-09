using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Bullet : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayer;

    public ShootType shootType { get; set; }
    public ForceType forceType { get; set; }
    public GameObject ExplosionParticle { get; set; }
    public float Radius { get; set; }
    public float Damage { get; set; }
    public float ExploasionForce { get; set; }

    private Rigidbody _rigidbody;

    private void Awake()
    {
        TryGetComponent(out _rigidbody);
    }

    private void Update()
    {
        if(shootType == ShootType.Prejectory) 
        {
            transform.rotation = Quaternion.LookRotation(_rigidbody.velocity);
        }
    }

    private void OnEnable()
    {
        Vector3 firstScale = transform.localScale;
        transform.localScale = Vector3.zero;
        transform.DOScale(firstScale, 0.2f);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.GetComponent<Bullet>()) return;

        Collider[] enemys = Physics.OverlapSphere(transform.position, Radius, enemyLayer);
        for (int i = 0; i < enemys.Length; i++)
        {
            var enemyHealth = enemys[i].GetComponent<EnemyHealth>();
            if (enemyHealth)
                enemyHealth.TakeDamage(Damage);
        }

        switch (forceType)
        {
            case ForceType.Explosion:
                Exploasion();
                break;
            case ForceType.Forward:
                ForwardForce(other);
                break;
        }

        var particle = Instantiate(ExplosionParticle, transform.position, Quaternion.identity);
        Destroy(particle, 2);
        gameObject.SetActive(false);
    }

    private void Exploasion() 
    {
        Collider[] allObjects = Physics.OverlapSphere(transform.position, Radius);
        for (int i = 0; i < allObjects.Length; i++)
        {
            var rigid = allObjects[i].GetComponent<Rigidbody>();
            var fixedJoint = allObjects[i].GetComponent<FixedJoint>();
            if (fixedJoint)
                Destroy(fixedJoint);
            if (rigid && !rigid.isKinematic)
            {
                rigid.AddForce(Vector3.up * (ExploasionForce / 3));
                rigid.AddExplosionForce(ExploasionForce, transform.position, Radius);
            }
        }
    }
    private void ForwardForce(Collision hit)
    {
        var rigid = hit.collider.GetComponent<Rigidbody>();
        if (rigid && !rigid.isKinematic)
        {
            rigid.AddForce(transform.forward * ExploasionForce);
        }
    }
}
