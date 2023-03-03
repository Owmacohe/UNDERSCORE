using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class SignController : MonoBehaviour
{
    [SerializeField] Vector2 heightRange = new Vector2(3, 7);
    [SerializeField] GameObject marker;
    
    void Start()
    {
        Transform pole = transform.GetChild(0).transform;
        float height = Random.Range(heightRange.x, heightRange.y);
        
        pole.localPosition = Vector3.up * height;
        pole.localScale = new Vector3(0.5f, height, 0.5f);

        for (int i = 0; i < Random.Range(1, height - 1); i++)
            Instantiate(
                marker,
                transform.position + (Vector3.up * height / 3f) + (Vector3.up * i * 2),
                Quaternion.Euler(0, Random.Range(0f, 360f), 0),
                transform);
    }
}