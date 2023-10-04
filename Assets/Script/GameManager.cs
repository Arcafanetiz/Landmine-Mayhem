using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class GameManager: MonoBehaviour
{
    public bool debugComplete = false;

    [HideInInspector] public bool gameComplete = false;
    public int sessionLength = 300;
    public float timer = 0.0f;

    public float enemySpeed;
    public float enemySpeedStart = 1.0f;
    public float enemySpeedMax = 5.0f;

    private GameObject character;
    private GameObject gameoverUI;
    private GameObject timerUI;
    private GameObject enemyHolder;
    private GameObject trapHolder;

    private EnemySpawner spawner;
    private int lastSecondSpawned;
    private void Start()
    {
        character   = GameObject.Find("Character");
        gameoverUI  = GameObject.Find("Gameover");
        timerUI     = GameObject.Find("Timer");
        enemyHolder = GameObject.Find("EnemyHolder");
        trapHolder  = GameObject.Find("TrapHolder");

        spawner = enemyHolder.GetComponent<EnemySpawner>();
        timer = sessionLength;
        gameoverUI.SetActive(false);
    }
    void Update()
    {
        if (!character)
        {
            DoGameover();
        }
        else
        {
            timer -= Time.deltaTime;
            int seconds = (int)(timer % 60);
            int minutes = (int)(timer / 60);
            string timerString = string.Format("{0}:{1:00}", minutes, seconds);
            timerUI.transform.Find("TextLabel").GetComponent<Text>().text = timerString;
            if ((timer <= 0) && (gameComplete == false))
            {
                DoWin();
            }
            if ((debugComplete == true) && (gameComplete == false))
            {
                DoWin();
            }
            enemySpeed = Mathf.Lerp(1.0f, enemySpeedMax / enemySpeedStart, 1.0f - (timer / sessionLength));
            if (lastSecondSpawned != seconds)
            {
                spawner.SpawnEnemy();
            }
            lastSecondSpawned = seconds;
        }
    }
    public void DoWin()
    {
        CompleteGame();
        gameoverUI.transform.Find("TextLabel").GetComponent<Text>().text = "YOU WIN!";
        gameoverUI.SetActive(true);
        timerUI.SetActive(false);
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
        enemyHolder.SetActive(false);
        trapHolder.SetActive(false);
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