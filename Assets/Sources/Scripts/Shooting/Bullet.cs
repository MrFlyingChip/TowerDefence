using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public SignalVector3 SignalOnEndFlying = new SignalVector3();
    private float maxTime;
    private float speed;
    private float gravity;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private float currentTime;
    private float distance;
    
    private bool endedFlying;
    
    public void Init(float acc, Vector3 startPos, Vector3 endPos)
    {
        startPosition = startPos;
        endPosition = endPos;
        gravity = acc;
        maxTime =  Mathf.Sqrt(2 * (startPosition.y - endPosition.y) / gravity);
        distance = Mathf.Sqrt(Mathf.Pow(endPosition.x - startPosition.x, 2) +
                              Mathf.Pow(endPosition.z - startPosition.z, 2));
        speed = distance / maxTime;
        transform.localPosition = startPos;
    }

    void Update()
    {
        if (endedFlying)
        {
            return;
        }
        
        currentTime += Time.deltaTime;

        if (currentTime >= maxTime)
        {
            endedFlying = true;
            SignalOnEndFlying.Invoke(endPosition);
            Destroy(gameObject);
        }

        float currentDistance = speed * currentTime / distance;
        float currentX = Mathf.Lerp(startPosition.x, endPosition.x, currentDistance);
        float currentZ = Mathf.Lerp(startPosition.z, endPosition.z, currentDistance);
        float currentY = startPosition.y - gravity * Mathf.Pow(currentTime, 2) / 2;
        
        transform.localPosition = new Vector3(currentX, currentY, currentZ);
    }

    private void OnDestroy()
    {
        SignalOnEndFlying.RemoveAllListeners();
    }
}
