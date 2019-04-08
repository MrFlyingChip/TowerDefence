using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager
{
    public SignalVoid OnGameOver = new SignalVoid();
    bool inited = false;

    public void StartGame()
    {
        TowerManager.Instance.Init();
        EnemyManager.Instance.Init();

        if (!inited)
        {
            TowerManager.Instance.OnTowerLivesChanged.AddListener(OnTowerChangedLives);
            inited = true;
        }

        EnemyManager.Instance.StartWave();
    }

    void OnTowerChangedLives(int lives)
    {
        if (lives <= 0)
        {
            OnGameOver.Invoke();
        }
    }
    
    private static GameManager _instance = null;
    public static GameManager Instance => _instance ?? (_instance = new GameManager());
}
