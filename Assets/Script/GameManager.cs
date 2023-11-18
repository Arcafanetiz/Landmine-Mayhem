using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using UnityEngine.UI;

public class GameManager: MonoBehaviour
{
    // Static Variables
    [HideInInspector] public static float EnemySpeed;
    [HideInInspector] public static bool GamePaused;

    // External Variables
    [HideInInspector] public bool gameComplete = false;
    [HideInInspector] public float timer = 0.0f;

    // Runtime Debug
    [Header("Runtime Debug")]
    [SerializeField] private bool debugComplete = false;

    // Attach GameObject
    [Header("Attach GameObject")]
    [SerializeField] private Transform mainCharacter;
    [SerializeField] private GameObject enemyContainer;
    [SerializeField] private GameObject trapContainer;
    [SerializeField] private GameObject medkitContainer;
    [SerializeField] private GameObject gameoverUI;
    [SerializeField] private GameObject timerUI;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private TextMeshProUGUI gameoverText;
    [SerializeField] private TextMeshProUGUI timerText;

    // Time Settings
    [Header("Time Settings")]
    [SerializeField] private int sessionLength = 300;

    // Enemy Speed Settings
    [Header("Enemy Speed Range Settings")]
    [SerializeField] private float enemySpeedStart = 1.0f;
    [SerializeField] private float enemySpeedMax = 5.0f;

    // Referernce Variables
    private EnemySpawner enemySpawner;
    private MedkitSpawner medkitSpawner;

    // Internal Variables
    private int lastSecondSpawned = 0;
    private int lastMinuteMedkit = 0;

    // Constant Variables
    private const string TimerFormat = "{0}:{1:00}";
    private const string WinnerText = "YOU WIN!";
    private const int SecondPerMinute = 60;
    private const int Zero = 0;
    private const int One = 1;

    private void Awake()
    {
        enemySpawner = GetComponent<EnemySpawner>();
        medkitSpawner = GetComponent<MedkitSpawner>();
    }

    private void Start()
    {
        timer = sessionLength;
        gameoverUI.SetActive(false);
        ResumeGame();
    }

    private void Update()
    {
        if (!mainCharacter)
        {
            // Do game over if character dies
            DoGameover();
        }
        else if (!GamePaused)
        {
            // Run timer
            timer -= Time.deltaTime;
            int seconds = (int)(timer % SecondPerMinute);
            int minutes = (int)(timer / SecondPerMinute);

            // Apply timer to interface
            string timerString = string.Format(TimerFormat, minutes, seconds);
            timerText.text = timerString;

            // Game State based on timer
            if ((timer <= Zero) && !gameComplete)
            {
                DoWin();
            }
            if (debugComplete && !gameComplete)
            {
                DoWin();
            }

            // Increase enemy speed from max session length to win time
            EnemySpeed = Mathf.Lerp(enemySpeedStart, enemySpeedMax, One - (timer / sessionLength));

            // Spawns enemy every second
            if (lastSecondSpawned != seconds)
            {
                enemySpawner.SpawnEnemy();
            }
            lastSecondSpawned = seconds;

            // Spawns medkit every minute
            if (seconds == 0 && lastMinuteMedkit != minutes)
            {
                medkitSpawner.SpawnMedkit();
                lastMinuteMedkit = minutes;
            }
        }
        // Game Pausing
        if (Input.GetKeyDown(KeyCode.Escape) && !gameComplete)
        {
            if (!GamePaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
    }

    // Main function for winning
    private void DoWin()
    {
        CompleteGame();
        gameoverText.text = WinnerText;
        gameoverUI.SetActive(true);
        timerUI.SetActive(false);
    }

    // Main function for losing
    private void DoGameover()
    {
        CompleteGame();
        gameoverUI.SetActive(true);
    }

    // Sub function complete game, used by both win/lost
    private void CompleteGame()
    {
        gameComplete = true;
        Cursor.lockState = CursorLockMode.None;
        enemyContainer.SetActive(false);
        trapContainer.SetActive(false);
        medkitContainer.SetActive(false);
    }

    // Internal Function, Pause
    private void PauseGame()
    {
        GamePaused = true;
        Time.timeScale = 0;
        AudioListener.pause = true;
        Cursor.lockState = CursorLockMode.None;
        pauseUI.SetActive(true);
        timerUI.SetActive(false);
    }

    // Internal Function, Resume
    private void ResumeGame()
    {
        GamePaused = false;
        Time.timeScale = 1;
        AudioListener.pause = false;
        Cursor.lockState = CursorLockMode.Locked;
        pauseUI.SetActive(false);
        timerUI.SetActive(true);
    }

    // Resume game
    public void ActionResume()
    {
        ResumeGame();
    }

    // Restart game
    public void ActionRestart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Return to main menus
    public void ActionMenu()
    {
        SceneLoader.Load(SceneLoader.Scene.MainMenu);
    }
}