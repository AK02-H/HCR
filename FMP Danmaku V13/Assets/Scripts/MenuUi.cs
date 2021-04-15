using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUi : MonoBehaviour
{
    [SerializeField] private CanvasGroup screenFadePanel;
    // Start is called before the first frame update
    void Start()
    {
        screenFadePanel.alpha = 0;
        screenFadePanel.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
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

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }
}
