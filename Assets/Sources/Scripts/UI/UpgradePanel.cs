using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UpgradePanel : MonoBehaviour
{
    public Button upgradeHealthButton;
    public Button upgradeReloadTimeButton;
    public Button refillLivesButton;

    private void Start()
    {
        upgradeHealthButton.onClick.AddListener(OnUpgradeHealthClick);
        upgradeReloadTimeButton.onClick.AddListener(OnUpgradeReloadTimeClick);
        refillLivesButton.onClick.AddListener(OnRefillLivesClick);
    }

    void OnEnable()
    {
        TowerManager.Instance.OnTowerGoldChanged.AddListener(OnTowerGoldChanged);
        OnTowerGoldChanged(0);
    }

    private void OnDisable()
    {
        TowerManager.Instance.OnTowerGoldChanged.RemoveListener(OnTowerGoldChanged);
    }

    void OnUpgradeHealthClick()
    {   
        TowerManager.Instance.AddLivesForLevelIfCan();
    }

    void OnUpgradeReloadTimeClick()
    {
        TowerManager.Instance.DecreaseReloadTimeForLevelIfCan();
    }

    void OnRefillLivesClick()
    {
        TowerManager.Instance.RefillLivesIfCan();
    }

    void OnTowerGoldChanged(int gold)
    {
        upgradeHealthButton.interactable = TowerManager.Instance.CanBuyLivesLevelUp();
        upgradeReloadTimeButton.interactable = TowerManager.Instance.CanBuyReloadLevelUp();
        refillLivesButton.interactable = TowerManager.Instance.CanRefillLives();
    }
}
