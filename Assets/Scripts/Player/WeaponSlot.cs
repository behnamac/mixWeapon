using Elementary.Scripts.Data.Management;
using Elementary.Scripts.LevelManagement;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponSlot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string saveId;
    [HideInInspector] public WeaponButton weaponButton;

    private IEnumerator Start()
    {
        LevelManager.OnLevelComplete += OnLevelCompelet;
        LevelManager.OnLevelFail += OnLevelFail;

        yield return new WaitForEndOfFrame();
        int weaponData = DataManager.GetWithJson<int>(saveId, -1);
        if (weaponData != -1) 
        {
            var weaponButtonObject = WeaponController.Instance.SpawnWeaponButton(weaponData);
            DropObject(weaponButtonObject.transform);
        }
    }
    private void OnDestroy()
    {
        LevelManager.OnLevelComplete -= OnLevelCompelet;
        LevelManager.OnLevelFail -= OnLevelFail;
    }
    public virtual void OnDrop(PointerEventData eventData)
    {
        
    }

    protected virtual void Merge(WeaponButton thisButton, WeaponButton otherButton)
    {
        if (thisButton.WeaponIndex + 1 >= WeaponController.Instance.WeaponButtons.Length)
        {
            otherButton.BackToFirstPoint();
            return;
        }
        var newWeaponButton = WeaponController.Instance.SpawnWeaponButton(thisButton.WeaponIndex + 1);
        DropObject(newWeaponButton.transform);
        Destroy(thisButton.gameObject);
        Destroy(otherButton.gameObject);
    }
    public virtual void DropObject(Transform obj)
    {
        var dragObject = obj;
        dragObject.position = transform.position;
        var buttonComponent = dragObject.GetComponent<WeaponButton>();
        buttonComponent.Slot = this;
        weaponButton = buttonComponent;
        DataManager.SaveWithJson(saveId, weaponButton.WeaponIndex);
        WeaponController.Instance.CheckActiveSellButton();
    }
    public virtual void RemoveWeaponButton() 
    {
        weaponButton = null;
        DataManager.SaveWithJson(saveId, -1);
    }
    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            if (weaponButton != null)
                weaponButton.GetComponent<CanvasGroup>().blocksRaycasts = false;
        }
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (eventData.pointerDrag != null)
        {
            if (weaponButton != null)
                weaponButton.GetComponent<CanvasGroup>().blocksRaycasts = true;
        }
    }

    private void OnLevelCompelet(Level level)
    {
        if (weaponButton == null) return;
        weaponButton.gameObject.SetActive(false);
    }
    private void OnLevelFail(Level level)
    {
        if (weaponButton == null) return;
        weaponButton.gameObject.SetActive(false);
    }
}
