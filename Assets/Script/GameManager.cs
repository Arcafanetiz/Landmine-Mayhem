using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager: MonoBehaviour
{
    // Static Variables
    [HideInInspector] public static float EnemySpeed;   // Speed of Enemies
    [HideInInspector] public static bool GamePaused;    // Pause Game State

    // External Variables
    [HideInInspector] public bool gameComplete = false; // Game is Completed
    [HideInInspector] public float timer = 0.0f;        // Game Timer

    // Attached Variables
    [Header("Attach GameObject")]
    [SerializeField] private GameObject SettingFrame;       // Setting UI Frame
    [SerializeField] private GameObject victorySFXPrefab;   // Victory Sound
    [SerializeField] private Transform mainCharacter;       // Main Character
    [SerializeField] private GameObject enemyContainer;     // Enemy Container
    [SerializeField] private GameObject trapContainer;      // Trap Container
    [SerializeField] private GameObject medkitContainer;    // Medkit Container
    [SerializeField] private GameObject objectiveUI;        // Objective Interface
    [SerializeField] private GameObject gameoverUI;         // Gameover Interface
    [SerializeField] private GameObject timerUI;            // Timer Interface
    [SerializeField] private GameObject pauseUI;            // Pause Interface
    [SerializeField] private TextMeshProUGUI gameoverText;  // Gameover Text
    [SerializeField] private TextMeshProUGUI timerText;     // Timer Text

    // Time Settings
    [Header("Time Settings")]
    [SerializeField] private int sessionLength = 300;   // How long a session lasts in seconds

    // Enemy Speed Settings
    [Header("Enemy Speed Range Settings")]
    [SerializeField] private float enemySpeedStart = 1.0f;  // Speed of the enemy from start
    [SerializeField] private float enemySpeedMax = 5.0f;    // Speed of the enemy to end

    // Reference Variables
    private EnemySpawner enemySpawner;
    private MedkitSpawner medkitSpawner;

    // Encapsulated Variables
    private float TimeLengthToEnd => 1 - (timer / sessionLength);
    private bool SettingOpen => SettingFrame.activeSelf;

    // Internal Variables
    private int lastSecondSpawned = 0;
    private int lastMinuteMedkit = 0;

    // Constant Variables
    private const string TimerFormat = "{0}:{1:00}";
    private const string WinnerText = "YOU WIN!";
    private const int SecondPerMinute = 60;

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
            if ((timer <= 0) && !gameComplete)
            {
                DoWin();
            }

            // Increase enemy speed from max session length to win time
            EnemySpeed = Mathf.Lerp(enemySpeedStart, enemySpeedMax, TimeLengthToEnd);

            // Spawns enemy every second
            if (lastSecondSpawned != seconds)
            {
                enemySpawner.SpawnEnemy();
            }
            lastSecondSpawned = seconds;

            // Spawns medkit every minute
            if (seconds == 0 && lastMinuteMedkit != minutes && timer < 290)
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
                if (!SettingOpen)
                {
                    ResumeGame();
                }
                else
                {
                    SettingFrame.SetActive(false);
                }
            }
        }
    }

    // Main function for winning
    private void DoWin()
    {
        GameObject victoryNoise = Instantiate(victorySFXPrefab);
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
        objectiveUI.SetActive(false);
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

    // Open Setting
    public void ActionSetting()
    {
        if (!SettingOpen)
        {
            SettingFrame.SetActive(true);
        }
    }

    // Return to main menus
    public void ActionMenu()
    {
        SceneLoader.Load(SceneLoader.Scene.MainMenu);
    }
}