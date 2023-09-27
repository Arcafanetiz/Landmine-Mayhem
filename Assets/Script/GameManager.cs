using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager: MonoBehaviour
{
    public bool debugComplete = false;

    [HideInInspector] public bool gameComplete = false;
    public int sessionLength = 300;
    public float timer = 0.0f;

    private GameObject gameoverUI;
    private GameObject enemyHolder;
    private GameObject trapHolder;
    private void Start()
    {
        gameoverUI = GameObject.Find("Gameover");
        enemyHolder = GameObject.Find("EnemyHolder");
        trapHolder = GameObject.Find("TrapHolder");
        timer = sessionLength;
        gameoverUI.SetActive(false);
    }
    void Update()
    {
        timer -= Time.deltaTime;
        int seconds = (int)(timer % 60);
        if ((seconds <= 0) && (gameComplete == false)) 
        {
            DoWin();
        }
        if ((debugComplete == true) && (gameComplete == false))
        {
            DoWin();
        }
    }
    public void DoWin()
    {
        CompleteGame();
        gameoverUI.transform.Find("TextLabel").GetComponent<Text>().text = "YOU WIN!";
        gameoverUI.SetActive(true);
    }
    public void DoGameover()
    {
        CompleteGame();
        gameoverUI.SetActive(true);
    }
    private void CompleteGame()
    {
        gameComplete = true;
        Cursor.lockState = CursorLockMode.None;
        Destroy(enemyHolder);
        Destroy(trapHolder);
    }
    public void ActionRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    public void ActionMenu()
    {
        Debug.Log("Hi! You wanted to go to the menu, but there's not one yet!");
    }
}