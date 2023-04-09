using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponSellButton : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            var dragObject = eventData.pointerDrag;
            var weaponButton = dragObject.GetComponent<WeaponButton>();
            UIController.instance.AddCoin(weaponButton.price);
            Destroy(dragObject);
            WeaponController.Instance.CheckActiveSellButton();
        }
    }
}
