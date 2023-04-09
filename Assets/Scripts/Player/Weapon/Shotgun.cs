using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : WeaponShoot
{
    [SerializeField] private Transform shellPoint;
    [SerializeField] private GameObject shellParticle;
    [SerializeField] private Transform[] otherShootPoints;
    [SerializeField] private float radiusRandomShoot;

    private Vector3 radiusPoint 
    {
        get 
        {
            Vector3 point = shootPoint.position + shootPoint.forward * 3;
            return point;
        }
    }
    private Vector3 randomShootPoint 
    {
        get 
        {
            Vector3 randomPoint = radiusPoint + (Random.insideUnitSphere * radiusRandomShoot);
            return randomPoint;
        }
    }
    
    protected override void ActiveBullet()
    {
        _currentDelayShoot -= Time.deltaTime;
        if (_currentDelayShoot > 0) return;

        SetRandomShootPoint();
        for (int i = 0; i < otherShootPoints.Length; i++)
        {
            var bullet = Instantiate(bulletPrifab, otherShootPoints[i].position, otherShootPoints[i].rotation, _bulletPoolParent);
            _bulletPool.Add(bullet);
            ShootTransform(bullet, otherShootPoints[i]);
            _currentDelayShoot = delayShoot;
        }

        //Active Muzzle Particle
        shootPoint.localEulerAngles = Vector3.zero;
        var muzzle = Instantiate(muzzleParticle, shootPoint.position, shootPoint.rotation);
        Destroy(muzzle, 0.4f);

        //Active Sell Particle
        var shell = Instantiate(shellParticle, shellPoint.position, shellPoint.rotation);
        Destroy(shell, 0.4f);
    }
    protected override void Shoot(Rigidbody bullet)
    {
    }
    private void ShootTransform(Rigidbody bullet, Transform shootAxis)
    {
        var bulletComponent = bullet.GetComponent<Bullet>();
        bulletComponent.shootType = shootType;
        bulletComponent.forceType = forceType;
        bulletComponent.Damage = bulletDamage;
        bulletComponent.Radius = explosionRadiusDamage;
        bulletComponent.ExploasionForce = explosionForce;
        bulletComponent.ExplosionParticle = explosionParticle;
        switch (shootType)
        {
            case ShootType.Prejectory:
                bullet.velocity = _targetBulletVelocity;
                break;
            case ShootType.Liner:
                bullet.velocity = shootAxis.forward * bulletSpeed;
                break;
        }
    }
    [ContextMenu("Set Random Shoot Point Rotation")]
    public void SetRandomShootPoint() 
    {
        for (int i = 0; i < otherShootPoints.Length; i++)
        {
            otherShootPoints[i].LookAt(randomShootPoint);
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        for (int i = 0; i < otherShootPoints.Length; i++)
        {
            Gizmos.DrawRay(otherShootPoints[i].position, otherShootPoints[i].forward * 3);
        }

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(radiusPoint, radiusRandomShoot);
    }
}
