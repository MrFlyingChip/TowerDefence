using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager
{
    public readonly SignalInt OnTowerLivesChanged = new SignalInt();
    public readonly SignalInt OnTowerMaxLivesChanged = new SignalInt();
    public readonly SignalFloat OnTowerReloadTimeChanged = new SignalFloat();
    
    public readonly SignalInt OnTowerGoldChanged = new SignalInt();
    public readonly SignalInt OnTowerScoreChanged = new SignalInt();

    public const float DAMAGE_RADIUS = 0.2f;
    public const float MAX_TOWER_DAMAGE = 10f;
    public const float MIN_TOWER_DAMAGE = 2f;

    const int STARTING_LIVES = 100;
    const int ADD_LIVES_PER_LEVEL = 10;

    const float STARTING_RELOAD_TIME = 2f;
    const float DECREASE_RELOAD_TIME_PER_LEVEL = 0.1f;

    const int STARTING_GOLD = 0;
    const int STARTING_SCORE = 0;

    public const int LIVES_REFILL_PRICE = 20;
    public const int LIVES_LEVELUP_PRICE = 10;
    public const int RELOAD_LEVELUP_PRICE = 10;
    
    private int _currentMaxLives = STARTING_LIVES;
    private int _currentLives = STARTING_LIVES;

    private float _currentReloadTime = STARTING_RELOAD_TIME;

    private int _currentGold = STARTING_GOLD;
    private int _currentScore = STARTING_SCORE;
    
    public int CurrentLives
    {
        get { return _currentLives; }
        private set
        {
            if (value != _currentLives)
            {
                _currentLives = value;
                OnTowerLivesChanged.Invoke(_currentLives);
            }
        }
    }
    
    public int CurrentMaxLives
    {
        get { return _currentMaxLives; }
        private set
        {
            if (value != _currentMaxLives)
            {
                _currentMaxLives = value;
                OnTowerMaxLivesChanged.Invoke(_currentMaxLives);
            }
        }
    }
    
    public int CurrentGold
    {
        get { return _currentGold; }
        private set
        {
            if (value != _currentGold)
            {
                _currentGold = value;
                OnTowerGoldChanged.Invoke(_currentGold);
            }
        }
    }
    
    public int CurrentScore
    {
        get { return _currentScore; }
        private set
        {
            if (value != _currentScore)
            {
                _currentScore = value;
                OnTowerScoreChanged.Invoke(_currentScore);
            }
        }
    }
    
    public float CurrentReloadTime
    {
        get { return _currentReloadTime; }
        private set
        {
            _currentReloadTime = value;
            OnTowerReloadTimeChanged.Invoke(_currentReloadTime);
        }
    }

    public void Init()
    {
        CurrentLives = STARTING_LIVES;
        CurrentMaxLives = STARTING_LIVES;

        CurrentReloadTime = STARTING_RELOAD_TIME;

        CurrentGold = STARTING_GOLD;
        CurrentScore = STARTING_SCORE;
    }

    public bool CanBuyLivesLevelUp()
    {
        return CurrentGold >= LIVES_LEVELUP_PRICE;
    }
    
    public void AddLivesForLevelIfCan()
    {
        if (CanBuyLivesLevelUp())
        {
            CurrentGold -= LIVES_LEVELUP_PRICE;
            CurrentMaxLives += ADD_LIVES_PER_LEVEL;
            CurrentLives += ADD_LIVES_PER_LEVEL;   
        }
    }

    bool CanDecreaseReloadTime()
    {
        return CurrentReloadTime > 0;
    }

    public bool CanBuyReloadLevelUp()
    {
        return CurrentGold >= RELOAD_LEVELUP_PRICE && CanDecreaseReloadTime();
    }
    
    public void DecreaseReloadTimeForLevelIfCan()
    {
        if (CanBuyReloadLevelUp())
        {
            CurrentGold -= RELOAD_LEVELUP_PRICE;
            CurrentReloadTime -= DECREASE_RELOAD_TIME_PER_LEVEL;
            if (CurrentReloadTime < 0)
            {
                CurrentReloadTime = 0;
            }
        }
    }

    public bool CanRefillLives()
    {
        return CurrentGold >= LIVES_REFILL_PRICE;
    }

    public void RefillLivesIfCan()
    {
        if (CanRefillLives())
        {
            CurrentGold -= LIVES_REFILL_PRICE;
            CurrentLives = CurrentMaxLives;
        }
    }

    public void GetDamage(int damage)
    {
        CurrentLives -= damage;
        if (CurrentLives < 0)
        {
            CurrentLives = 0;
        }
    }

    public void GetScore(int score)
    {
        CurrentScore += score;
    }

    public void GetGold(int gold)
    {
        CurrentGold += gold;
    }
    
    private static TowerManager _instance = null;
    public static TowerManager Instance => _instance ?? (_instance = new TowerManager());
}
