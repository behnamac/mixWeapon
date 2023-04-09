using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SMG : WeaponShoot
{
    [SerializeField] private Transform shellPoint;
    [SerializeField] private GameObject shellParticle;

    protected override void Shoot(Rigidbody bullet)
    {
        base.Shoot(bullet);

        var shell = Instantiate(shellParticle, shellPoint.position, shellPoint.rotation);
        Destroy(shell, 0.4f);
    }
}
