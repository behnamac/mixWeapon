using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CPI4Controller : MonoBehaviour
{
    [SerializeField] private Button addWeaponButton;

    private int _index;
    // Start is called before the first frame update
    void Start()
    {
        addWeaponButton.onClick.RemoveAllListeners();
        addWeaponButton.onClick.AddListener(() =>
        {
            _index = Random.Range(0, 5);
            WeaponController.Instance.AddWeapon(_index);
        });
    }
}
