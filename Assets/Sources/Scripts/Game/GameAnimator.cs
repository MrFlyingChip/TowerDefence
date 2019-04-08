using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameAnimator : MonoBehaviour
{
    public GameObject FieldObject;
    public GameObject TowerObject;
    public Transform BulletTransform;
    public Transform LeftBorder;
    public Transform RightBorder;

    private bool animationStarted = false;

    bool inited = false;

    public void Init()
    {
        if (inited)
        {
            return;
        }

        inited = true;

        GameTrackableEventHandler.onTrackingFound.AddListener(OnTargetFound);
        GameTrackableEventHandler.onTrackingLost.AddListener(OnTargetLost);
        TouchInput.OnTouchSignal.AddListener(PerformTouch);
        
        ShootingAnimator.Instance.Init(this, BulletTransform);
        ShootingAnimator.Instance.SignalOnBulletEndedFlying.AddListener(OnBulletEndedFlying);
        EnemyAnimator.Instance.Init(this, RightBorder, LeftBorder, TowerObject.transform);
    }
 
    public void StartAnimation()
    {
        animationStarted = true;
        ShowGame();
    }

    public void EndAnimation()
    {
        animationStarted = false;
    }

    void OnTargetLost()
    {
        HideGame();
    }

    void OnTargetFound()
    {
        if (animationStarted)
        {
            ShowGame();
        }
    }

    void HideGame()
    {
        FieldObject.SetActive(false);
        TowerObject.SetActive(false);
        EnemyAnimator.Instance.HideAllEnemies();
    }

    void ShowGame()
    {
        FieldObject.SetActive(true);
        TowerObject.SetActive(true);
        EnemyAnimator.Instance.ShowAllEnemies();
    }

    public GameObject CreateObject(GameObject prefab, Vector3 position, Transform lookAt = null)
    {
        GameObject result = null;
        if (lookAt)
        {
            result = Instantiate(prefab, transform);
            result.transform.position = position;
            result.transform.LookAt(lookAt);
            result.transform.localRotation = Quaternion.Euler(0, result.transform.localRotation.y, 0);
        }
        else
        {
            result = Instantiate(prefab, position, Quaternion.identity, transform);
            result.transform.localPosition = position;
            result.transform.localRotation = Quaternion.identity;
        }
        
        return result;
    }

    void OnBulletEndedFlying(Vector3 endPos)
    {
        EnemyAnimator.Instance.DamageEnemies(endPos);
    }

    void PerformTouch(Vector3 pos)
    {
        ShootingAnimator.Instance.PerformShoot(pos);
    }
}
