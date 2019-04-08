using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerGoldAutoUpdate : MonoBehaviour
{
    public Text text;
    
    void OnEnable(){
        TowerManager.Instance.OnTowerGoldChanged.AddListener(OnChangedTowerGold);

        OnChangedTowerGold(TowerManager.Instance.CurrentGold);
    }
    
    void OnDisable(){
        TowerManager.Instance.OnTowerGoldChanged.RemoveListener(OnChangedTowerGold);
    }
    
    void OnChangedTowerGold(int gold)
    {
        text.text = gold.ToString();
    }
}
