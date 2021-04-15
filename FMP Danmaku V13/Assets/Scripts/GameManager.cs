using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }


    public PlayerController player;
    public WaveManager waveController;
    
    public GameUi gameUi;
    
    private int score;

    public int continueCount = 0;
    public int maxPlayerLives = 3;


    private bool gamePaused = false;
    private bool gameIsActive = true;
    private bool waitingForContinue = false;
    private int continueTimer = 9;
    private bool waitForGameEndInput = false;
    
    public int Score
    {
        get
        {
            return score;
        }
        set
        {
            score = value;
        }
    }
    
    public Animator anim;
    
    public float timeScale;    //the animated timescale value

    [Header("Time Control")]
    public float easeInTime;

    public float easeSpeed;
    public float minTimeScale;
    private bool easingIn;


    private float timeScaleBeforePause;

    [SerializeField] private Text scoreText;
    [SerializeField] private Text livesText;
    
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        player = PlayerController.instance;
        waveController = WaveManager.Instance;
        continueTimer = 9;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameIsActive)
        {
            Time.timeScale = timeScale;
        }
        
        if (Input.GetKeyDown(KeyCode.M))
        {
            Debug.Log("space pressed");
            SlowDown();
        }
        
        if (Input.GetKeyDown(KeyCode.N))
        {
            Debug.Log("space pressed");
            ConfirmContinue();
        }

        if (waitingForContinue)
        {
            //every set amount of time decrease timer
            
            
            if (Input.GetKeyDown(KeyCode.Return))
            {
                StopCoroutine(WaitForContinueTimerDecrease());
                ConfirmContinue();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                StopCoroutine(WaitForContinueTimerDecrease());
                GameOver();
            }

            if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2"))
            {
                //Reduce continue countdown
                StopCoroutine(WaitForContinueTimerDecrease());
                DecreaseContinueTimer();
                StartCoroutine(WaitForContinueTimerDecrease());
            }
        }

        if (Input.GetButtonDown("Submit") || Input.GetButtonDown("Cancel") && !waitingForContinue && !waitForGameEndInput)
        {
            //TogglePause
            if(gamePaused)
                UnpauseGame();
            else
                PauseGame();
            
        }

        if (waitForGameEndInput)
        {
            if (Input.GetButtonDown("Fire1") || Input.GetButtonDown("Fire2") || Input.GetButtonDown("Confirm") ||
                Input.GetButtonDown("Cancel"))
            {
                QuitGame();
            }
        }
        
        
        if (gameIsActive)
        {
            scoreText.text = score.ToString();
            livesText.text = "Lives: " + player.LifeCount.ToString();
        }
    }


    public void PauseGame()
    {
        gameIsActive = false;
        Time.timeScale = 0;
        gamePaused = true;
        gameUi.DisplayPauseMenu();
    }

    public void UnpauseGame()
    {
        gameUi.HidePauseMenu();
        gamePaused = false;
        Time.timeScale = 1; //Needed?
        gameIsActive = true;
    }
    
    public void SlowDown()
    {
        Debug.LogWarning("STARTING SLOW");
        anim.SetTrigger("Hitslow");
    }

    public void EndSlowdown()
    {
        Debug.LogWarning("ENDING SLOW");
        anim.SetTrigger("EndHitslow");
    }


    public void HandleDeath(float delay)
    {
        Invoke("HandleDeath", delay);
    }
    
    private void HandleDeath()
    {
        //Time.timeScale = 0;

        AudioManager.instance.musicSource.Pause();
        gameIsActive = false;
        
        if (continueCount > 0)
        {
            WaitForContinue();
        }
        else
        {
            GameOver();
        }
    }
    
    public void GameOver(float delay)
    {
        Invoke("GameOver", delay);
    }
    
    private void GameOver()
    {
        //AudioManager.instance.musicSource.UnPause();
        //AudioManager.instance.musicSource.
        waitingForContinue = false;
        Time.timeScale = 1f;
        gameUi.HideContinue();
        gameUi.DisplayGameOver();
    }

    public void WaitForContinue()
    {
        Time.timeScale = 0;
        gameUi.DisplayContinue();
        continueTimer = 9;
        waitingForContinue = true;
        Debug.Log("WAIT FOR CONTINUE");
        //Invoke("DecreaseContinueTimer", 0f, 0.2f);
        StartCoroutine(WaitForContinueTimerDecrease());
    }

    void DecreaseContinueTimer()
    {
        Debug.Log("DECREASE CALLED");
        AudioManager.instance.PlaySfx(AudioManager.instance.sfx_UiBeep, 1f);
        if (continueTimer <= 0)
        {
            //Game Over
            StopCoroutine(WaitForContinueTimerDecrease());
            GameOver();
        }
        else
        {
            continueTimer--;
            gameUi.SetTimer(continueTimer.ToString());
            StartCoroutine(WaitForContinueTimerDecrease());
        }
        
    }

    IEnumerator WaitForContinueTimerDecrease()
    {
        yield return new WaitForSecondsRealtime(1.7f);
        DecreaseContinueTimer();
    }
    
    public void ConfirmContinue()
    {
        AudioManager.instance.PlaySfx(AudioManager.instance.sfx_UiSelect, 1f);
        AudioManager.instance.musicSource.UnPause();
        StopCoroutine(WaitForContinueTimerDecrease());
        gameUi.HideContinue();
        continueCount--;
        waitingForContinue = false;
        Time.timeScale = 1;     //necessary?
        player.LifeCount = maxPlayerLives;
        gameIsActive = true;
        player.Respawn();
        continueTimer = 9;
    }

    public void WaitForGameEndInput()
    {
        waitForGameEndInput = true;
    }
    
    public void QuitGame()
    {
        StartCoroutine(QuitGameRoutine());
    }
    
    public IEnumerator QuitGameRoutine()
    {
        //Fade out
        //Destroy player
        //Load start menu
        //Fade in
        //Destroy gamemanager

        gameIsActive = false;
        
        gameUi.FadeOutScreen(1.5f);
        yield return new WaitForSecondsRealtime(2f);
        gameUi.HidePauseMenu();
        gameUi.HideGameOver();
        Destroy(player.gameObject);
        Destroy(waveController.gameObject);
        SceneManager.LoadScene(0);
        Time.timeScale = 1f;
        gameUi.FadeInScreen(1.5f);
        yield return new WaitForSecondsRealtime(1.7f);
        Destroy(gameObject);
    }
}
