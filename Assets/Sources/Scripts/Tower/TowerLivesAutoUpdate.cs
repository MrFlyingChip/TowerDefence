using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerLivesAutoUpdate : MonoBehaviour
{
    public Slider slider;
    
    void OnEnable(){
        TowerManager.Instance.OnTowerLivesChanged.AddListener(OnChangedTowerLives);
        TowerManager.Instance.OnTowerMaxLivesChanged.AddListener(OnChangedMaxLives);

        OnChangedMaxLives(TowerManager.Instance.CurrentMaxLives);
        OnChangedTowerLives(TowerManager.Instance.CurrentLives);
    }
    
    void OnDisable(){
        TowerManager.Instance.OnTowerLivesChanged.RemoveListener(OnChangedTowerLives);
        TowerManager.Instance.OnTowerMaxLivesChanged.RemoveListener(OnChangedMaxLives);
    }
    
    void OnChangedMaxLives(int maxLives)
    {
        slider.maxValue = maxLives;
    }
    
    void OnChangedTowerLives(int lives)
    {
        slider.value = lives;
    }
}
