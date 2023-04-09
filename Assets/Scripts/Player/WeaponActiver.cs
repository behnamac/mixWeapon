using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponActiver : WeaponSlot
{
    public override void OnDrop(PointerEventData eventData)
    {
        base.OnDrop(eventData);
        if (eventData.pointerDrag != null)
        {
            var dragObject = eventData.pointerDrag.transform;
            if (weaponButton == null)
            {
                DropObject(dragObject);
            }
            else
            {
                var otherWeaponButton = dragObject.GetComponent<WeaponButton>();
                var thisWeapnButton = weaponButton;
                Destroy(WeaponController.Instance.ActiveWeapon.gameObject);
                if (otherWeaponButton.WeaponIndex == thisWeapnButton.WeaponIndex)
                {
                    Merge(thisWeapnButton, otherWeaponButton);
                }
                else
                {
                    BackToInventoroy();
                    DropObject(dragObject);
                    if (weaponButton != null)
                        weaponButton.GetComponent<CanvasGroup>().blocksRaycasts = true;
                }
            }
        }
    }
    private void BackToInventoroy() 
    {
        WeaponButtonHolder weaponButtonHolder = null;
        for (int i = 0; i < WeaponController.Instance.WeaponButtonHolders.Length; i++)
        {
            var holder = WeaponController.Instance.WeaponButtonHolders[i];
            if (holder.weaponButton == null)
            {
                weaponButtonHolder = holder;
                break;
            }
        }
        if (weaponButtonHolder == null) return;

        weaponButtonHolder.DropObject(weaponButton.transform);
        weaponButton.gameObject.SetActive(true);
        weaponButton.GetComponent<CanvasGroup>().blocksRaycasts = true;
    }
    public override void DropObject(Transform obj)
    {
        base.DropObject(obj);
        var dragObject = obj;
        var buttonComponent = dragObject.GetComponent<WeaponButton>();
        WeaponController.Instance.SpawnWeapon(buttonComponent.Weapon);
        obj.gameObject.SetActive(false);
    }
    public override void RemoveWeaponButton()
    {
        base.RemoveWeaponButton();
        Destroy(WeaponController.Instance.ActiveWeapon.gameObject);
    }
}
