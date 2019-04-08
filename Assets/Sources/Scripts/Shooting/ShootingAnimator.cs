using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingAnimator
{
    public SignalVector3 SignalOnBulletEndedFlying = new SignalVector3();
    const float Gravity = 2f;
    
    GameAnimator animator;
    Transform startTransform;
    float reloadTime;
    bool isReloading;

    public void Init(GameAnimator gameAnimator, Transform start)
    {
        animator = gameAnimator;
        startTransform = start;
        TowerManager.Instance.OnTowerReloadTimeChanged.AddListener(ChangeReloadTime);
        isReloading = false;
        ChangeReloadTime(TowerManager.Instance.CurrentReloadTime);
    }

    void ChangeReloadTime(float time)
    {
        reloadTime = time;
    }

    public void PerformShoot(Vector3 destination)
    {
        if (isReloading)
        {
            return;
        }  
        
        isReloading = true;
        SpawnAndAnimateBullet(destination);
        animator.StartCoroutine(Shoot());
    }

    IEnumerator Shoot()
    {
        yield return new WaitForSeconds(reloadTime);
        isReloading = false;
    }

    void SpawnAndAnimateBullet(Vector3 destination)
    {
        destination = animator.transform.InverseTransformPoint(destination);
        Vector3 startPoint = startTransform.localPosition;
        GameObject bulletObj = animator.CreateObject(PrefabHolder.Instance.Bullet, startPoint);

        Bullet bullet = bulletObj.GetComponent<Bullet>();
        if (!bullet)
        {
            return;
        }

        bullet.Init(Gravity, startPoint, destination);
        bullet.SignalOnEndFlying.AddListener(OnBulletEndedFlying);
    }

    void OnBulletEndedFlying(Vector3 endPos)
    {
        animator.CreateObject(PrefabHolder.Instance.ExplosionPrafab, endPos);
        SignalOnBulletEndedFlying.Invoke(endPos);
    }
    
    private static ShootingAnimator _instance = null;
    public static ShootingAnimator Instance => _instance ?? (_instance = new ShootingAnimator());
}
