using Elementary.Scripts.LevelManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class WeaponButton : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler, IDropHandler
{
    public WeaponShoot Weapon;
    public int WeaponIndex;
    public int price = 50;

    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    public WeaponSlot Slot { get; set; }
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        LevelManager.OnLevelComplete += OnLevelCompelet;
        LevelManager.OnLevelFail += OnLevelFail;
    }
    private void OnDestroy()
    {
        LevelManager.OnLevelComplete -= OnLevelCompelet;
        LevelManager.OnLevelFail -= OnLevelFail;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Slot != null)
        {
            Slot.RemoveWeaponButton();
        }
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / WeaponController.Instance.canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        if(eventData.pointerEnter != null) 
        {
            if (!eventData.pointerEnter.GetComponent<WeaponSlot>())
                BackToFirstPoint();
        }
        else
            BackToFirstPoint();

        WeaponController.Instance.CheckActiveSellButton();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnDrop(PointerEventData eventData)
    {
    }

    public void BackToFirstPoint() 
    {
        Slot.DropObject(transform);
    }

    private void OnLevelCompelet(Level level) 
    {
        gameObject.SetActive(false);
    }
    private void OnLevelFail(Level level) 
    {
        gameObject.SetActive(false);
    }
}
