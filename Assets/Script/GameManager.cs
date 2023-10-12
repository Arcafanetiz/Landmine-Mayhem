using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class GameManager: MonoBehaviour
{
    [HideInInspector] public static float EnemySpeed;

    [HideInInspector] public bool gameComplete = false;
    [HideInInspector] public float timer = 0.0f;

    [Header("Runtime Debug")]
    [SerializeField] private bool debugComplete = false;
    [Header("Attach GameObject")]
    [SerializeField] private Transform mainCharacter;
    [SerializeField] private GameObject enemyHolder;
    [SerializeField] private GameObject trapHolder;
    [SerializeField] private GameObject gameoverUI;
    [SerializeField] private GameObject timerUI;
    [SerializeField] private Text gameoverTextLabel;
    [SerializeField] private Text timerTextLabel;
    [Header("Setting Time Settings")]
    [SerializeField] private int sessionLength = 300;
    [Header("Enemy Speed Range Settings")]
    [SerializeField] private float enemySpeedStart = 1.0f;
    [SerializeField] private float enemySpeedMax = 5.0f;

    private EnemySpawner spawner;
    private int lastSecondSpawned;

    private const string TimerFormat = "{0}:{1:00}";
    private const string WinnerText = "YOU WIN!";
    private const int SecondPerMinute = 60;
    private const int Zero = 0;
    private const int One = 1;
    private void Start()
    {
        spawner = GetComponent<EnemySpawner>();
        timer = sessionLength;
        gameoverUI.SetActive(false);
    }
    private void Update()
    {
        if (!mainCharacter)
        {
            //Do game over if character dies
            DoGameover();
        }
        else
        {
            //Run timer
            timer -= Time.deltaTime;
            int seconds = (int)(timer % SecondPerMinute);
            int minutes = (int)(timer / SecondPerMinute);
            //Apply timer to interface
            string timerString = string.Format(TimerFormat, minutes, seconds);
            timerTextLabel.text = timerString;
            //Game State based on timer
            if ((timer <= Zero) && !gameComplete)
            {
                DoWin();
            }
            if (debugComplete && !gameComplete)
            {
                DoWin();
            }
            //Increase enemy speed from max session length to win time
            EnemySpeed = Mathf.Lerp(enemySpeedStart, enemySpeedMax, One - (timer / sessionLength));
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
        gameoverTextLabel.text = WinnerText;
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