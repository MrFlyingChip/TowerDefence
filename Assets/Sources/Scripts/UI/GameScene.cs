using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameScene : MonoBehaviour
{
    [Header("UI")] 
    public GameObject TrackingLostPanel;
    public GameObject MainMenuPanel;
    public GameObject GameUIPanel;

    public Button QuitButton;
    public Button StartGameButton;

    public Button StartNewWaveButton;
    
    [Header("Game Animator")]
    public GameAnimator Animator;

    bool gameStarted = false;
    
    void Awake()
    {
        GameTrackableEventHandler.onTrackingLost.AddListener(OnTrackingLost);
        GameTrackableEventHandler.onTrackingFound.AddListener(OnTrackingFound);
        
        QuitButton.onClick.AddListener(OnQuitButtonClick);
        StartGameButton.onClick.AddListener(OnStartClick);
        StartNewWaveButton.onClick.AddListener(OnStartNewWaveClick);

        Animator.Init();
    }

    void Start()
    {
        ShowTrackingLostUI();
    }
    
    void OnStartClick()
    {
        gameStarted = true;
        GameManager.Instance.StartGame();
        Animator.StartAnimation();
        ShowGameUI();
        EnemyManager.Instance.OnWaveEnded.AddListener(OnWaveEnded);
        GameManager.Instance.OnGameOver.AddListener(OnGameOver);
    }

    void OnTrackingFound()
    {
        if (gameStarted)
        {
            ShowGameUI();
        }
        else
        {
            ShowMainMenu();
        }
    }

    void ShowMainMenu()
    {
        HideAllPanels();
        MainMenuPanel.SetActive(true);
    }

    void ShowGameUI()
    {
        HideAllPanels();
        GameUIPanel.SetActive(true);
    }

    void OnTrackingLost()
    {
        ShowTrackingLostUI();
    }

    void OnQuitButtonClick()
    {
        Application.Quit();
    }
    
    void ShowTrackingLostUI()
    {
        HideAllPanels();
        TrackingLostPanel.SetActive(true);
    }

    void HideAllPanels()
    {
        TrackingLostPanel.SetActive(false);
        MainMenuPanel.SetActive(false);
        GameUIPanel.SetActive(false);
    }

    void OnStartNewWaveClick()
    {
        if (gameStarted)
        {
            EnemyManager.Instance.StartWave();
            StartNewWaveButton.gameObject.SetActive(false);
        }
    }

    void OnWaveEnded()
    {
        StartNewWaveButton.gameObject.SetActive(true);
    }

    void OnGameOver()
    {
        gameStarted = false;
        EnemyManager.Instance.OnWaveEnded.RemoveListener(OnWaveEnded);
        GameManager.Instance.OnGameOver.RemoveListener(OnGameOver);
        ShowMainMenu();
    }
}
