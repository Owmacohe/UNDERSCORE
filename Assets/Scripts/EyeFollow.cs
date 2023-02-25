using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class EyeFollow : MonoBehaviour
{
    [SerializeField] Transform eyes;
    [SerializeField] Vector2 lookAmount = new Vector2(0.05f, 0.1f);

    Vector3 startPosition;
    bool isBlinking;

    void Start()
    {
        startPosition = eyes.localPosition;
        
        Invoke(nameof(Blink), 5);
    }

    void FixedUpdate()
    {
        Vector3 normalizedPosition = new Vector2(
            -lookAmount.x * ((Input.mousePosition.x / Screen.width) - 0.5f),
            lookAmount.y * ((Input.mousePosition.y / Screen.height) - 0.5f));

        eyes.localPosition = startPosition + normalizedPosition;
    }

    void Blink()
    {
        Vector3 localScale = eyes.localScale;
        eyes.localScale = new Vector3(localScale.x, localScale.y * (isBlinking ? 2 : 0.5f), localScale.z);
        
        isBlinking = !isBlinking;

        Invoke(nameof(Blink), isBlinking ? 0.5f : Random.Range(5f, 10f));
    }
}