using System;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class ImageFlash : MonoBehaviour
{
    [Range(0, 1)] public float flashChance = 0.001f;
    [SerializeField] Vector2 maxShowingTimes = new Vector2(1, 3);
    [SerializeField] float rotateInterval = 1;

    Sprite[] images;
    Image img;
    
    bool isShowing;
    float startShowingTime, maxShowingTime;
    
    RectTransform imgTransform;

    void Start()
    {
        images = Resources.LoadAll<Sprite>("");
        
        img = GetComponentInChildren<Image>();
        imgTransform = img.GetComponent<RectTransform>();

        img.enabled = false;
    }
    
    void FixedUpdate()
    {
        if (!isShowing && Random.Range(0f, 1f) <= flashChance)
        {
            ShowRandomImage();
        }
        else if (isShowing)
        {
            if (Time.time % rotateInterval == 0)
            {
                imgTransform.anchoredPosition = new Vector3(
                    Random.Range(-100f, 100f),
                    Random.Range(-100f, 100f),
                    0);

                imgTransform.localRotation = Quaternion.Euler(0, 0, Random.Range(-15f, 15f));
            }

            if (Time.time - startShowingTime >= maxShowingTime)
            {
                img.sprite = null;
                img.enabled = false;
                isShowing = false;
            }
        }
    }

    void ShowRandomImage()
    {
        img.enabled = true;
        
        Sprite temp = images[Random.Range(0, images.Length)];
        img.sprite = temp;
        
        img.SetNativeSize();

        isShowing = true;
        startShowingTime = Time.time;
        maxShowingTime = Random.Range(maxShowingTimes.x, maxShowingTimes.y);
    }
}