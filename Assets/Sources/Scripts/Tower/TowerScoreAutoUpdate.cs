using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerScoreAutoUpdate : MonoBehaviour
{
    public Text text;
    
    void OnEnable(){
        TowerManager.Instance.OnTowerScoreChanged.AddListener(OnChangedTowerScore);

        OnChangedTowerScore(TowerManager.Instance.CurrentScore);
    }
    
    void OnDisable(){
        TowerManager.Instance.OnTowerScoreChanged.RemoveListener(OnChangedTowerScore);
    }
    
    void OnChangedTowerScore(int score)
    {
        text.text = score.ToString();
    }
}
