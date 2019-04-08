using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TouchInput : MonoBehaviour
{
    public static SignalVector3 OnTouchSignal = new SignalVector3();
    Camera mainCamera;
    RaycastHit hit;
    
    // Start is called before the first frame update
    void Start()
    {
        mainCamera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = mainCamera.ScreenPointToRay(touch.position);
                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform.CompareTag("Field"))
                    {
                        Vector3 pos = hit.point;
                        OnTouchSignal.Invoke(pos);
                    }
                }
            }
        }

        if (Input.GetMouseButton(0))
        {
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(ray, out hit))
            {
                if (hit.transform.CompareTag("Field"))
                {
                    Vector3 pos = hit.point;
                    OnTouchSignal.Invoke(pos);
                }
            }
        }
    }
}
