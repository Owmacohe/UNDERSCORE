using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ImageFlash : MonoBehaviour
{
    [SerializeField] Sprite[] images;
    [Range(0, 1)] public float flashChance = 0.001f;

    SpriteRenderer rend;
    
    bool isShowing;
    float startShowingTime, maxShowingTime;
    
    Transform rendTransform;
    Vector3 startPosition;

    void Start()
    {
        rend = GetComponentInChildren<SpriteRenderer>();
        rendTransform = rend.transform;
        startPosition = rendTransform.localPosition;
    }
    
    void FixedUpdate()
    {
        if (!isShowing && Random.Range(0f, 1f) <= flashChance)
        {
            ShowRandomImage();
        }
        else if (isShowing)
        {
            if (Time.time % 1f == 0)
            {
                rendTransform.localPosition = startPosition + new Vector3(
                    Random.Range(-1f, 1f),
                    Random.Range(-1f, 1f),
                    0);
        
                rendTransform.rotation = Quaternion.Euler(0, 0, Random.Range(-15f, 15f));
            }

            if (Time.time - startShowingTime >= maxShowingTime)
            {
                rend.sprite = null;
                isShowing = false;   
            }
        }
    }

    void ShowRandomImage()
    {
        Sprite temp = images[Random.Range(0, images.Length)];
        rend.sprite = temp;

        isShowing = true;
        startShowingTime = Time.time;
        maxShowingTime = Random.Range(1f, 3f);
    }
}