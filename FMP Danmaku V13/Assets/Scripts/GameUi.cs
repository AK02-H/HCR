using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class GameUi : MonoBehaviour
{
    [SerializeField] private CanvasGroup continuePanel;
    [SerializeField] private CanvasGroup gameOverPanel;
    [SerializeField] private CanvasGroup pausePanel;
    [SerializeField] private GameObject pauseButtonPanel;
    [SerializeField] private CanvasGroup screenFadePanel;
    
    [SerializeField] private Text continueTimerText, creditText;
    [SerializeField] private GameObject scoreLabel, gradeLabel, gradeText;
    [SerializeField] private Text scoreText;

    [SerializeField] private Button resumeButton;
    
    
    // Start is called before the first frame update
    void Start()
    {
        pausePanel.alpha = 0;
        continuePanel.alpha = 0;
        gameOverPanel.alpha = 0;
        screenFadePanel.alpha = 0;

        continuePanel.gameObject.SetActive(false);
        gameOverPanel.gameObject.SetActive(false);
        screenFadePanel.gameObject.SetActive(false);

        scoreText.gameObject.SetActive(false);
        scoreText.text = 0.ToString();
        scoreLabel.SetActive(false);
        creditText.text = GameManager.Instance.continueCount.ToString();
        
        pausePanel.gameObject.SetActive(false);
        pauseButtonPanel.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            DisplayGameOver();
        }
    }

    public void DisplayPauseMenu()
    {
        resumeButton.GetComponent<Selectable>().Select();
        //resumeButton.targetGraphic.color = resumeButton.colors.selectedColor;
        pauseButtonPanel.transform.localScale = new Vector2(1, 0);
        pausePanel.gameObject.SetActive(true);
        pauseButtonPanel.SetActive(true);
        LeanTween.alphaCanvas(pausePanel, 1, .3f).setIgnoreTimeScale(true);
        LeanTween.scaleY(pauseButtonPanel, 1, .4f).setIgnoreTimeScale(true);
    }

    public void HidePauseMenu()
    {
        pauseButtonPanel.SetActive(false);
        pausePanel.gameObject.SetActive(false);
        pauseButtonPanel.transform.localScale = new Vector2(1, 0);
        pausePanel.alpha = 0;
    }
    
    public void DisplayContinue()
    {
        creditText.text = GameManager.Instance.continueCount.ToString();
        continuePanel.gameObject.SetActive(true);
        continuePanel.alpha = 1;
    }

    public void HideContinue()
    {
        continuePanel.gameObject.SetActive(false);
        continuePanel.alpha = 0;
    }

    public void SetTimer(string count)
    {
        continueTimerText.text = count;
        //Play sfx?
    }
    
    public void DisplayGameOver()
    {
        //LeanTween.alpha(gameOverPanel, 0, 2);
        gameOverPanel.gameObject.SetActive(true);
        scoreText.text = GameManager.Instance.Score.ToString();
        LeanTween.alphaCanvas(gameOverPanel, 1, 2);
        StartCoroutine(DisplayResults());
    }

    IEnumerator DisplayResults()
    {
        yield return new WaitForSeconds(3f);
        scoreLabel.SetActive(true);
        yield return new WaitForSeconds(1f);
        scoreText.gameObject.SetActive(true);
        GameManager.Instance.WaitForGameEndInput();
    }

    public void HideGameOver()
    {
        gameOverPanel.gameObject.SetActive(false);
    }
    
    public void ResumeGame()
    {
        GameManager.Instance.UnpauseGame();
    }
    
    public void ExitGame()
    {
        GameManager.Instance.QuitGame();
    }

    public void FadeOutScreen(float time = 1)
    {
        screenFadePanel.gameObject.SetActive(true);
        LeanTween.alphaCanvas(screenFadePanel, 1, time).setIgnoreTimeScale(true);;
    }
    
    public void FadeInScreen(float time = 1)
    {
        LeanTween.alphaCanvas(screenFadePanel, 0, time).setIgnoreTimeScale(true);
        //set gameobject inactive when done?
    }
}
