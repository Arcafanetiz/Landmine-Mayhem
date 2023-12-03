using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    // Attached Variables
    [Header("Attach GameObject")]
    [SerializeField] private GameObject SettingFrame;   // Setting Game Object
    [SerializeField] private GameObject CreditFrame;    // Credit Game Object

    // Encapsulated Variables
    private bool SettingOpen => SettingFrame.activeSelf;
    private bool CreditOpen => CreditFrame.activeSelf;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SettingOpen)
            {
                SettingFrame.SetActive(false);
            }
            else if (CreditOpen)
            {
                CreditFrame.SetActive(false);
            }
        }
    }

    // Play Game
    public void ActionPlay()
    {
        SceneLoader.Load(SceneLoader.Scene.Gameplay);
    }

    // Open Setting
    public void ActionSetting()
    {
        if (!SettingOpen)
        {
            SettingFrame.SetActive(true);
            CreditFrame.SetActive(false);
        }
        else
        {
            SettingFrame.SetActive(false);
        }
    }

    // Open Credit
    public void ActionCredit()
    {
        if (!CreditOpen)
        {
            CreditFrame.SetActive(true);
            SettingFrame.SetActive(false);
        }
        else
        {
            CreditFrame.SetActive(false);
        }
    }

    // Exit Game
    public void ActionExit()
    {
        Application.Quit();
    }
}