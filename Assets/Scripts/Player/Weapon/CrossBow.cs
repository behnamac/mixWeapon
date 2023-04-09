using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrossBow : WeaponShoot
{
    [SerializeField] private GameObject fakeBow;

    protected override void Shoot(Rigidbody bullet)
    {
        base.Shoot(bullet);

        fakeBow.SetActive(false);
        Invoke(nameof(ActiveFakeBow), delayShoot / 2);
    }

    private void ActiveFakeBow() 
    {
        fakeBow.SetActive(true);
    }
}
