using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Elementary.Scripts.Data.Management;
using DG.Tweening;
using Elementary.Scripts.LevelManagement;
using UnityEngine.UI;
using UnityEngine.Events;

public class UIController : MonoBehaviour
{
    public static UIController instance;

    [SerializeField] private Canvas canvas;

    [Header("Coin")]
    [SerializeField] private TextMeshProUGUI coinText;
    [SerializeField] private TextMeshProUGUI coinFailText;
    [SerializeField] private TextMeshProUGUI coinCompeletText;
    [SerializeField] private int firstCoin;
    [SerializeField] private Transform coinPrefab;
    [SerializeField] private Transform coinTarget;
    [SerializeField] private int addCoinFail;
    [SerializeField] private int addCoinCompelet;

    [Header("Panel")]
    [SerializeField] private Panel[] Panels;
    [SerializeField] private float activeDelayWinPanel;
    [SerializeField] private float activeDelayLosePanel;
    [SerializeField] private TextMeshProUGUI levelText;

    [Header("Button")]
    [SerializeField] private Button nextLevelButton;
    [SerializeField] private Button resetLevelButton;

    private Dictionary<string, GameObject> _panelDic;
    public int CurrentCoin => DataManager.Get<int>("Coin");
    public UnityAction OnAddCoin;

    #region Unity Function
    private void Awake()
    {
        instance = this;

        LevelManager.OnLevelStart += OnLevelStart;
        LevelManager.OnLevelComplete += OnlevelCompelet;
        LevelManager.OnLevelFail += OnLevelFail;
    }
    private void Start()
    {
        if (CurrentCoin == default)
            DataManager.Save("Coin", firstCoin);
        coinText.text = CurrentCoin.ToString();

        _panelDic = new Dictionary<string, GameObject>();
        for (int i = 0; i < Panels.Length; i++)
            _panelDic.Add(Panels[i].PanelName, Panels[i].PanelObject);

        nextLevelButton.onClick.AddListener(() =>
        {
            AddCoin(addCoinCompelet);
            LevelManager.Instance.ResetLevel();
        });
        resetLevelButton.onClick.AddListener(() =>
        {
            AddCoin(addCoinFail);
            LevelManager.Instance.ResetLevel();
        });

        int level = DataManager.Get<int>("level-number", 1);
            levelText.text = "Level " + level;       
        
    }
    private void OnDestroy()
    {
        LevelManager.OnLevelStart -= OnLevelStart;
        LevelManager.OnLevelComplete -= OnlevelCompelet;
        LevelManager.OnLevelFail -= OnLevelFail;
    }
    #endregion

    #region CoinController
    public void AddCoin(int value) 
    {
        int coin = DataManager.Get<int>("Coin");
        coin += value;
        DataManager.Save("Coin", coin);

        coinText.text = CurrentCoin.ToString();

        OnAddCoin?.Invoke();
    }
    public void AddCoin(int value, Vector3 coinPoint) 
    {
        AddCoin(value);

        Vector3 point = Camera.main.WorldToScreenPoint(coinPoint);
        var coinObj = Instantiate(coinPrefab, point, Quaternion.identity, canvas.transform);
        coinObj.localScale = Vector3.one;
        coinObj.DOMove(coinTarget.position, 0.5f);
        Destroy(coinObj.gameObject, 0.5f);
    }
    #endregion

    #region PanelController
    public void ActivePanel(string panelName, float activeDelay = 0) => StartCoroutine(ActivePanelCo(panelName, activeDelay));
    private IEnumerator ActivePanelCo(string panelName, float activeDelay) 
    {
        yield return new WaitForSeconds(activeDelay);

        for (int i = 0; i < Panels.Length; i++)
        {
            Panels[i].PanelObject.SetActive(false);
        }

        GetPanel(panelName).SetActive(true);
    }
    public void InactivePanel(string panelName) => GetPanel(panelName).SetActive(false);
    public GameObject GetPanel(string panelName) => _panelDic[panelName];
    #endregion

    #region Events
    private void OnLevelStart(Level level) 
    {
    }
    private void OnlevelCompelet(Level level) 
    {
        coinCompeletText.text = addCoinCompelet.ToString();
        ActivePanel("WinPanel", activeDelayWinPanel);
    }
    private void OnLevelFail(Level level) 
    {
        coinFailText.text = addCoinFail.ToString();
        ActivePanel("LosePanel", activeDelayWinPanel);
    }
    #endregion

    #region Class's
    [System.Serializable]
    private class Panel 
    {
        public string PanelName;
        public GameObject PanelObject;
    }
    #endregion
}
