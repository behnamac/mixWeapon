using Elementary.Scripts.Data.Management;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialController : MonoBehaviour
{
    [SerializeField] private GameObject[] handAddItem;
    [SerializeField] private GameObject[] handActiveWeapon;
    [SerializeField] private HandMove handMergeInWeaponActiver;
    [SerializeField] private GameObject[] texts;
    [SerializeField] private HandMove handMergeInInventory;

    [Space(5)]

    [SerializeField] private Button addItemButton;
    [SerializeField] private WeaponActiver weaponActiver;
    [SerializeField] private WeaponButtonHolder[] weaponButtonHolders;

    private void Awake()
    {
        //if(DataManager.Get<int>("level-number") > 1) 
        //{
        //    enabled = false;
        //}

        addItemButton.onClick.AddListener(ActiveWeaponActiverTutorial);
    }
    // Start is called before the first frame update
    void Start()
    {
        ActiveAddItemTutorial();

        StartCoroutine(CheckWeaponActiverForMerge());
        StartCoroutine(CheckInventoryForMerge());
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0)) 
        {
            InactiveAllTutorials();
        }
    }

    private void ActiveAddItemTutorial()
    {
        if (DataManager.Get<bool>("handAddItem")) return;
        foreach (var item in handAddItem)
        {
            item.SetActive(true);
        }
        Time.timeScale = 0.1f;
        DataManager.Save<bool>("handAddItem", true);
    }
    private void ActiveWeaponActiverTutorial()
    {
        if (DataManager.Get<bool>("handActiveWeapon")) return;
        foreach (var item in handActiveWeapon)
        {
            item.SetActive(true);
        }
        Time.timeScale = 0.1f;
        DataManager.Save<bool>("handActiveWeapon", true);
    }
    private void ActiveMergeInWeaponActiverTutorial()
    {
        if (DataManager.Get<bool>("handMergeInWeaponActiver")) return;
        handMergeInWeaponActiver.gameObject.SetActive(true);
        if (texts.Length > 0)
            texts[0].gameObject.SetActive(true);

        Time.timeScale = 0.1f;
        DataManager.Save<bool>("handMergeInWeaponActiver", true);
    }
    private void ActiveMergeInInventoryTutorial()
    {
        if (DataManager.Get<bool>("handMergeInInventory")) return;
        handMergeInInventory.gameObject.SetActive(true);
        if (texts.Length > 0)
            texts[1].gameObject.SetActive(true);

        Time.timeScale = 0.1f;
        DataManager.Save<bool>("handMergeInInventory", true);
    }

    private void InactiveAllTutorials() 
    {
        foreach (var item in handAddItem)
        {
            item.SetActive(false);

        }
        foreach (var item in handActiveWeapon)
        {
            item.SetActive(false);
        }
        handMergeInWeaponActiver.gameObject.SetActive(false);
        handMergeInInventory.gameObject.SetActive(false);

        for (int i = 0; i < texts.Length; i++)
        {
            texts[i].gameObject.SetActive(false);
            texts[i].gameObject.SetActive(false);
        }

        Time.timeScale = 1f;
    }

    private IEnumerator CheckWeaponActiverForMerge() 
    {
        while (!DataManager.Get<bool>("WeaponActiverTutorial"))
        {
            for (int i = 0; i < weaponButtonHolders.Length; i++)
            {
                if (weaponActiver.weaponButton != null && weaponButtonHolders[i].weaponButton != null)
                {
                    if (weaponButtonHolders[i].weaponButton.WeaponIndex == weaponActiver.weaponButton.WeaponIndex)
                    {
                        handMergeInWeaponActiver.transform.position = weaponButtonHolders[i].transform.position;
                        ActiveMergeInWeaponActiverTutorial();
                    }
                }
            }
            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator CheckInventoryForMerge() 
    {
        while (!DataManager.Get<bool>("handMergeInInventory"))
        {
            for (int i = 0; i < weaponButtonHolders.Length; i++)
            {
                for (int j = 0; j < weaponButtonHolders.Length; j++)
                {
                    if (i != j) 
                    {
                        if (weaponButtonHolders[j].weaponButton != null && weaponButtonHolders[i].weaponButton != null)
                        {
                            if (weaponButtonHolders[i].weaponButton.WeaponIndex == weaponButtonHolders[j].weaponButton.WeaponIndex)
                            {
                                handMergeInInventory.transform.position = weaponButtonHolders[i].transform.position;
                                handMergeInInventory.targetMove = weaponButtonHolders[j].transform;
                                ActiveMergeInInventoryTutorial();
                            }
                        }
                    }
                }
            }
            yield return new WaitForSeconds(1);
        }
    }
}
