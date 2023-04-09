using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponButtonHolder : WeaponSlot
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
                if (otherWeaponButton.WeaponIndex == thisWeapnButton.WeaponIndex)
                {
                    Merge(thisWeapnButton, otherWeaponButton);
                }
                else
                {
                    otherWeaponButton.BackToFirstPoint();
                    if (weaponButton != null)
                        weaponButton.GetComponent<CanvasGroup>().blocksRaycasts = true;
                }
            }
        }
    }
}
