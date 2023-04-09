using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CPI1Controller : MonoBehaviour
{
    [SerializeField] private Button addWeaponButton;
    [SerializeField] private int weaponIndex;
    // Start is called before the first frame update
    void Start()
    {
        addWeaponButton.onClick.RemoveAllListeners();
        addWeaponButton.onClick.AddListener(() =>
        {
            WeaponController.Instance.AddWeapon(weaponIndex);
            weaponIndex++;
            if (weaponIndex > 1)
                weaponIndex = 0;
        });
    }
}
