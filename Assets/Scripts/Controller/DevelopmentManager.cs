using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DevelopmentManager : MonoBehaviour
{
    [SerializeField] private string password;
    [SerializeField] private int addCoinValue = 1000;
    [SerializeField] private GameObject developmentPanel;
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private Button activePanelButton; 
    [SerializeField] private Button closePanelButton;
    [SerializeField] private Button checkPasswordButton; 

    private int numberTouch;

    
    private void Awake()
    {
        activePanelButton.onClick.AddListener(OpenPanel);
        checkPasswordButton.onClick.AddListener(CheckPassword);
        closePanelButton.onClick.AddListener(ClosePanel);
    }

    private void OpenPanel()
    {
        numberTouch++;
        if (numberTouch >= 6)
        {
            developmentPanel.SetActive(true);
            numberTouch = 0;
        }
    }
    private void ClosePanel()
    {
        developmentPanel.SetActive(false);
    }

    private void CheckPassword()
    {
        if(inputField.text == password)
        {
        UIController.instance.AddCoin(addCoinValue);
        ClosePanel();
        return;
        }

        inputField.text = "";
    }
}
