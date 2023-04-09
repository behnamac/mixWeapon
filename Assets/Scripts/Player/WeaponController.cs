using Elementary.Scripts.Data.Management;
using Elementary.Scripts.LevelManagement;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class WeaponController : MonoBehaviour
{
    public static WeaponController Instance;

    public Canvas canvas;
    public Transform WeaponAimLiner;
    public Button SellButton;
    [SerializeField] private int defaultCountWeaponButton;
    [SerializeField] private int firstPriceWeapon;
    [SerializeField] private int weaponPriceRate;
    [SerializeField] private TextMeshProUGUI weaponPriceText;
    [SerializeField] private WeaponButtonHolder[] weaponButtonHolders;
    [SerializeField] private WeaponButton[] weaponButtons;
    [SerializeField] private Transform spawnWeaponPoint;
    [SerializeField] private Button addWeaponButton;
    [SerializeField] private FixedTouchField touchField;
    [SerializeField] private float speedRotateCamera;
    [SerializeField]private Vector2 MaxClampAim;
    [SerializeField]private Vector2 MinClampAim;

    private int _weaponPrice;
    private bool _canAimMove;
    private Transform _camera;
    private Vector3 _targetRotate;
    public WeaponShoot ActiveWeapon { get; set; }
    public WeaponButton[] WeaponButtons => weaponButtons;
    public WeaponButtonHolder[] WeaponButtonHolders => weaponButtonHolders;
    public FixedTouchField TouchField => touchField;
    private void Awake()
    {
        Instance = this;

        _canAimMove = true;
        addWeaponButton.onClick.AddListener(() => 
        {
            AddWeapon();
        });

        _weaponPrice = DataManager.GetWithJson("AddWeaponPrice", firstPriceWeapon);
        weaponPriceText.text = _weaponPrice.ToString();

        _camera = Camera.main.transform;
        _targetRotate = _camera.eulerAngles;

        LevelManager.OnLevelComplete += OnLevelCompelet;
        LevelManager.OnLevelFail += OnLevelFail;
        UIController.instance.OnAddCoin += OnAddCoin;
    }
    private IEnumerator Start()
    {
        CheckActiveSellButton();
        CheckCoin();

        for (int i = 0; i < defaultCountWeaponButton; i++)
        {
            var weaponButton = Instantiate(weaponButtons[0], canvas.transform);
            yield return new WaitForEndOfFrame();
            weaponButtonHolders[i].DropObject(weaponButton.transform);
        }
    }
    private void Update()
    {
        if (_canAimMove)
            AimMove();
    }
    private void OnDestroy()
    {
        LevelManager.OnLevelComplete -= OnLevelCompelet;
        LevelManager.OnLevelFail -= OnLevelFail;
        UIController.instance.OnAddCoin -= OnAddCoin;
    }

    private void AimMove() 
    {
        _targetRotate.x -= touchField.TouchDist.y * speedRotateCamera * Time.deltaTime;
        if (ActiveWeapon)
            _targetRotate.y += touchField.TouchDist.x * speedRotateCamera * Time.deltaTime;
        _targetRotate.x = Mathf.Clamp(_targetRotate.x, MinClampAim.y, MaxClampAim.y);
        _targetRotate.y = Mathf.Clamp(_targetRotate.y, MinClampAim.x, MaxClampAim.x);

        _camera.eulerAngles = _targetRotate;
    }

    public void AddWeapon(int targetIndex = -1) 
    {
        if (UIController.instance.CurrentCoin < _weaponPrice) return;

        WeaponButtonHolder weaponButtonHolder = GetEmptyButtonHolder();
        if (weaponButtonHolder == null) return;

        UIController.instance.AddCoin(-_weaponPrice);
        int level = DataManager.Get<int>("level-number");

        int randomIndex = 0;
        if (targetIndex == -1)
        {
            if (level <= 3)
                randomIndex = Random.Range(0, 4);
            else if (level >= 4 && level <= 6)
                randomIndex = Random.Range(0, 7);
            else if (level >= 7)
                randomIndex = Random.Range(0, weaponButtons.Length);
        }
        else
            randomIndex = targetIndex;

        var weaponButton = SpawnWeaponButton(randomIndex);
        weaponButtonHolder.DropObject(weaponButton.transform);
        _weaponPrice += weaponPriceRate;
        weaponPriceText.text = _weaponPrice.ToString();
        DataManager.SaveWithJson("AddWeaponPrice", _weaponPrice);
    }
    public void SpawnWeapon(WeaponShoot weapon) 
    {
        var weaponSpawn = Instantiate(weapon, spawnWeaponPoint.position, spawnWeaponPoint.rotation, spawnWeaponPoint);
        ActiveWeapon = weaponSpawn;
    }
    public WeaponButton SpawnWeaponButton(int weaponButtonIndex) 
    {
        var button = Instantiate(weaponButtons[weaponButtonIndex], canvas.transform);
        return button;
    }

    public void CheckActiveSellButton() 
    {
        bool active = false;
        for (int i = 0; i < weaponButtonHolders.Length; i++)
        {
            if (weaponButtonHolders[i].weaponButton != null) 
            {
                active = true;
            }
        }
        SellButton.interactable = active;
    }
    public void LookAtCamera(Transform target) 
    {
        _camera.DOLookAt(target.position, 0.3f);
    }

    public WeaponButtonHolder GetEmptyButtonHolder() 
    {
        WeaponButtonHolder weaponButtonHolder = null;
        for (int i = 0; i < weaponButtonHolders.Length; i++)
        {
            if (weaponButtonHolders[i].weaponButton == null)
            {
                weaponButtonHolder = weaponButtonHolders[i];
                break;
            }
        }
        return weaponButtonHolder;
    }

    private void CheckCoin()
    {
        var currentCoin = UIController.instance.CurrentCoin;
        if (currentCoin < _weaponPrice)
            addWeaponButton.interactable = false;
        else
            addWeaponButton.interactable = true;
    }

    private void OnLevelCompelet(Level level)
    {
        _canAimMove = false;
        addWeaponButton.interactable = false;
    }
    private void OnLevelFail(Level level)
    {
        _canAimMove = false;
        addWeaponButton.interactable = false;
    }
    private void OnAddCoin() 
    {
        CheckCoin();
    }
}
