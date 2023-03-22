using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class FlowerController : MonoBehaviour
{
    [SerializeField] Sprite[] flowers;

    Transform playerTransform;

    void Start()
    {
        transform.rotation = Quaternion.Euler(Vector3.right * 90);
        transform.position = new Vector3(transform.position.x, 0.02f, transform.position.z);

        playerTransform = GameObject.FindWithTag("Player").transform;
        
        GetComponent<SpriteRenderer>().sprite = flowers[Random.Range(0, flowers.Length)];
    }

    void FixedUpdate()
    {
        float max = 0.04f;
        float size = -0.0025f * Vector3.Distance(transform.position, playerTransform.position) + max;

        if (size > max) size = max;
        if (size < 0) size = 0;
        
        transform.localScale = Vector3.one * size;
    }
}