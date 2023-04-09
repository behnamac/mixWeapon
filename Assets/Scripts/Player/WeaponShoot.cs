using Elementary.Scripts.LevelManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ShootType { Prejectory, Liner }
public enum ForceType { Explosion, Forward }
public class WeaponShoot : MonoBehaviour
{

    [SerializeField] protected ShootType shootType;
    [SerializeField] protected ForceType forceType;
    [SerializeField] protected Rigidbody bulletPrifab;
    [SerializeField] protected Transform shootPoint;
    [SerializeField] protected Transform lookAtObject; // If this variable is empty, the game object to which the script is connected is used to look
    [SerializeField] protected float bulletSpeed;
    [SerializeField] protected float bulletDamage;
    [SerializeField] protected float explosionRadiusDamage;
    [SerializeField] protected float explosionForce;
    [SerializeField] protected float delayShoot = 0.5f;
    [SerializeField] protected GameObject explosionParticle;
    [SerializeField] protected GameObject muzzleParticle;

    protected Vector3 _targetBulletVelocity;
    protected Vector3 _targetBullet;
    protected float _currentDelayShoot;
    protected List<Rigidbody> _bulletPool;
    protected Transform _bulletPoolParent;
    protected bool _canShoot;

    protected virtual void Awake()
    {
        _bulletPool = new List<Rigidbody>();
        _bulletPoolParent = new GameObject("Bullet Pool").transform;

        _canShoot = true;
    }
    protected virtual void Start()
    {
        LevelManager.OnLevelComplete += OnLevelCompelet;
        LevelManager.OnLevelFail += OnLevelFail;
    }
    // Update is called once per frame
    protected virtual void Update()
    {
        CalculateAim();
        switch (shootType)
        {
            case ShootType.Prejectory:
                Prejectory();
                break;
            case ShootType.Liner:
                Liner();
                break;
        }

        if (_canShoot)
            ActiveBullet();
    }

    protected virtual void OnDestroy()
    {
        if (_bulletPoolParent)
            Destroy(_bulletPoolParent.gameObject);

        LevelManager.OnLevelComplete -= OnLevelCompelet;
        LevelManager.OnLevelFail -= OnLevelFail;
    }

    protected virtual void Prejectory() 
    {
        var targetBullet = _targetBullet;
        float distance = Vector3.Distance(shootPoint.position, targetBullet);
        float time = distance / bulletSpeed;
        _targetBulletVelocity = Calculate.CalculateVelocity(targetBullet, shootPoint.position, time);

        if (lookAtObject)
            lookAtObject.rotation = Quaternion.LookRotation(_targetBulletVelocity);
        else
            transform.rotation = Quaternion.LookRotation(_targetBulletVelocity);
    }

    protected virtual void Liner() 
    {
        if (lookAtObject)
            lookAtObject.LookAt(_targetBullet);
        else
            transform.LookAt(_targetBullet);
        shootPoint.LookAt(_targetBullet);
    }
    protected virtual void CalculateAim() 
    {
        
        RaycastHit hit;
        var screenToWorld = new ScreenToWorldPoint(WeaponController.Instance.WeaponAimLiner.position);
        Ray ray = new Ray(screenToWorld.OriginPoint, screenToWorld.Axis);
        if (Physics.Raycast(ray, out hit))
            _targetBullet = hit.point;
        else
            _targetBullet = screenToWorld.farPoint;
    }

    protected virtual void ActiveBullet()
    {
        _currentDelayShoot -= Time.deltaTime;
        if (_currentDelayShoot > 0) return;
        for (int i = 0; i < _bulletPool.Count; i++)
        {
            if (!_bulletPool[i].gameObject.activeInHierarchy)
            {
                _bulletPool[i].transform.position = shootPoint.position;
                _bulletPool[i].transform.rotation = shootPoint.rotation;
                _bulletPool[i].gameObject.SetActive(true);
                Shoot(_bulletPool[i]);
                _currentDelayShoot = delayShoot;
                return;
            }
        }
        var bullet = Instantiate(bulletPrifab, shootPoint.position, shootPoint.rotation, _bulletPoolParent);
        _bulletPool.Add(bullet);
        Shoot(bullet);
        _currentDelayShoot = delayShoot;

        //Active Muzzle Particle
        var muzzle = Instantiate(muzzleParticle, shootPoint.position, shootPoint.rotation);
        Destroy(muzzle, 0.4f);
    }
    protected virtual void Shoot(Rigidbody bullet) 
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
                bullet.velocity = shootPoint.forward * bulletSpeed;
                break;
        }
    }

    protected virtual void OnLevelCompelet(Level level) 
    {
        _canShoot = false;
    }
    protected virtual void OnLevelFail(Level level)
    {
        _canShoot = false;
    }
}
