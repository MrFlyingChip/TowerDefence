using UnityEngine;
using UnityEngine.UI;

public class FadeEffect : MonoBehaviour
{
    public float startValue;
    public float endValue;
    public float time;
    
    Image image;
    float delta = 0;
    float currentValue = 0;

    private void OnEnable()
    {
        if (!image)
        {
            image = gameObject.GetComponent<Image>();
            if (!image)
            {
                image.gameObject.AddComponent<Image>();
            }
        }
        
        Color color = image.color;
        color.a = startValue;
        image.color = color;

        currentValue = 0;
        delta = Mathf.Abs(endValue - startValue) / time;
    }
    
    void Update()
    {
        currentValue += Time.deltaTime * delta;
        Color color = image.color;
        color.a = Mathf.Lerp(startValue, endValue, currentValue);
        image.color = color;
    }
}
