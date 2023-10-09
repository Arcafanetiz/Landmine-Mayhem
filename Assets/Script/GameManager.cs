using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class GameManager: MonoBehaviour
{
    [Header("Runtime Debug")]
    public bool debugComplete = false;

    [HideInInspector] public bool gameComplete = false;
    [HideInInspector] public float timer = 0.0f;
    [Header("Setting Time Settings")]
    public int sessionLength = 300;

    [HideInInspector] public float enemySpeed;
    [Header("Enemy Speed Range Settings")]
    public float enemySpeedStart = 1.0f;
    public float enemySpeedMax = 5.0f;

    private GameObject character;
    private GameObject gameoverUI;
    private GameObject timerUI;
    private GameObject enemyHolder;
    private GameObject trapHolder;

    private EnemySpawner spawner;
    private int lastSecondSpawned;

    private readonly string TIMERFORMAT = "{0}:{1:00}";
    private readonly string WINNERTEXT = "YOU WIN!";
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
            //Do game over if character dies
            DoGameover();
        }
        else
        {
            //Run timer
            timer -= Time.deltaTime;
            int seconds = (int)(timer % 60);
            int minutes = (int)(timer / 60);
            //Apply timer to interface
            string timerString = string.Format(TIMERFORMAT, minutes, seconds);
            timerUI.transform.Find("TextLabel").GetComponent<Text>().text = timerString;
            //Game State based on timer
            if ((timer <= 0) && (gameComplete == false))
            {
                DoWin();
            }
            if ((debugComplete == true) && (gameComplete == false))
            {
                DoWin();
            }
            //Increase enemy speed from max session length to win time
            enemySpeed = Mathf.Lerp(1.0f, enemySpeedMax / enemySpeedStart, 1.0f - (timer / sessionLength));
            //Spawns enemy every second
            if (lastSecondSpawned != seconds)
            {
                spawner.SpawnEnemy();
            }
            lastSecondSpawned = seconds;
        }
    }
    //Main function for winning
    public void DoWin()
    {
        CompleteGame();
        gameoverUI.transform.Find("TextLabel").GetComponent<Text>().text = WINNERTEXT;
        gameoverUI.SetActive(true);
        timerUI.SetActive(false);
    }
    //Main function for losing
    public void DoGameover()
    {
        CompleteGame();
        gameoverUI.SetActive(true);
    }
    //Sub function complete game, used by both win/lost
    private void CompleteGame()
    {
        gameComplete = true;
        Cursor.lockState = CursorLockMode.None;
        enemyHolder.SetActive(false);
        trapHolder.SetActive(false);
    }
    //Restart game
    public void ActionRestart()
    {
        
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    //Return to main menus
    public void ActionMenu()
    {
        Debug.Log("Hi! You wanted to go to the menu, but there's not one yet!");
    }
}