using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CPI2Controller : MonoBehaviour
{
    [SerializeField] private Button addWeaponButton;
    [SerializeField] private List<Bullet> bullets=new List<Bullet>();

    private int _index;
    private bool _getBullets;
    // Start is called before the first frame update

    private void Awake()
    {
    }
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    GetBullets();
        //    foreach (var item in bullets)
        //    {
        //        item.GetComponent<Collider>().enabled = false;
        //    }

        //}
        //if (Input.GetKeyDown(KeyCode.E))
        //{
        //    GetBullets();
        //    foreach (var item in bullets)
        //    {
        //        item.GetComponent<Collider>().enabled = true;
        //    }
        //}
    }
    private void GetBullets()
    {
        //if (_getBullets) return;
        //_getBullets = true;
        bullets.Clear();
        bullets.AddRange(FindObjectsOfType<Bullet>());


    }

    void Start()
    {
        addWeaponButton.onClick.RemoveAllListeners();
        addWeaponButton.onClick.AddListener(() =>
        {
            if (_index > 2) return;
            WeaponController.Instance.AddWeapon(_index);
            _index++;
        });
    }
}
