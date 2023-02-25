using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EyeFollow : MonoBehaviour
{
    [SerializeField] Transform eyes;
    [SerializeField] Vector2 lookAmount = new Vector2(0.05f, 0.1f);
    [SerializeField] float blinkInterval = 3;
    [SerializeField] bool follow = true;
    [SerializeField] bool blink = true;

    Vector3 startPosition;
    bool isBlinking;

    void Start()
    {
        startPosition = eyes.localPosition;
        
        if (blink) Invoke(nameof(Blink), Random.Range(blinkInterval, blinkInterval * 2));
    }

    void FixedUpdate()
    {
        if (follow)
        {
            Vector3 normalizedPosition = new Vector2(
                -lookAmount.x * ((Input.mousePosition.x / Screen.width) - 0.5f),
                lookAmount.y * ((Input.mousePosition.y / Screen.height) - 0.5f));

            eyes.localPosition = startPosition + normalizedPosition;
        }
    }

    void Blink()
    {
        Vector3 localScale = eyes.localScale;
        eyes.localScale = new Vector3(localScale.x, localScale.y * (isBlinking ? 2 : 0.5f), localScale.z);
        
        isBlinking = !isBlinking;

        Invoke(nameof(Blink), isBlinking ? 0.5f : Random.Range(blinkInterval, blinkInterval * 2));
    }
}