using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CPI5Controller : MonoBehaviour
{
    [SerializeField] private Button addWeaponButton;

    // Start is called before the first frame update
    void Start()
    {
        addWeaponButton.onClick.RemoveAllListeners();
        
        addWeaponButton.onClick.AddListener(() =>
        {
            WeaponController.Instance.AddWeapon(0);
        });
    }
}
