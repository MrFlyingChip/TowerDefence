using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkController : MonoBehaviour
{
    public SignalInt SignalOnEnemyDamagedTower = new SignalInt();
    
    private Vector3 startPosition;
    private Vector3 endPosition;
    float distance;
    float currentTime;
    float maxTime;
    float speed;
    int enemyId;

    bool walkEnded = false;

    public void Init(Vector3 endPos, float walkSpeed, int id)
    {
        speed = walkSpeed;
        startPosition = transform.localPosition;
        endPosition = endPos;
        distance = Mathf.Sqrt(Mathf.Pow(endPosition.x - startPosition.x, 2) +
                              Mathf.Pow(endPosition.z - startPosition.z, 2));
        maxTime = distance / speed;
        currentTime = 0;
        enemyId = id;
    }

    private void Update()
    {
        if (walkEnded)
        {
            return;
        }

        currentTime += Time.deltaTime;
        if (currentTime >= maxTime)
        {
            walkEnded = true;
            SignalOnEnemyDamagedTower.Invoke(enemyId);
            Destroy(gameObject);
        }
        
        float currentDistance = speed * currentTime / distance;
        float currentX = Mathf.Lerp(startPosition.x, endPosition.x, currentDistance);
        float currentZ = Mathf.Lerp(startPosition.z, endPosition.z, currentDistance);
        
        transform.localPosition = new Vector3(currentX, startPosition.y, currentZ);
    }

    private void OnDestroy()
    {
        SignalOnEnemyDamagedTower.RemoveAllListeners();
    }
}